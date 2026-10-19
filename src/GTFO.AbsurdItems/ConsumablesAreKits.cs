using System;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using GameData;
using HarmonyLib;
using MindAttic.GTFO.Shared;

namespace GTFO.AbsurdItems;

// Consumables found in lockers, boxes or lying around are a few small health, tool and ammo kits -
// the shared Shared/KitLoot.cs, published from GTFO.EZ - on top of the zone's normal resource packs.
// Moved here from GTFO.EZ with the same config keys and defaults. GTFO.EZ, when installed, leaves
// its own consumable spawn odds alone while this is on, and its datablock tweaks (the consumable
// spawn multiplier, the zone resource budget the kit count follows) run first.
[HarmonyPatch]
internal static class ConsumablesAreKits
{
    internal static ConfigEntry<bool> Enabled = null!;
    internal static ConfigEntry<float> KitLootPercent = null!;
    internal static ConfigEntry<float> HealthKitWeight = null!;
    internal static ConfigEntry<float> ToolKitWeight = null!;
    internal static ConfigEntry<float> AmmoKitWeight = null!;
    internal static ConfigEntry<int> KitChargesMin = null!;
    internal static ConfigEntry<int> KitChargesMax = null!;

    internal static void Bind(ConfigFile config)
    {
        const string s = "Loot";
        Enabled = config.Bind(s, "Consumables Are Kits", true,
            "Consumables found in lockers, boxes or lying around are small health, tool and ammo kits, and only a few of " +
            "them (see Kit Loot) - on top of the zone's normal resource packs, which are left as they are. Off: vanilla " +
            "consumables (retuned by GTFO.EZ's per-item weights, if that's installed). Takes effect on restart.");

        KitLootPercent = NonNegative.Bind(config, s, "Kit Loot (%)", 20f,
            "With Consumables Are Kits on: how much the kits found as loot add, in percent of a zone's normal health, ammo " +
            "and tool pack budget (averaged over the difficulty tiers). Default 20 - about a fifth more kits than the resource " +
            "packs alone. Takes effect on restart.");

        HealthKitWeight = NonNegative.Bind(config, s, "Health Kit Weight", 1f,
            "With Consumables Are Kits on: multiplies the odds a kit is a health kit. The odds already follow the zone's own health pack budget; 1 keeps that split. Takes effect on restart.");

        ToolKitWeight = NonNegative.Bind(config, s, "Tool Kit Weight", 1f,
            "With Consumables Are Kits on: multiplies the odds a kit is a tool kit. The odds already follow the zone's own tool pack budget; 1 keeps that split. Takes effect on restart.");

        AmmoKitWeight = NonNegative.Bind(config, s, "Ammo Kit Weight", 1f,
            "With Consumables Are Kits on: multiplies the odds a kit is an ammo kit. The odds already follow the zone's own ammo pack budget; 1 keeps that split. Takes effect on restart.");

        KitChargesMin = config.Bind(s, "Kit Charges Min", 1,
            "Fewest charges a kit found as loot holds. One charge is one use, 20% of a full pack. Takes effect on restart.");

        KitChargesMax = config.Bind(s, "Kit Charges Max", 2,
            "Most charges a kit found as loot holds. Takes effect on restart.");
    }

    // After GTFO.EZ's tweaks: its consumable spawn multiplier sets the tables' sizes and its
    // expedition balance the resource budget the kit count follows. Without it, this runs alone.
    [HarmonyPatch(typeof(GameDataInit), nameof(GameDataInit.Initialize))]
    [HarmonyPostfix]
    [HarmonyAfter(Plugin.GtfoEzGuid)]
    private static void Apply()
    {
        try
        {
            if (!Enabled.Value) return;
            if (AbsurdWeaponsLootMixOn())
            {
                Plugin.Logger.LogInfo("Loot: GTFO.AbsurdWeapons' Loot Mix is on, so it decides which consumables spawn - Consumables Are Kits is skipped.");
                return;
            }

            KitLoot.Fill(new KitLootSettings
            {
                LogName = "Consumables Are Kits",
                LootPercent = KitLootPercent.Value,
                MinCharges = KitChargesMin.Value,
                MaxCharges = KitChargesMax.Value,
                HealthWeight = HealthKitWeight.Value,
                ToolWeight = ToolKitWeight.Value,
                AmmoWeight = AmmoKitWeight.Value,
            });
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"GTFO.AbsurdItems couldn't set the kit loot: {e}");
        }
    }

    // With GTFO.AbsurdWeapons' Loot Mix on, its mix (kits, Hand Grenades, Stim Packs) is the
    // consumable loot, so this steps aside rather than filling the tables twice. Its setting is read
    // off its own config; if that can't be found, a loaded AbsurdWeapons counts as on (its default).
    private static bool AbsurdWeaponsLootMixOn()
    {
        try
        {
            if (!IL2CPPChainloader.Instance.Plugins.TryGetValue(Plugin.AbsurdWeaponsGuid, out var info)) return false;
            if (info.Instance is BasePlugin plugin && plugin.Config.TryGetEntry<bool>("Loot", "Loot Mix Enabled", out var enabled))
                return enabled.Value;
            return true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogWarning($"Loot: couldn't check for GTFO.AbsurdWeapons, applying this mod's kit loot. {e.Message}");
            return false;
        }
    }
}
