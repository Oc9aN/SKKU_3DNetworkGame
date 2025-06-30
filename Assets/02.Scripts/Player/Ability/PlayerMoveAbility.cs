using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility, IDisableOnDeath
{
    private Animator _animator;
    private CharacterController _characterController;

    private float _yVelocity;

    private bool _isRunning;
    public bool IsRunning => _isRunning;

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_photonView.IsMine
            || _player.GetAbility<PlayerAttackAbility>().IsAttacking
            || _player.PlayerState.Is(EPlayerState.Dead))
        {
            return;
        }

        Run();
        Move();
    }

    private void Run()
    {
        float staminaCost = _player.PlayerStat.RunStaminaRate * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift)
            && _player.TryUseStamina(staminaCost))
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 카메라 기준 방향
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; // = dir.Normalize();

        // 카메라가 바라보는 방향 기준으로 수정하기
        dir = Camera.main.transform.TransformDirection(dir);

        // 점프 및 중력
        if (_characterController.isGrounded)
        {
            _animator.SetBool("Fall", false);
            var jumpStaminaCost = _player.PlayerStat.JumpStamina;
            if (Input.GetKeyDown(KeyCode.Space) && _player.TryUseStamina(jumpStaminaCost))
            {
                _animator.SetBool("Jump", true);
                _yVelocity = _player.PlayerStat.JumpPower;
            }
            else
            {
                _animator.SetBool("Jump", false);
                _yVelocity = -1f;
            }
        }
        else
        {
            _yVelocity += Physics.gravity.y * Time.deltaTime;
            _animator.SetBool("Fall", _yVelocity < 0);
        }

        dir.y = _yVelocity;

        // 이동
        float moveSpeed = _isRunning ? _player.PlayerStat.RunSpeed : _player.PlayerStat.MoveSpeed;
        _characterController.Move(dir * (moveSpeed * Time.deltaTime));

        _animator.SetBool("IsGround", _characterController.isGrounded);

        float hAnimation = Input.GetAxis("Horizontal");
        float vAnimation = Input.GetAxis("Vertical");
        float moveAmount = new Vector2(hAnimation, vAnimation).magnitude; // 입력 기반
        _animator.SetBool("IsIdle", moveAmount < 0.1f);
        _animator.SetFloat("MoveX", hAnimation);
        _animator.SetFloat("MoveY", vAnimation);
    }
}