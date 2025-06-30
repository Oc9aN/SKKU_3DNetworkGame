using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility, IDisableOnDeath
{
    public Transform CameraRoot;
    
    // 마우스 누적
    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (!_photonView.IsMine)
        {
            return;
        }
        
        CinemachineCamera camera = GameObject.FindWithTag("FollowCamera").GetComponent<CinemachineCamera>();
        camera.Follow = CameraRoot;
    }

    void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        
        _mx += mouseX * _player.PlayerStat.RotationSpeed * Time.deltaTime;
        _my += mouseY * _player.PlayerStat.RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        // y축 회전은 캐릭터만 한다.
        transform.eulerAngles = new Vector3(0f, _mx, 0f);
        
        // x축 회전은 캐릭터는 하지 않는다.
        CameraRoot.localEulerAngles = new Vector3(-_my, 0f, 0f);
    }
}
