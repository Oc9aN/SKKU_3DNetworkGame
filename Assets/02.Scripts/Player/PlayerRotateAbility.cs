using System;
using UnityEngine;

public class PlayerRotateAbility : MonoBehaviour
{
    public Transform CameraRoot;
    public float RotationSpeed = 10f;
    
    // 마우스 누적
    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        // y축 회전은 캐릭터만 한다.
        transform.eulerAngles = new Vector3(0f, _mx, 0f);
        
        // x축 회전은 캐릭터는 하지 않는다.
        CameraRoot.localEulerAngles = new Vector3(-_my, 0f, 0f);
    }
}
