public class PlayerPresenter
{
    private Player _player;
    private PlayerView _playerView;

    public PlayerPresenter(Player player, PlayerView playerView)
    {
        _player = player;
        _playerView = playerView;
        
        _player.PlayerStat.OnDataChanged += _playerView.Refresh;
    }
}