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
  (%)*, *Kit Charges Min/Max* and the per-kind weights are configurable. With GTFO.AbsurdWeapons'
  Loot Mix on, that mix decides the consumable loot instead.
- **Insert** (infinite ammo) and **Delete** (infinite health) toggles, announced as warden
  messages - the shared keys every GTFO.* mod has, so they never clash with several installed.
