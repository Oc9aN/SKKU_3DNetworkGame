using System.Linq;
using UnityEngine;

public class MonsterIdleState : IMonsterState
{
    private Monster _monster;

    private float _idleTimer = 0f;
    
    public MonsterIdleState(Monster monster)
    {
        _monster = monster;
    }
    
    public void Enter()
    {
        _idleTimer = 0f;
        _monster.Animator.SetBool("IsWalk", false);
    }

    public void Acting()
    {
        // Idle이 지나면 패트롤
        _idleTimer += Time.deltaTime;
        if (_idleTimer >= _monster.IdleTime)
        {
            _monster.ChangeState(EMonsterState.Patrol);
            return;
        }
        
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
    }

    public void Exit()
    {
        _idleTimer = 0f;
    }
}