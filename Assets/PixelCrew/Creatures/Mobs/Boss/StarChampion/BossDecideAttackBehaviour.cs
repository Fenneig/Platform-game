using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.StarChampion
{
    public class BossDecideAttackBehaviour : StateMachineBehaviour
    {
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.GetComponentInChildren<BossDecideAttack>().FaceHero();
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.GetComponentInChildren<BossDecideAttack>().Decide();
        }
        
    }
}