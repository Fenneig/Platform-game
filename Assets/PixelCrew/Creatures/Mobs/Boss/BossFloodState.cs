using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossFloodState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var flood = animator.GetComponent<FloodController>();
            flood.StartFlood();
        }
    }
}   