using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private IAttackable _attackable;

    private void Awake()
    {
        _attackable = GetComponentInParent<IAttackable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신은 무시
        if (_attackable.IsMe(other.transform))
        {
            return;
        }

        if (other.GetComponent<IDamaged>() == null)
        {
            return;
        }

        _attackable.Hit(other.gameObject, other.ClosestPoint(transform.position));
    }
}
