# GTFO.EZ.AbsurdItems

*(Listed on Thunderstore as `GtfoEZAbsurdItems`. Package names there can only use letters, numbers
and underscores, so they can't contain a period.)*

GTFO.EZ.AbsurdItems is a GTFO.EZ companion mod, built on [GTFO.EZ](https://github.com/mindattic/GTFO.EZ)'s core: it carries GTFO.EZ's shared code (the Insert/Delete infinite ammo and health toggles, and MessageHelper, which puts on-screen messages wherever they fit) and works with or without GTFO.EZ installed. It was called GTFO.AbsurdItems before its first release.

A BepInEx plugin for GTFO that changes items and pickups: artifacts buff whoever picks them up for
the rest of the level, two new consumables - the **Hand Grenade** and the **Stim Pack** - and the
consumables found in lockers, boxes and lying around are a few small health, tool and ammo kits plus
those two. No game files are shipped or edited.

**Every feature has its own on/off switch, and its numbers can be retuned,** in the config file
BepInEx creates on first launch: `BepInEx\config\MindAttic.GTFO.EZ.AbsurdItems.cfg`. Its first entry,
`[0. Mod] Enabled`, is the master switch: set it to `false` and the whole mod is off - nothing is
patched, no item is added, no hotkey or loot change - and the other GTFO.* mods treat it as not
installed.

## What it changes

| Feature | Vanilla | Default | Switch |
|---|---|---|---|
| [Artifact buffs](#artifact-buffs) (Bold / Aggressive / Muted) | count toward boosters only | +5% max health / +5% damage / 5% harder to detect, each, for the level | `[Artifacts] Artifact Buffs Enabled` |
| [Hand Grenade](#hand-grenade) (new consumable) | - | a lock melter thrown like a glowstick, blowing after 3 s; found as loot, up to 6 carried | `[Hand Grenade] Hand Grenade Enabled` |
| [Stim Pack](#stim-pack) (new consumable) | - | heals 33% of max health and can overheal to 200%, draining back down; +33% damage for 15 s | `[Stim Pack] Stim Pack Enabled` |
| [Consumables are kits](#consumables-are-kits) (lockers, boxes, lying around) | mixed items | a few small health, tool and ammo kits, 1-2 charges each, adding about 20% to the zone's resource packs | `[Loot] Consumables Are Kits` |
| [Loot mix](#loot-mix) | - | Hand Grenades and Stim Packs join the kits; 60% of the loot lies out in the open | `[Loot] Loot Mix Enabled` |
| [Infinite ammo / health](#infinite-ammo-and-health) | none | **Insert** / **Delete** toggles, off at the start | - |

Artifact buffs and *Consumables Are Kits* moved here from GTFO.EZ, the Hand Grenade, Stim Pack and
Loot Mix from GTFO.EZ.AbsurdWeapons - each with the same settings and defaults, except that the kit
settings both of those mods had are now one set (see [`[Loot]`](#loot)).

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
consumables (and no Loot Mix). The host's settings decide what spawns.

With **GTFO.EZ** installed, its data changes (consumable spawn multiplier, resource budget) apply
first, and its own consumable spawn odds are skipped while this is on.

## Loot mix

On top of the kits, with *Loot Mix Enabled* (on by default): **Hand Grenades** at 4 to the kits' 3,
so grenades are common, and **Stim Packs** at 1 - both in the same lottery as the kits, so the kits
still add their 20% and these come on top. 60% of the loot (*OutInTheOpenShare*) lies out in the
open, like artifacts, instead of in boxes and lockers. Off: kits only, where the game puts them.
It's one kit-loot run either way, with one set of kit settings.

### `[Loot]`

All of it takes effect on restart.

| Setting | Default | What it does |
|---|---|---|
| `Consumables Are Kits` | true | Turns kit loot on or off. Off: vanilla consumables, and no Loot Mix. |
| `Kit Loot (%)` | 20 | How much the kits add, in percent of a zone's normal health, ammo and tool pack budget (averaged over the difficulty tiers). |
| `Health Kit Weight` | 1 | Multiplies the odds a kit is a health kit (the zone's budget already sets the split; 1 keeps it). 0 leaves health kits out. |
| `Tool Kit Weight` | 1 | The same for tool kits. |
| `Ammo Kit Weight` | 1 | The same for ammo kits. |
| `Kit Charges Min` | 1 | Fewest charges a kit holds. |
| `Kit Charges Max` | 2 | Most charges a kit holds. |
| `Loot Mix Enabled` | true | Hand Grenades and Stim Packs join the kits, and *OutInTheOpenShare* of the loot lies out in the open. |
| `HandGrenadeWeight` | 4 | With Loot Mix on: odds a loot consumable is a Hand Grenade, against the kits' weights (1 each). |
| `StimPackWeight` | 1 | With Loot Mix on: relative odds it's a Stim Pack (only while the Stim Pack is switched on). |
| `OutInTheOpenShare` | 0.6 | With Loot Mix on: share (0-1) of the loot left lying out in the open instead of in boxes and lockers. |

With every kit weight at 0, no kits spawn - only Loot Mix's Hand Grenades and Stim Packs, if it's on.

GTFO.EZ.AbsurdWeapons' Loot Mix had its own copies of the kit settings (`KitLootPercent`,
`HealthKitWeight`, `ToolKitWeight`, `AmmoKitWeight`, `KitChargesMin`, `KitChargesMax`); they're
merged into GTFO.EZ's names above (`Kit Loot (%)`, `Health Kit Weight`, ...), which mean the same
and have the same defaults. Its `Loot Mix Enabled`, `HandGrenadeWeight`, `StimPackWeight` and
`OutInTheOpenShare` keep their names.

## Hand Grenade

The glowstick's own item, cloned - so the throw, bounce and sync are all vanilla - wearing a lock
melter's look. Hold to wind up and release to throw (twice as hard as a glowstick); it bounces
around, flashing red faster and faster, and blows after **3 seconds**: 60 damage at the center,
falling to a quarter at 6 m, x6 stagger, and it breaks padlocks in reach. It never hurts players.

Nobody starts with any: **each one you find** as loot (see [Loot mix](#loot-mix)) **is 1 more**, up
to **6** carried. Thrown ones are gone. The terminal lists them as Hand Grenades (key
HAND_GRENADE), not glow sticks.

### `[Hand Grenade]`

| Setting | Default | What it does |
|---|---|---|
| `Hand Grenade Enabled` | true | Adds the Hand Grenade at all (takes effect on restart). Off: not added, and not in the loot. |
| `FuseSeconds` | 3 | Seconds from the throw until it explodes. |
| `ThrowForceMultiplier` | 2 | Multiplies the glowstick's own throw strength. |
| `BlastRadius` | 6 | Metres the blast reaches. |
| `BlastDamage` | 60 | Damage at the blast's center. |
| `BlastEdgeFraction` | 0.25 | Share of damage left at the blast's edge. |
| `BlastStagger` | 6 | Blast stagger multiplier. |
| `PickupCount` | 1 | How many one pickup holds (takes effect on restart). |
| `MaxCarried` | 6 | The most you can carry. |

### `[Sounds]`

| Setting | Default | What it does |
|---|---|---|
| `HandGrenadeThrow` | FRAGGRENADETHROW | Throwing a Hand Grenade (instead of the glowstick's toss). Empty = silent. |
| `HandGrenadeBounce` | FRAGGRENADEBOUNCE | A Hand Grenade bouncing (instead of the glowstick's clatter). Empty = silent. |

## Stim Pack

A consumable of its own - the I2-LP syringe's item cloned, so the I2-LP itself is left as it is -
that's an **overheal like Doom's**. It gives **33%** of your max health and can take you **past
max, up to 200%**; anything above max then **drains back down to 100%** at 1% of max a second.
Damage eats into the overheal first. For **15 seconds** after a Stim Pack, everything you hit (guns
and melee) takes **33% more damage**. The overheal only adds and takes back its own share of max
health, so a Bold artifact's bonus survives it. Found as loot (see [Loot mix](#loot-mix)).

### `[Stim Pack]`

| Setting | Default | What it does |
|---|---|---|
| `Stim Pack Enabled` | true | Adds the Stim Pack at all (takes effect on restart). Off: not added, and not in the loot. |
| `HealPercent` | 33 | Health a Stim Pack gives, in percent of max health. |
| `MaxHealthPercent` | 200 | How high it can take your health, in percent of max health. |
| `DrainPercentPerSecond` | 1 | How fast health above max drains back to max, in percent of max a second. |
| `DamageBoostPercent` | 33 | Extra damage dealt (guns and melee) while the boost runs, in percent. |
| `DamageBoostSeconds` | 15 | How long the damage boost lasts after a Stim Pack. |

## Infinite ammo and health

**Insert** toggles infinite ammo (every gun and tool stays full, no reloads) and **Delete** infinite
health (no damage to you; bots and teammates are unaffected), each announced in the event log.
Both start off. They're the same two keys every GTFO.* mod has, from one shared copy of the code
written in GTFO.EZ; with several of the mods installed, only one answers, so a press never toggles
twice.

## Compatibility

- **GTFO.EZ** used to have artifact buffs and *Consumables Are Kits*; both moved here. Running an
  older GTFO.EZ build alongside this mod buffs each artifact twice: switch one of them off in its
  config.
- **GTFO.EZ.AbsurdWeapons** used to have the Hand Grenade, Stim Pack and Loot Mix; all three moved
  here. Running an older GTFO.EZ.AbsurdWeapons build alongside this mod adds both mods' copies of the
  items (the second one to load finds the item id taken and skips it) and fills the loot twice:
  update it, or switch its Hand Grenade, Stim Pack and Loot Mix off. Otherwise compatible - it
  leaves artifacts in place, so artifact buffs work with it installed.
- **Your old settings carry over.** On first run, each moved setting that isn't in this mod's
  `.cfg` yet is copied from `MindAttic.GTFO.EZ.cfg` (artifact buffs, *Consumables Are Kits* and the
  kit settings) or `MindAttic.GTFO.EZ.AbsurdWeapons.cfg` (Hand Grenade, Stim Pack, their sounds, Loot
  Mix - and the kit settings instead, if its Loot Mix was on, since those were the ones in use).
  After that this mod's `.cfg` is the one that counts; a setting already in it is never overwritten.
- **Turning a mod off:** every GTFO.* mod's `.cfg` starts with `[0. Mod] Enabled`; a GTFO.* mod
  switched off there counts as not installed for the others.
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
src/GTFO.EZ.AbsurdItems/
  Plugin.cs              BepInEx entry point: binds each feature's config, applies Harmony patches
  ArtifactBuffs.cs       Per-player artifact counts and the max health, damage and detection buffs
  ConsumablesAreKits.cs  Runs the shared kit loot at GameDataInit, after GTFO.EZ's tweaks, with Loot
                         Mix's Hand Grenades and Stim Packs as extras
  HandGrenade.cs         Hand Grenade consumable: glowstick clone, lock-melter look, throw force, fuse, cap
  StimPack.cs            Stim Pack consumable: I2-LP clone, overheal, damage boost
  Blast.cs               The Hand Grenade's explosion, area damage, padlock damage, model and flash
  Sounds.cs              The Hand Grenade's [Sounds] events
  ConfigImport.cs        Carries moved settings over from GTFO.EZ's and GTFO.EZ.AbsurdWeapons' cfgs, once
  DataBlocks.cs          Datablock cloning
  Pure/Formulas.cs       The items' numbers (no game types; unit tested)
  LocalPlayerDamage.cs   Tells your own damage component from the host's bots'
  NonNegative.cs         Holds percentage and weight settings at 0 or more
package/                 Thunderstore manifest, icon (256x256, from logo.png), README and changelog
logo.png                 Full-size logo
```
