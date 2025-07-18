using UnityEngine;

public class PlayerWeaponAbility : PlayerAbility
{
    [SerializeField]
    private Transform _weapon;
    [SerializeField]
    private float _weaponScaleFactor;
    [SerializeField]
    private int _weaponScaleUnit;
    private void Start()
    {
        ScoreManager.Instance.OnScoreAdded += OnScoreAdded;
    }

    private void OnScoreAdded()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        
        // 무기 크기 변경
        RefreshWeaponScale();
    }

    private void RefreshWeaponScale()
    {
        float addedScale = (ScoreManager.Instance.TotalScore / _weaponScaleUnit) * _weaponScaleFactor;
        
        _weapon.localScale = Vector3.one + Vector3.one * addedScale;
    }
}