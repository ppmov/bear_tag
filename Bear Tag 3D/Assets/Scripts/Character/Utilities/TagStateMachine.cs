using UnityEngine;

public class TagStateMachine : StateMachineBehaviour
{
    [SerializeField]
    private string enterTag;
    [SerializeField]
    private string exitTag;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.tag = enterTag;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.tag = exitTag;
    }
}
