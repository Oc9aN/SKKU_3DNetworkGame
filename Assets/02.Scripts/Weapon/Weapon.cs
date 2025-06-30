using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttackAbility _attackAbility;

    private void Awake()
    {
        _attackAbility = GetComponentInParent<PlayerAttackAbility>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신은 무시
        if (other.transform == _attackAbility.transform)
        {
            return;
        }

        if (other.GetComponent<IDamaged>() == null)
        {
            return;
        }

        _attackAbility.Hit(other.gameObject, other.ClosestPoint(transform.position));
    }
}
