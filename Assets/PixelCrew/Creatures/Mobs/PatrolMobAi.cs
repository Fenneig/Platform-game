using PixelCrew.Components.ColliderBased;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class PatrolMobAi : MobAI
    {
        [SerializeField] private LayerCheck _canAttack;

        public override void OnHeroInVision(GameObject go)
        {
            if (IsDead) return;
            Target = go;
            StartState(AgroToHero());
        }

        protected override IEnumerator AgroToHero()
        {
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(AlarmDelay);

            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            while (Vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }
            if (!IsDead)
            {
                Particles.Spawn("MissHero");
                yield return new WaitForSeconds(MissHeroCooldown);
                StartState(Patrol?.DoPatrol());
            }
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                StopMoving();
                Mob.Attack();
                yield return new WaitForSeconds(AttackCooldown);
            }

            StartState(GoToHero());
        }
    }
}
