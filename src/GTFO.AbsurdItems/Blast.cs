using System;
using System.Collections.Generic;
using Agents;
using AK;
using AssetShards;
using FX_EffectSystem;
using Il2CppInterop.Runtime.Attributes;
using LevelGeneration;
using UnityEngine;

namespace GTFO.AbsurdItems;

// What the Hand Grenade's blast needs, ported from GTFO.AbsurdWeapons (where its grenade guns share
// the same look, sound and damage).

/// <summary>The blast's look and sound: the trip mine's explosion effect, a flash and the frag grenade bang.</summary>
internal static class Explosion
{
    private const string TripMineExplosion = "Assets/AssetPrefabs/FX_Effects/FX_Tripmine.prefab";
    private static FX_Pool? pool;
    private static bool effectFailed, effectLogged;

    internal static void Show(Vector3 position)
    {
        CellSound.Post(EVENTS.FRAGGRENADEEXPLODE, position);
        BurstFlash.Spawn(position + Vector3.up * 0.3f, new Color(1f, 0.55f, 0.2f), 10f, 10f, 0.35f);
        if (effectFailed) return;
        try
        {
            if (pool == null) pool = MineDeployerInstance_Detonate_Explosive.s_explosionPool; // Unity null: destroyed at level cleanup
            if (pool == null)
            {
                var prefab = AssetShardManager.GetLoadedAsset<GameObject>(TripMineExplosion);
                if (prefab != null) pool = FX_Manager.GetEffectPool(prefab);
            }
            if (!effectLogged)
            {
                effectLogged = true;
                Plugin.Logger.LogInfo($"GTFO.AbsurdItems grenade explosion effect: {(pool != null ? "trip mine explosion" : "none loaded (flash only)")}.");
            }
            pool?.AquireEffect()?.Play(null, position, Quaternion.identity);
        }
        catch (Exception e)
        {
            effectFailed = true;
            Plugin.Logger.LogWarning($"GTFO.AbsurdItems grenade explosion effect unavailable: {e.Message}");
        }
    }
}

/// <summary>
/// Damage and stagger to every living enemy within a radius that the center can see (not
/// through walls), once each through its limb nearest the center, and to every door lock in it.
/// Uses the bullet damage path, so it syncs; call it only on the attacker's own game.
/// </summary>
internal static class AreaDamage
{
    internal static void Apply(Vector3 center, float radius, float damage, float edgeDamageFraction, float staggerMulti,
        Agent source, uint gearCategoryId)
    {
        var nearestLimb = new Dictionary<IntPtr, (Dam_EnemyDamageLimb Limb, Vector3 Point, float Distance)>();
        foreach (var collider in Physics.OverlapSphere(center, radius, LayerManager.MASK_ENEMY_DAMAGABLE))
        {
            var limb = collider.GetComponent<Dam_EnemyDamageLimb>();
            var enemy = limb?.m_base?.Owner;
            if (limb == null || enemy == null || !enemy.Alive) continue;

            var point = collider.ClosestPoint(center);
            var distance = Vector3.Distance(center, point);
            if (nearestLimb.TryGetValue(enemy.Pointer, out var known) && known.Distance <= distance) continue;
            nearestLimb[enemy.Pointer] = (limb, point, distance);
        }

        // Lifted slightly off the floor so a low wall edge doesn't block the whole burst.
        var from = center + Vector3.up * 0.2f;
        foreach (var (limb, point, distance) in nearestLimb.Values)
        {
            if (Physics.Linecast(from, point, LayerManager.MASK_WORLD)) continue;

            var falloff = Formulas.AreaFalloff(distance, radius, edgeDamageFraction);
            var direction = (point - center).sqrMagnitude > 0.0001f ? (point - center).normalized : Vector3.up;
            limb.BulletDamage(damage * falloff, source, point, direction, -direction,
                allowDirectionalBonus: false, staggerMulti, 1f, gearCategoryId);
        }
        LockDamage.Radius(center, radius, damage, source);
    }
}

