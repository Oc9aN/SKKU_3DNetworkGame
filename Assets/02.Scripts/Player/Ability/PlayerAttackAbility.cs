using System;
using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility, IDisableOnDeath
{
    public Collider WeaponCollider;
    
    private float _attackTimer;
    private Animator _animator;

    private CharacterController _characterController;
    
    private bool _isAttacking;
    public bool IsAttacking => _isAttacking;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        
        WeaponCollider.enabled = false;
    }
    
    // 위치/회전처럼 상시 확인이 필요한 데이터 동기화: IPunObservable(OnPhotonSerializeView)
    // 트리거, 피격처럼 간헐적으로 특정한 이벤트가 발생했을때의 변화된 동기화: RPC
    // RPC: Remote Procedure Call - 물리적으로 떨어져 있는 다른 디바이스의 함수를 호출하는 기능
    // RPC함수를 호출하면 네트워크를 통해 다른 사용자의 스크립트에서 해당 함수가 호출 된다.

    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        _attackTimer += Time.deltaTime;
        var attackStaminaCost = _player.PlayerStat.AttackStamina;
        if (_attackTimer >= (1f / _player.PlayerStat.AttackSpeed)
            && Input.GetMouseButtonDown(0)
            && _characterController.isGrounded
            && _player.TryUseStamina(attackStaminaCost))
        {
            // 공격 1~3 랜덤 공격
            int random = UnityEngine.Random.Range(1, 4);
            _attackTimer = 0f;

            _isAttacking = true;
            _photonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, random);
        }
    }

    public void OnAttackStart()
    {
        WeaponCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        WeaponCollider.enabled = false;
    }

    public void AttackAnimationEnd()
    {
        _isAttacking = false;
    }

    [PunRPC]
    private void PlayAttackAnimation(int randomNumber)
    {
        _animator.SetTrigger($"Attack{randomNumber}");
    }

    public void Hit(GameObject target, Vector3 hitPoint)
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        var targetDamaged = target.GetComponent<IDamaged>();
        if (targetDamaged == null)
        {
            return;
        }
        
        OnAttackEnd();
        
        var targetPhotonView = target.GetComponent<PhotonView>();
        targetPhotonView.RPC(nameof(PlayerDamageAbility.Damaged), RpcTarget.All, _player.PlayerStat.AttackDamage, hitPoint);
    }
}