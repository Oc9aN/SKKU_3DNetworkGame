using MoreMountains.Feedbacks;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerDamageAbility : PlayerAbility, IDamaged
{
    [SerializeField]
    private ParticleSystem _hitParticle;
    private CinemachineImpulseSource _impulseSource;
    private MMF_Player _mmfPlayer;

    protected override void Awake()
    {
        base.Awake();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _mmfPlayer = GetComponent<MMF_Player>();
    }
    
    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        if (transform.position.y <= -10)
        {
            _photonView.RPC(nameof(Damaged), RpcTarget.All, float.MaxValue);
        }
    }
    
    [PunRPC]
    public void Damaged(float damage, Vector3 hitPoint, int actorNumber)
    {
        if (_player.PlayerState.Is(EPlayerState.Dead))
        {
            return;
        }
        
        if (_player.PlayerStat.Health - damage <= 0f)
        {
            // 사망
            _player.OnDead(actorNumber);
            Debug.Log("사망");
            return;
        }

        if (_photonView.IsMine)
        {
            _player.PlayerStat.SetHealth(Mathf.Max(_player.PlayerStat.Health - damage, 0f));
        }
        Debug.Log($"남은 체력{_player.PlayerStat.Health}");
        
        // 연출
        if (_photonView.IsMine)
        {
            _impulseSource.GenerateImpulse();
        }
        Instantiate(_hitParticle, hitPoint, Quaternion.identity);
        _mmfPlayer.PlayFeedbacks();
    }
}