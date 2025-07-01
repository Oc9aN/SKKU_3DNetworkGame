using UnityEngine;

public interface IDamaged
{
    void Damaged(float damage, Vector3 hitPoint, int actorNumber);
}
