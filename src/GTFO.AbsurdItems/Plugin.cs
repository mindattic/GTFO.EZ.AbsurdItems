using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace GTFO.AbsurdItems;

[BepInPlugin(GUID, NAME, VERSION)]
[BepInProcess("GTFO.exe")]
[BepInDependency(GtfoEzGuid, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(AbsurdWeaponsGuid, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BasePlugin
{
    public const string GUID = "MindAttic.GTFO.AbsurdItems";
    public const string NAME = "GTFO.AbsurdItems";
    public const string VERSION = "1.0.0";

    // Optional, both: GTFO.EZ's datablock tweaks run before this mod's kit loot (the kits follow the
    // resource budget it sets), and GTFO.AbsurdWeapons' Loot Mix, when on, decides the loot instead.
    internal const string GtfoEzGuid = "MindAttic.GTFO.EZ";
    internal const string AbsurdWeaponsGuid = "MindAttic.GTFO.AbsurdWeapons";

    internal static ManualLogSource Logger = null!;

    public override void Load()
    {
        Logger = Log;
        ArtifactBuffs.Bind(Config);
        ConsumablesAreKits.Bind(Config);

        new Harmony(GUID).PatchAll();

        Logger.LogInfo($"{NAME} v{VERSION} loaded.");
    }
}
