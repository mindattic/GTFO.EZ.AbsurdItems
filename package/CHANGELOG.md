# Changelog

## v1

- Initial release. Every feature has its own on/off switch in the config.
- Master switch: `[0. Mod] Enabled` (default `true`) is the config file's first entry; set it to
  `false` and the whole mod is off - nothing is patched, no item is added, no hotkey or loot change.
  The other GTFO.* mods then treat it as not installed.
- **Artifact buffs** (moved here from GTFO.EZ, same settings): picking up an artifact buffs whoever
  picked it up for the rest of the level - each **Bold** +5% max health (healed too), each
  **Aggressive** +5% damage to enemies, each **Muted** 5% harder for enemies to detect you (capped
  at 75%). A checkpoint restart that puts an artifact back takes its buff off again.
- **Consumables Are Kits** (moved here from GTFO.EZ, same settings): consumables found in lockers,
  boxes or lying around are a few small health, tool and ammo kits of 1-2 charges each, adding about
  20% to the zone's normal resource packs, split the way the zone's resource budget is. *Kit Loot
  (%)*, *Kit Charges Min/Max* and the per-kind weights are configurable.
- **Hand Grenade** (moved here from GTFO.AbsurdWeapons, same `[Hand Grenade]` and `[Sounds]`
  settings): a consumable lock melter thrown like a glowstick that blows after 3 s (60 damage
  falling to a quarter at 6 m, x6 stagger, breaks padlocks, never hurts players). Found only as
  loot, 1 a pickup, up to 6 carried. The terminal lists it as a Hand Grenade (key `HAND_GRENADE`),
  not as the glow stick it is cloned from.
- **Stim Pack** (moved here from GTFO.AbsurdWeapons, same `[Stim Pack]` settings): the I2-LP
  cloned as a consumable of its own - heals 33% of max health and can overheal to 200%, draining
  back at 1% a second, and boosts your damage 33% for 15 s.
- **Loot Mix** (moved here from GTFO.AbsurdWeapons): with *Consumables Are Kits* on, Hand Grenades
  (`HandGrenadeWeight` 4) and Stim Packs (`StimPackWeight` 1) join the kits in one kit-loot run, and
  60% of the loot lies out in the open (`OutInTheOpenShare`). `Loot Mix Enabled` keeps its name.
  AbsurdWeapons' own copies of the kit settings (`KitLootPercent`, `HealthKitWeight`,
  `ToolKitWeight`, `AmmoKitWeight`, `KitChargesMin/Max`) are merged into the GTFO.EZ names here
  (`Kit Loot (%)`, `Health Kit Weight`, ..., `Kit Charges Min/Max`), same meanings and defaults.
- **Insert** (infinite ammo) and **Delete** (infinite health) toggles, announced as warden
  messages - the shared keys every GTFO.* mod has, so they never clash with several installed.
- Every Hand Grenade and Stim Pack number is held at 0 or more, like the other settings (a typo'd
  minus sign lands on 0: a negative `DrainPercentPerSecond` made the overheal grow without end), and
  `PickupCount` / `MaxCarried` at 1 or more (below that, pickups were empty).
