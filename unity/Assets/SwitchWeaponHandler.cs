using UnityEngine;
using System.Collections;

public class SwitchWeaponHandler : StateMachineBehaviour {

    public WeaponLocation weaponLocation;

    public enum SwitchType
    {
        Equip,
        Unequip
    }

    public SwitchType switchType;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (switchType)
        {
            case SwitchType.Equip:
                ObjectManager.Instance.WeaponController.WeaponEquipped(weaponLocation);
                break;
            case SwitchType.Unequip:
                ObjectManager.Instance.WeaponController.WeaponUnequipped(weaponLocation);
                break;
            default:
                break;
        }
        
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
