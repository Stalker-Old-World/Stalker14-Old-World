using Content.Server.Players.PlayTimeTracking;
using Content.Server._Stalker_EN.PdaMessenger;
using Content.Shared.GameTicking;

namespace Content.Server._Stalker_OW.Player;

public sealed class RookieNotificationSystem : EntitySystem
{
    [Dependency] private readonly PlayTimeTrackingManager _playTime = default!;
    [Dependency] private readonly STMessengerSystem _messenger = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);
    }

    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent ev)
    {
        if (!_playTime.TryGetTrackerTimes(ev.Player, out _))
            return;

        if (_playTime.GetOverallPlaytime(ev.Player) > TimeSpan.Zero)
            return;

        var rookieName = Name(ev.Mob);

        _messenger.SendSystemChannelMessage(
            "STNeutrals",
            "STALKER NETWORK",
            $"New stalker registered: {rookieName}. There is no indication of previous activity in the Zone. Experienced stalkers are encouraged to provide guidance."
        );
    }
}