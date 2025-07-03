using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EMonsterState
{
    Idle,
    Patrol,
    Trace,
    Attack,
    Dead,
}

public class Monster : MonoBehaviourPun, IDamaged, IAttackable, IPunObservable
{
    [Header("스텟")]
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _damage;

    [Header("공격")]
    [SerializeField]
    private BoxCollider _attackCollider;

    [Header("FSM 정보")]
    [SerializeField]
    private float _idleTime;
    public float IdleTime => _idleTime;

    [SerializeField]
    private float _attackTime;
    public float AttackTime => _attackTime;

    [SerializeField]
    private float _attackRange;
    public float AttackRange => _attackRange;

    [SerializeField]
    private float _traceRange;
    public float TraceRange => _traceRange;
    
    [SerializeField]
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    
    [SerializeField]
    private float _traceSpeed;
    public float TraceSpeed => _traceSpeed;

    [SerializeField]
    private float _respawnTime;
    public float RespawnTime => _respawnTime;

    [Header("FSM 정보")]
    [SerializeField]
    private ParticleSystem _hitPrefab;

    private IReadOnlyList<Transform> _patrolPoints;

    private EMonsterState _state;

    private Dictionary<EMonsterState, IMonsterState> _states;

    public GameObject Target;

    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private Animator _animator;
    public Animator Animator => _animator;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _states = new Dictionary<EMonsterState, IMonsterState>()
        {
            { EMonsterState.Idle, new MonsterIdleState(this) },
            { EMonsterState.Patrol, new MonsterPatrolState(this) },
            { EMonsterState.Trace, new MonsterTraceState(this) },
            { EMonsterState.Attack, new MonsterAttackState(this) },
            { EMonsterState.Dead, new MonsterDeadState(this) }
        };
        
        _state = EMonsterState.Idle;
    }

    public void ChangeState(EMonsterState newState)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        Debug.Log($"{_state}에서 {newState}로 변환");
        _states[_state]?.Exit();
        _state = newState;
        _states[_state]?.Enter();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        _states[_state]?.Acting();
    }

    public Vector3 GetPatrolPosition()
    {
        if (_patrolPoints == null)
        {
            _patrolPoints = MonsterManager.Instance.PatrolPoints;
        }
        return _patrolPoints[Random.Range(0, _patrolPoints.Count)].position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, TraceRange);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, AttackRange); // 반투명 내부 채움
    }

    public void RequestAttackAnimation(int attackNumber)
    {
        photonView.RPC(nameof(AttackAnimationTrigger), RpcTarget.All, attackNumber);
    }

    [PunRPC]
    private void AttackAnimationTrigger(int attackNumber)
    {
        Animator.SetTrigger($"Attack{attackNumber}");
    }

    public void RequestDeathAnimation()
    {
        photonView.RPC(nameof(DeathAnimationTrigger), RpcTarget.All);
    }

    [PunRPC]
    private void DeathAnimationTrigger()
    {
        Animator.SetTrigger("Death");
    }

    [PunRPC]
    public void Damaged(float damage, Vector3 hitPoint, int actorNumber)
    {
        if (_state == EMonsterState.Dead)
        {
            return;
        }

        Instantiate(_hitPrefab, hitPoint, Quaternion.identity);

        if (photonView.IsMine)
        {
            _health -= damage;
            Debug.Log($"현재 채력: {_health}");
        }

        if (_health <= 0)
        {
            // 사망
            ChangeState(EMonsterState.Dead);
        }
    }

    public void OnAttackStart()
    {
        _attackCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        _attackCollider.enabled = false;
    }

    public bool IsMe(Transform target)
    {
        return target == transform;
    }

    public void Hit(GameObject target, Vector3 hitPoint)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        var targetDamaged = target.GetComponent<IDamaged>();
        if (targetDamaged == null)
        {
            return;
        }

        var targetPhotonView = target.GetComponent<PhotonView>();
        targetPhotonView.RPC(nameof(IDamaged.Damaged), RpcTarget.All, _damage, hitPoint, photonView.Owner.ActorNumber);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_health);
        }
        else if (stream.IsReading)
        {
            _health = (float)stream.ReceiveNext();
        }
    }
}