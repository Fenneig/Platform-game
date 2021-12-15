using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.Patric
{
    public class BossFloodState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var flood = animator.GetComponentInChildren<FloodController>();
            flood.StartFlood();
        }
    }
}   