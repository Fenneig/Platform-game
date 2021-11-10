using PixelCrew.Components.GOBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossShootBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.LaunchProjectile();
        }

    }
}