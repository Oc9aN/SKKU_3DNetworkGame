using UnityEngine;

public class MonsterAttackState : IMonsterState
{
    private Monster _monster;

    private float _attackTimer;

    public MonsterAttackState(Monster monster)
    {
        _monster = monster;
    }
    
    public void Enter()
    {
        _attackTimer = 0f;
        _monster.Animator.SetBool("IsWalk", false);
        _monster.NavMeshAgent.isStopped = true;
        _monster.NavMeshAgent.ResetPath();
    }

    public void Acting()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer > _monster.AttackTime &&
            Vector3.Distance(_monster.transform.position, _monster.Target.transform.position) < _monster.AttackRange)
        {
            // 공격
            Debug.Log("곰 공격");
            _monster.RequestAttackAnimation(Random.Range(1, 5));
            _attackTimer = 0f;
        }

        if (Vector3.Distance(_monster.transform.position, _monster.Target.transform.position) > _monster.AttackRange)
        {
            _monster.ChangeState(EMonsterState.Trace);
        }
    }

    public void Exit()
    {
        _attackTimer = 0f;
    }
}