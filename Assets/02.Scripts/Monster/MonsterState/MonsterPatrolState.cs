using System.Linq;
using UnityEngine;

public class MonsterPatrolState : IMonsterState
{
    private Monster _monster;
    
    private Vector3 _patrolPoint;

    public MonsterPatrolState(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _patrolPoint = _monster.GetPatrolPosition();
        
        _monster.NavMeshAgent.SetDestination(_patrolPoint);
        
        _monster.Animator.SetBool("IsWalk", true);
        _monster.Animator.SetBool("IsTrace", false);
        
        _monster.NavMeshAgent.speed = _monster.MoveSpeed;
    }

    public void Acting()
    {
        // 가까이 오면 공격
        var colliders = Physics.OverlapSphere(_monster.transform.position, _monster.TraceRange, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            Collider nearest = colliders
                .OrderBy(c => Vector3.Distance(_monster.transform.position, c.transform.position))
                .FirstOrDefault();
            _monster.Target = nearest?.gameObject;
            _monster.ChangeState(EMonsterState.Trace);
            return;
        }
        
        // 도착 시 Idle
        if (_monster.NavMeshAgent.remainingDistance <= _monster.NavMeshAgent.stoppingDistance)
        {
            _monster.ChangeState(EMonsterState.Idle);
            return;
        }
    }

    public void Exit()
    {
        
    }
}