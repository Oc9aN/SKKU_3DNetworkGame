using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpPower = 7f;
    public float Gravity = 9.8f;
    private Animator _animator;
    private CharacterController _characterController;

    private float _yVelocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 카메라 기준 방향
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * v + camRight * h).normalized;

        // 점프 및 중력
        if (_characterController.isGrounded)
        {
            _animator.SetBool("Fall", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetBool("Jump", true);
                _yVelocity = JumpPower;
            }
            else
            {
                _animator.SetBool("Jump", false);
                _yVelocity = -1f;
            }
        }
        else
        {
            _yVelocity -= Gravity * Time.deltaTime;
            _animator.SetBool("Fall", _yVelocity < 0);
        }

        moveDir.y = _yVelocity;

        // 이동
        _characterController.Move(moveDir * MoveSpeed * Time.deltaTime);
        
        _animator.SetBool("IsGround", _characterController.isGrounded);
        
        float hAnimation = Input.GetAxis("Horizontal");
        float vAnimation = Input.GetAxis("Vertical");
        float moveAmount = new Vector2(hAnimation, vAnimation).magnitude; // 입력 기반
        _animator.SetFloat("Movement", moveAmount);
    }
}