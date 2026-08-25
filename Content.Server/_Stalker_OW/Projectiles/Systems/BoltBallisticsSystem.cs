using System;
using System.Numerics;
using Content.Shared._Stalker_OW.Projectiles.Components;
using Content.Shared.FixedPoint;
using Content.Shared.Projectiles;
using Robust.Shared.GameObjects;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Server._Stalker_OW.Projectiles.Systems;

public sealed class BoltGroundHitEvent : EntityEventArgs
{
}

/// <summary>
/// Manages flight duration, tracks active flight, and handles "recycling"
/// </summary>
public sealed class BoltBallisticsSystem : EntitySystem
{
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<BoltBallisticsComponent, PhysicsComponent, ProjectileComponent>();

        while (query.MoveNext(out var uid, out var ballistics, out var physics, out var projectile))
        {
            if (projectile.Weapon == null)
            {
                ResetFlight(ballistics);
                continue;
            }

            if (!ballistics.InFlight)
                BeginFlight(uid, ballistics, physics);

            ballistics.FlightTimeRemaining -= frameTime;

            if (ballistics.FlightTimeRemaining <= 0f)
                FinishFlight(uid, ballistics, physics, projectile);
        }
    }

    /// <summary>
    /// Starts a new flight cycle
    /// </summary>
    private void BeginFlight(
        EntityUid uid,
        BoltBallisticsComponent ballistics,
        PhysicsComponent physics)
    {
        ballistics.InFlight = true;

        var requestedTime = ballistics.FlightTimeForCurrentLaunch >= 0f
            ? ballistics.FlightTimeForCurrentLaunch
            : ballistics.MaxFlightTime;

        ballistics.FlightTimeRemaining = Math.Clamp(
            requestedTime,
            0f,
            ballistics.MaxFlightTime);

        // Apply speed multiplier
        if (physics.BodyType == BodyType.Dynamic)
        {
            var speedMultiplier = Math.Max(0f, ballistics.SpeedMultiplier);
            var velocity = physics.LinearVelocity * speedMultiplier;

            _physics.SetLinearVelocity(uid, velocity, body: physics);
        }
    }
    
    /// <summary>
    /// Manually overrides flight duration
    /// </summary>
    public void SetFlightTime(EntityUid uid, float requestedTime, BoltBallisticsComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.FlightTimeForCurrentLaunch = Math.Clamp(requestedTime, 0f, component.MaxFlightTime);
        if (component.InFlight)
            component.FlightTimeRemaining = component.FlightTimeForCurrentLaunch;
    }

    /// <summary>
    /// Resets projectile entity
    /// </summary>
    private void FinishFlight(EntityUid uid, BoltBallisticsComponent ballistics, PhysicsComponent physics, ProjectileComponent projectile)
    {
        if (physics.BodyType == BodyType.Dynamic)
            _physics.SetLinearVelocity(uid, Vector2.Zero, body: physics);

        ResetFlight(ballistics);

        // Restore reusable state
        projectile.Weapon = null;
        projectile.Shooter = null;
        projectile.ProjectileSpent = false;
        projectile.PenetrationAmount = FixedPoint2.Zero;

        Dirty(uid, projectile);
        
        RaiseLocalEvent(uid, new BoltGroundHitEvent());
    }

    /// <summary>
    /// Clears flight timers
    /// </summary>
    private static void ResetFlight(BoltBallisticsComponent ballistics)
    {
        ballistics.InFlight = false;
        ballistics.FlightTimeRemaining = 0f;
        ballistics.FlightTimeForCurrentLaunch = -1f;
    }
}