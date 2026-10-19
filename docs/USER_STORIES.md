---
codex: 1
project: GTFO.EZ.AbsurdItems
code: GAI
layer: stories
status: living
updated: 2026-10-18
---
<!-- GENERATED from MindAttic.Workflow/data/workflow.db by `workflow db render`. Do not edit: this file is rewritten from the database; change stories and tickets with the workflow CLI. -->

# GTFO.EZ.AbsurdItems — User Stories
> ✅ done (shipped & tested) · 🟡 partial · ⬜ planned. Every ✅ cites the test.

## Priority backlog
_Nothing recorded yet._

## Tickets
> Permanent log of every change and fix — this repo's ticket system. Rows are append-only: never
> delete or reword one; only move its Status (open → closed / wont-do) and fill in Ref. Status is
> a word, not a glyph, so tickets don't count toward the story index. Keep this section last.

| ID | Date | Type | Status | Summary | Ref |
|---|---|---|---|---|---|
| GAI-T1 | 2026-10-18 | docs | closed | Start the permanent ticket log in the workflow database | — |
| GAI-T2 | 2026-10-18 | fix | closed | Push initial project scaffolding so origin/main exists and pull stops failing | 3309db6 |
| GAI-T3 | 2026-10-18 | feature | closed | v1: GTFO.AbsurdItems mod - artifact buffs (from GTFO.EZ), Hand Grenade and Stim Pack (from GTFO.AbsurdWeapons), kits as loot in lockers and out in the open (Loot Mix + shared KitLoot), plus the shared Insert/Delete toggles | 73ec87c |
| GAI-T4 | 2026-10-18 | feature | closed | Shared code republished from GTFO.EZ: SharedOwner treats a switched-off GTFO.* mod as not installed, IsActive/TryGetSetting helpers | 2ff597c |
| GAI-T5 | 2026-10-18 | feature | closed | Master switch: [0. Mod] Enabled is the cfg's first entry; false and GTFO.AbsurdItems loads nothing | baf57f5 |
| GAI-T6 | 2026-10-18 | fix | closed | Hand Grenade keeps the glowstick's terminal names, so LIST/QUERY/PING show every Hand Grenade as a glow stick; give it its own (Hand Grenade, key HAND_GRENADE) | 645d804 |
| GAI-T7 | 2026-10-18 | fix | closed | Hand Grenade and Stim Pack numbers aren't held at 0 or more (a negative DrainPercentPerSecond grows the overheal without end, negative damage/heal); PickupCount/MaxCarried below 1 make empty pickups - validate them like the other settings | ba9fb30 |
| GAI-T8 | 2026-10-18 | fix | closed | Moved features lost players' settings: artifact buffs and Consumables Are Kits (GTFO.EZ) and Hand Grenade, Stim Pack, Loot Mix (GTFO.AbsurdWeapons) bind in a new cfg, so values tuned in the old mods' cfgs silently reset to defaults; import each key once from the old cfg when it isn't in AbsurdItems' yet | 88a5e61 |
| GAI-T9 | 2026-10-18 | fix | closed | Shared code from GTFO.EZ (G.EZ-T72/T75): warden message never cuts the game's own short; infinite-health check can't throw | — |
| GAI-T10 | 2026-10-18 | chore | closed | Renamed to GTFO.EZ.AbsurdItems (project, namespace, plugin GUID, network events, package name, docs) as a GTFO.EZ companion mod built on its core | 6ac3e98 |
