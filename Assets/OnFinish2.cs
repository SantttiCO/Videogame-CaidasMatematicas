using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinish2 : StateMachineBehaviour
{
    [SerializeField] private string animation;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Alberto_dos>().changeAnimation(animation, .2f, stateInfo.length);
    }
}
