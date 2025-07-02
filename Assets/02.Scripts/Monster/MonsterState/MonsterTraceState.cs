using UnityEngine;

public class MonsterTraceState : IMonsterState
{
    private Monster _monster;
    public MonsterTraceState(Monster monster)
    {
        _monster = monster;
    }
    
    public void Enter()
    {
        if (_monster.Target == null)
        {
            _monster.ChangeState(EMonsterState.Patrol);
        }
        _monster.Animator.SetBool("IsWalk", true);
    }

    public void Acting()
    {
        if (Vector3.Distance(_monster.transform.position, _monster.Target.transform.position) >= _monster.TraceRange)
        {
            _monster.ChangeState(EMonsterState.Patrol);
            return;
        }
        
        // 전이 Attack
        if (Vector3.Distance(_monster.transform.position, _monster.Target.transform.position) < _monster.AttackRange)
        {
            _monster.ChangeState(EMonsterState.Attack);
            return;
        }
        
        _monster.NavMeshAgent.SetDestination(_monster.Target.transform.position);
    }

    public void Exit()
    {
        
    }
}