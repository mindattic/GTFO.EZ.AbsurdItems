using Player;

namespace GTFO.AbsurdItems;

// The host's bots have a Dam_PlayerDamageLocal too, so its Setup runs once per bot as well as for
// you - a bot joining mid-level would otherwise wipe the artifact buffs. Every hook on it checks
// this first so only your own component counts.
internal static class LocalPlayerDamage
{
    internal static bool IsMine(Dam_PlayerDamageLocal damage)
    {
        var agent = damage.Owner;
        return agent != null && agent.Owner != null && !agent.Owner.IsBot;
    }
}
