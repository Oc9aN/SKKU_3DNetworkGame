public class MonsterDeadState : IMonsterState
{
    private Monster _monster;

    public MonsterDeadState(Monster monster)
    {
        _monster = monster;
    }
    
    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Acting()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}