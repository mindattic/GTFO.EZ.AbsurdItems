# GTFO.AbsurdItems

*(Listed on Thunderstore as `GtfoAbsurdItems`. Package names there can only use letters, numbers
and underscores, so they can't contain a period.)*

A BepInEx plugin for GTFO that changes items and pickups: artifacts buff whoever picks them up for
the rest of the level, and the consumables found in lockers, boxes and lying around are a few small
health, tool and ammo kits. No game files are shipped or edited.

**Every feature has its own on/off switch, and its numbers can be retuned,** in the config file
BepInEx creates on first launch: `BepInEx\config\MindAttic.GTFO.AbsurdItems.cfg`.

## What it changes

| Feature | Vanilla | Default | Switch |
|---|---|---|---|
| [Artifact buffs](#artifact-buffs) (Bold / Aggressive / Muted) | count toward boosters only | +5% max health / +5% damage / 5% harder to detect, each, for the level | `[Artifacts] Artifact Buffs Enabled` |
| [Consumables are kits](#consumables-are-kits) (lockers, boxes, lying around) | mixed items | a few small health, tool and ammo kits, 1-2 charges each, adding about 20% to the zone's resource packs | `[Loot] Consumables Are Kits` |
| [Infinite ammo / health](#infinite-ammo-and-health) | none | **Insert** / **Delete** toggles, off at the start | - |

Both features moved here from GTFO.EZ, with the same settings and defaults.

## Artifact buffs

Picking up an artifact buffs whoever picked it up until the level ends: each **Bold** (the game's
Uncommon) adds max health, healed too; each **Aggressive** (Rare) adds damage to enemies, guns and
melee; each **Muted** (Common) makes you harder for enemies to detect, capped at 75%. A checkpoint
restart that puts an artifact back in the level takes its buff off again. Every player's pickups
count on every game, so the host's enemies hear the Muted and each player's own hits carry the
Aggressive; max health only changes on your own game.

### `[Artifacts]`

| Setting | Default | What it does |
|---|---|---|
| `Artifact Buffs Enabled` | true | Turns artifact buffs on or off. |
| `Bold: Max Health (%)` | 5 | Max health added per Bold artifact, in percent of your starting max health. |
| `Aggressive: Damage (%)` | 5 | Damage to enemies added per Aggressive artifact. |
| `Muted: Quieter (%)` | 5 | How much harder you are to detect per Muted artifact (capped at 75%). |

## Consumables are kits

Consumables found in lockers, boxes or lying around are the game's own health, tool and ammo packs,
as small kits of 1 or 2 charges (one charge is one use, 20% of a pack), and sparse: together they
add about 20% (*Kit Loot*) to the zone's normal resource packs, which spawn as usual. The split
between health, ammo and tool kits follows the zone's own resource budget. Turn it off for vanilla
consumables. The host's settings decide what spawns.

With **GTFO.EZ** installed, its data changes (consumable spawn multiplier, resource budget) apply
first, and its own consumable spawn odds are skipped while this is on. With **GTFO.AbsurdWeapons**
installed and its Loot Mix on, that mix (kits plus Hand Grenades and Stim Packs) is the consumable
loot instead and this is skipped.

### `[Loot]`

| Setting | Default | What it does |
|---|---|---|
| `Consumables Are Kits` | true | Turns kit loot on or off. Off: vanilla consumables. |
| `Kit Loot (%)` | 20 | How much the kits add, in percent of a zone's normal health, ammo and tool pack budget (averaged over the difficulty tiers). |
| `Health Kit Weight` | 1 | Multiplies the odds a kit is a health kit (the zone's budget already sets the split; 1 keeps it). 0 leaves health kits out. |
| `Tool Kit Weight` | 1 | The same for tool kits. |
| `Ammo Kit Weight` | 1 | The same for ammo kits. |
| `Kit Charges Min` | 1 | Fewest charges a kit holds. |
| `Kit Charges Max` | 2 | Most charges a kit holds. |

Every kit setting takes effect on restart. With every kit weight at 0, no consumables spawn.

## Infinite ammo and health

**Insert** toggles infinite ammo (every gun and tool stays full, no reloads) and **Delete** infinite
health (no damage to you; bots and teammates are unaffected), each announced as a warden message.
Both start off. They're the same two keys every GTFO.* mod has, from one shared copy of the code
written in GTFO.EZ; with several of the mods installed, only one answers, so a press never toggles
twice.

## Compatibility

- **GTFO.EZ** used to have artifact buffs and *Consumables Are Kits*; both moved here. Running an
  older GTFO.EZ build alongside this mod buffs each artifact twice: switch one of them off in its
  config.
- **GTFO.AbsurdWeapons:** compatible. With its Loot Mix on, it decides the consumable loot; with it
  off, this mod's kits do. It leaves artifacts in place, so artifact buffs work with it installed.
- Artifact buffs haven't been confirmed in play yet. Muted scales the game's per-player detection
  modifier (`PlayerAgent.GetDetectionMod`); whether enemies consult it for ordinary movement noise
  is the part to check.

## Building

```powershell
.\build.ps1                         # builds into the "GTFO - Easy Mode DEV" mod-manager profile and zips package\ into dist\
.\build.ps1 -Profile "My Profile"   # another Thunderstore Mod Manager profile
```

Needs the .NET 6 SDK and a GTFO profile with BepInEx installed (BepInEx's `core` and `interop`
DLLs are the build references).

## Project layout

```
src/GTFO.AbsurdItems/
  Plugin.cs              BepInEx entry point: binds each feature's config, applies Harmony patches
  ArtifactBuffs.cs       Per-player artifact counts and the max health, damage and detection buffs
  ConsumablesAreKits.cs  Runs the shared kit loot at GameDataInit, after GTFO.EZ's tweaks
  LocalPlayerDamage.cs   Tells your own damage component from the host's bots'
  NonNegative.cs         Holds percentage and weight settings at 0 or more
package/                 Thunderstore manifest, icon (256x256, from logo.png), README and changelog
logo.png                 Full-size logo
```
