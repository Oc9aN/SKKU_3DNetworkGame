using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility, IPunObservable
{
    private Animator _animator;
    private CharacterController _characterController;

    private float _yVelocity;

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
        if (!_photonView.IsMine)
        {
            return;
        }
        
        if (!_player.PlayerState.Is(EPlayerState.Idle))
        {
            return;
        }
        
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
            if (Input.GetKeyDown(KeyCode.Space))
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
        _characterController.Move(dir * _player.PlayerStat.MoveSpeed * Time.deltaTime);
        
        _animator.SetBool("IsGround", _characterController.isGrounded);
        
        float hAnimation = Input.GetAxis("Horizontal");
        float vAnimation = Input.GetAxis("Vertical");
        float moveAmount = new Vector2(hAnimation, vAnimation).magnitude; // 입력 기반
        _animator.SetBool("IsIdle", moveAmount < 0.1f);
        _animator.SetFloat("MoveX", hAnimation);
        _animator.SetFloat("MoveY", vAnimation);
    }

    // 데이터 동기화를 할 수 있는 송수신 기능
    // stream : 서버에서 주고 받을 데이터가 담겨있는 변수
    // info : 송수진 성공/실패 여부에 대한 로그
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 내 데이터만 전송
            // 데이터를 전송하는 상황 -> 데이터 보내기
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            // 내가 아닌 데이터만 수신
            // 데이터를 수신하는 상황 -> 받은 데이터 셋팅
            Vector3 receivedPosition = (Vector3)stream.ReceiveNext();
            Quaternion receiveQuaternion = (Quaternion)stream.ReceiveNext();

            transform.position = receivedPosition;
            transform.rotation = receiveQuaternion;
        }
    }
}