using PixelCrew.Components.GOBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossSwapStageBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.Stage++;
        }
    }
}