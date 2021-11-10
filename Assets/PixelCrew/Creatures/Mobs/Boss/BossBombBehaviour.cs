﻿using PixelCrew.Components.GOBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossBombBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponentInChildren<BombSpawnerComponent>().StartSpawn();
        }
    }
}