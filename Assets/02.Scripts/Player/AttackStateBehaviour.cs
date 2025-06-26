using UnityEngine;

public class AttackStateBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var attackAbility = animator.GetComponent<PlayerAttackAbility>();
        attackAbility.OnAttackEnd();
    }
}