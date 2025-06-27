public enum EPlayerState
{
    Idle,
    Attack,
    Run,
}

public class PlayerState
{
    private EPlayerState _currentState;
    public EPlayerState CurrentState => _currentState;

    public void ChangeState(EPlayerState newState)
    {
        _currentState = newState;
    }
    
    public bool Is(EPlayerState state) => _currentState == state;
}