/// <summary>
/// Door locks (padlocks): vanilla bullets and swings break them through <see cref="LG_WeakLockDamage"/>,
/// but the blast only looks for enemy limbs, so without this it passes a lock by.
/// </summary>
internal static class LockDamage
{
    // Every lock within `radius` of `center` that it can see, once each.
    internal static void Radius(Vector3 center, float radius, float damage, Agent source)
    {
        var struck = new HashSet<IntPtr>();
        foreach (var collider in Physics.OverlapSphere(center, radius, ~0, QueryTriggerInteraction.Collide))
        {
            var target = collider.GetComponentInParent<LG_WeakLockDamage>();
            if (target == null || !struck.Add(target.Pointer)) continue;
            var point = collider.ClosestPoint(center);
            // The lock's own door is world geometry, so only a wall well short of the lock blocks it.
            var line = point - center;
            if (line.magnitude > 0.5f && Physics.Raycast(center, line.normalized, line.magnitude - 0.4f, LayerManager.MASK_WORLD)) continue;
            var direction = line.sqrMagnitude > 0.0001f ? line.normalized : Vector3.forward;
            try
            {
                target.BulletDamage(damage, source, point, direction, -direction);
            }
            catch (Exception e)
            {
                Plugin.Logger.LogWarning($"GTFO.AbsurdItems lock damage failed: {e.Message}");
            }
        }
    }
}

/// <summary>The lock melter's meshes, copied onto a Hand Grenade - just the look.</summary>
internal static class GrenadeModel
{
    private static readonly string[] LockMelterPrefabs =
    {
        "Assets/AssetPrefabs/Items/Consumables/LockMelter/Consumable_LockMelter_Pickup.prefab",
        "Assets/AssetPrefabs/Items/Consumables/LockMelter/Consumable_LockMelter.prefab",
        "Assets/AssetPrefabs/Items/Consumables/LockMelter/Consumable_LockMelter_FirstPerson.prefab",
    };

    private static bool logged;

    // None of the prefab's pickup or interaction scripts come along. Falls back to a plain capsule
    // if the prefab isn't loaded.
    internal static void Build(Transform parent)
    {
        string source = "fallback capsule";
        try
        {
            foreach (var path in LockMelterPrefabs)
            {
                var prefab = AssetShardManager.GetLoadedAsset<GameObject>(path);
                if (prefab == null) continue;
                var root = prefab.transform;
                var copied = 0;
                foreach (var filter in prefab.GetComponentsInChildren<MeshFilter>(true))
                {
                    var renderer = filter.GetComponent<MeshRenderer>();
                    if (filter.sharedMesh == null || renderer == null) continue;
                    var matrix = root.worldToLocalMatrix * filter.transform.localToWorldMatrix;
                    var part = new GameObject("Part");
                    part.transform.SetParent(parent, false);
                    part.transform.localPosition = matrix.GetColumn(3);
                    part.transform.localRotation = matrix.rotation;
                    part.transform.localScale = matrix.lossyScale;
                    part.AddComponent<MeshFilter>().sharedMesh = filter.sharedMesh;
                    part.AddComponent<MeshRenderer>().sharedMaterials = renderer.sharedMaterials;
                    copied++;
                }
                if (copied == 0) continue;
                source = $"{path} ({copied} meshes)";
                return;
            }

            var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            UnityEngine.Object.Destroy(capsule.GetComponent<Collider>());
            capsule.transform.SetParent(parent, false);
            capsule.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
        }
        finally
        {
            if (!logged)
            {
                logged = true;
                Plugin.Logger.LogInfo($"GTFO.AbsurdItems Hand Grenade model: {source}.");
            }
        }
    }
}

/// <summary>A short flash of the game's own light type at a blast, fading out.</summary>
internal sealed class BurstFlash : MonoBehaviour
{
    private EffectLight light = null!;
    private float peak, duration, elapsed;

    public BurstFlash(IntPtr ptr) : base(ptr) { }

    [HideFromIl2Cpp]
    internal static void Spawn(Vector3 position, Color color, float range, float intensity, float seconds)
    {
        var go = new GameObject("GTFO.AbsurdItems Flash");
        go.transform.position = position;
        var flash = go.AddComponent<BurstFlash>();
        flash.peak = intensity;
        flash.duration = seconds;
        flash.light = go.AddComponent<EffectLight>();
        flash.light.Color = color;
        flash.light.Range = range;
        flash.light.Intensity = intensity;
        flash.light.enabled = true;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            Destroy(gameObject);
            return;
        }
        light.Intensity = peak * (1f - elapsed / duration);
    }
}
