using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared._Stalker_OW.Projectiles.Components;

/// <summary>
/// Controls maximum flight times
/// </summary>
[RegisterComponent]
public sealed partial class BoltBallisticsComponent : Component
{
    /// <summary>
    /// This is the absolute maximum amount of time an entity will remain in flight
    /// </summary>
    [DataField("maxFlightTime"), ViewVariables(VVAccess.ReadWrite)]
    public float MaxFlightTime = 0.9f;

    /// <summary>
    /// Current flight duration
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float FlightTimeForCurrentLaunch = -1f;

    /// <summary>
    /// Time left in flight
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float FlightTimeRemaining;

    /// <summary>
    /// To see if entity is in flight state
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public bool InFlight;
    
    /// <summary>
    /// Multiplier to flight speed
    /// </summary>
    [DataField("speedMultiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float SpeedMultiplier = 1.0f;
}