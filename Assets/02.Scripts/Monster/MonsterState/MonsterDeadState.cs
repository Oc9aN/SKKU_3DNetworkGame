using Photon.Pun;
using UnityEngine;

public class MonsterDeadState : IMonsterState
{
    private Monster _monster;
    
    private float _respawnTimer;

    public MonsterDeadState(Monster monster)
    {
        _monster = monster;
    }
    
    public void Enter()
    {
        _monster.Animator.SetBool("IsWalk", false);
        _monster.NavMeshAgent.isStopped = true;
        _monster.NavMeshAgent.ResetPath();
        
        _monster.RequestRPC(() => _monster.Animator.SetTrigger("Death"));
    }

    public void Acting()
    {
        _respawnTimer += Time.deltaTime;
        if (_respawnTimer >= _monster.RespawnTime)
        {
            MonsterManager.Instance.CreateMonster();
            PhotonNetwork.Destroy(_monster.gameObject);
        }
    }

    public void Exit()
    {
        
    }
}