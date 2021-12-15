using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.StarChampion
{
    public class BossRollBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.GetComponent<BossRollAttack>().Attack();
        }
    }
}