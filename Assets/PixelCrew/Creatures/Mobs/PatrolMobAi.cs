using PixelCrew.Components.ColliderBased;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class PatrolMobAi : MobAI
    {
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private float _horizontalThreshold = 0.2f;
        
        public override void OnHeroInVision(GameObject go)
        {
            if (IsDead) return;
            Target = go;
            StartState(AgroToHero());
        }

        protected override IEnumerator AgroToHero()
        {
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    var horizontalDelta = Mathf.Abs(Target.transform.position.x - transform.position.x);
                    if (horizontalDelta <= _horizontalThreshold) Mob.Direction = Vector2.zero;
                    else SetDirectionToTarget();
                }

                yield return null;
            }

            if (!IsDead)
            {
                Particles.Spawn("MissHero");
                yield return new WaitForSeconds(_missHeroCooldown);
                StartState(Patrol.DoPatrol());
            }
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                StopMoving();
                Mob.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }
    }
}