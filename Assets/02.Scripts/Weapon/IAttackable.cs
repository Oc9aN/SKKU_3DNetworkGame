using UnityEngine;

public interface IAttackable
{
    public bool IsMe(Transform target);
    public void Hit(GameObject target, Vector3 hitPoint);
}