using NUnit.Framework;

namespace GTFO.EZ.AbsurdItems.Tests;

// Moved here with the Hand Grenade and Stim Pack from GTFO.EZ.AbsurdWeapons, whose own copies of these
// formulas (still used by its grenade guns) keep their tests there.
[TestFixture]
public class FormulasTests
{
    [Test]
    public void DisplayName_MarksModdedItems()
    {
        Assert.That(Formulas.DisplayName("Hand Grenade"), Is.EqualTo("Hand Grenade (!)"));
        Assert.That(Formulas.DisplayName("Stim Pack"), Is.EqualTo("Stim Pack (!)"));
    }

    [Test]
    public void AreaFalloff_FullAtCenterEdgeShareAtRadius()
    {
        Assert.That(Formulas.AreaFalloff(0f, 6f, 0.25f), Is.EqualTo(1f).Within(1e-6));
        Assert.That(Formulas.AreaFalloff(6f, 6f, 0.25f), Is.EqualTo(0.25f).Within(1e-6));
        Assert.That(Formulas.AreaFalloff(3f, 6f, 0.25f), Is.EqualTo(0.625f).Within(1e-6));
    }

    [Test]
    public void AreaFalloff_ClampsOutsideTheRadius()
    {
        Assert.That(Formulas.AreaFalloff(9f, 5f, 0.4f), Is.EqualTo(0.4f).Within(1e-6));
        Assert.That(Formulas.AreaFalloff(1f, 0f, 0.4f), Is.EqualTo(1f));
    }

    [Test]
    public void GrenadeFlashInterval_SpeedsUpTowardTheEnd()
    {
        Assert.That(Formulas.GrenadeFlashInterval(0f, 3f), Is.EqualTo(0.5f).Within(1e-6));
        Assert.That(Formulas.GrenadeFlashInterval(3f, 3f), Is.EqualTo(0.06f).Within(1e-6));
        var previous = float.MaxValue;
        for (var age = 0f; age <= 3f; age += 0.25f)
        {
            var interval = Formulas.GrenadeFlashInterval(age, 3f);
            Assert.That(interval, Is.LessThanOrEqualTo(previous));
            previous = interval;
        }
        Assert.That(Formulas.GrenadeFlashInterval(10f, 3f), Is.EqualTo(0.06f).Within(1e-6));
    }

    [Test]
    public void GrenadeFlashInterval_NoFuseFlashesFastest()
    {
        Assert.That(Formulas.GrenadeFlashInterval(0f, 0f), Is.EqualTo(0.06f).Within(1e-6));
    }
}
