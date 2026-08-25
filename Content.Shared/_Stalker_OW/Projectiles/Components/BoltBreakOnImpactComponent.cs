using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Stalker_OW.Projectiles.Components;

[RegisterComponent]
public sealed partial class BoltBreakOnImpactComponent : Component
{
    [DataField("breakChance")]
    public float BreakChance = 0.15f;

    [DataField("brokenPrototype")]
    public EntProtoId? BrokenPrototype;
}