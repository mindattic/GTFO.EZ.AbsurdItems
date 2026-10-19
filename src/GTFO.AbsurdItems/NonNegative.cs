using BepInEx.Configuration;

namespace GTFO.AbsurdItems;

// Every float setting here is a percentage or weight, none of which mean anything below 0 - a
// negative one would turn into negative health, damage or kit counts. BepInEx clamps a value it
// reads from the .cfg to this, so a typo'd minus sign lands on 0 instead. (The same rule as GTFO.EZ's.)
internal sealed class NonNegative : AcceptableValueBase
{
    internal static readonly NonNegative Instance = new();

    private NonNegative() : base(typeof(float)) { }

    // !(f >= 0) also catches NaN.
    public override object Clamp(object value) => value is float f && !(f >= 0f) ? 0f : value;
    public override bool IsValid(object value) => value is float f && f >= 0f;
    public override string ToDescriptionString() => "# Acceptable value range: 0 or more";

    internal static ConfigEntry<float> Bind(ConfigFile config, string section, string key, float defaultValue, string description) =>
        config.Bind(section, key, defaultValue, new ConfigDescription(description, Instance));
}
