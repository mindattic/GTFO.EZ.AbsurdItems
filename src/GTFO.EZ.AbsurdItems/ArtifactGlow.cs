using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using GameData;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using LevelGeneration;
using UnityEngine;

namespace GTFO.EZ.AbsurdItems;

/// <summary>
/// Every artifact still lying in the level glows softly in its category's color - Aggressive (Rare)
/// red, Muted (Common) blue, Bold (Uncommon) green - and its own model is tinted to match, so which
/// buff it gives is visible from across the room, not just once you're close enough to read its
/// name. A scan every second finds newly spawned (or checkpoint-restored) artifacts; the light sits
/// as a child of the artifact itself, so it follows it and disappears with it on pickup, and comes
/// back on its own if a checkpoint restart puts the artifact back in the level.
/// </summary>
internal sealed class ArtifactGlow : MonoBehaviour
{
    private const float ScanSeconds = 1f;
    private const float Range = 2.5f;
    private const float Intensity = 1.1f;
    private const float TintStrength = 0.55f; // 0 = the model's own color, 1 = fully the glow color

    internal static ConfigEntry<bool> Enabled = null!;

    private static readonly Dictionary<ArtifactCategory, Color> Colors = new()
    {
        { ArtifactCategory.Rare, new Color(1f, 0.15f, 0.15f) },      // Aggressive: red
        { ArtifactCategory.Common, new Color(0.2f, 0.45f, 1f) },     // Muted: blue
        { ArtifactCategory.Uncommon, new Color(0.3f, 0.9f, 0.35f) }, // Bold: green
    };

    private readonly HashSet<IntPtr> seen = new();

    private float nextScanAt;

    public ArtifactGlow(IntPtr ptr) : base(ptr) { }

    internal static void Bind(ConfigFile config)
    {
        Enabled = config.Bind("Artifacts", "Artifact Glow Enabled", true,
            "Artifacts still lying in the level glow and are tinted in their category's color: Aggressive (Rare) red, " +
            "Muted (Common) blue, Bold (Uncommon) green - so which buff one gives is visible from across the room.");
    }

    private void Update()
    {
        try
        {
            if (GameStateManager.CurrentStateName != eGameStateName.InLevel)
            {
                seen.Clear();
                return;
            }
            if (!Enabled.Value || Time.time < nextScanAt) return;
            nextScanAt = Time.time + ScanSeconds;
            Scan();
        }
        catch (Exception e)
        {
            Plugin.Logger.LogWarning($"GTFO.EZ.AbsurdItems artifact glow failed: {e.Message}");
        }
    }

    [HideFromIl2Cpp]
    private void Scan()
    {
        foreach (var found in UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<ArtifactPickup_Core>()))
        {
            var artifact = found.TryCast<ArtifactPickup_Core>();
            if (artifact == null || !seen.Add(artifact.Pointer)) continue;
            if (!Colors.TryGetValue(artifact.m_artifactCategory, out var color)) continue;

            Light(artifact, color);
            Tint(artifact, color);
        }
    }

    [HideFromIl2Cpp]
    private static void Light(ArtifactPickup_Core artifact, Color color)
    {
        var go = new GameObject("GTFO.EZ.AbsurdItems Artifact Glow");
        go.transform.SetParent(artifact.transform, false);
        go.transform.localPosition = Vector3.up * 0.15f;
        var light = go.AddComponent<EffectLight>();
        light.Color = color;
        light.Range = Range;
        light.Intensity = Intensity;
        light.enabled = true;
    }

    // Tints every renderer on the artifact toward its category color, and nudges its emission the
    // same way if its shader has one - each renderer's own material instance, so only this one
    // artifact changes, never the whole prefab.
    [HideFromIl2Cpp]
    private static void Tint(ArtifactPickup_Core artifact, Color color)
    {
        foreach (var renderer in artifact.GetComponentsInChildren<Renderer>(true))
        {
            try
            {
                var material = renderer.material;
                material.color = Color.Lerp(material.color, color, TintStrength);
                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", color * 0.4f);
                }
            }
            catch (Exception)
            {
                // Some renderers' shaders have no _Color/_EmissionColor property; leave those be.
            }
        }
    }
}
