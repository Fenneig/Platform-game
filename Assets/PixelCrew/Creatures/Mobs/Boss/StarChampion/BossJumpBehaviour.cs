using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.StarChampion
{
    public class BossJumpBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponentInChildren<BossJumpAttack>().Attack();
        }
    }
}