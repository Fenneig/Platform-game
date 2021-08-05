using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GOBased;
using PixelCrew.Creatures.Patroling;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    [RequireComponent(typeof(SpawnListComponent))]
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Patrol))]
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] protected float AlarmDelay = 0.5f;
        [SerializeField] protected float AttackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 1f;

        private Coroutine _current;
        private GameObject _target;

        protected Creature Mob;
        protected Animator MobAnimator;
        protected SpawnListComponent Particles;
        protected bool IsDead;
        protected Patrol Patrol;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        protected virtual void Awake()
        {
            Particles = GetComponent<SpawnListComponent>();
            Mob = GetComponent<Creature>();
            MobAnimator = GetComponent<Animator>();
            Patrol = GetComponent<Patrol>();
            IsDead = false;
        }

        private void Start()
        {
            StartState(Patrol?.DoPatrol());
        }

        public virtual void OnHeroInVision(GameObject go)
        {
            if (IsDead) return;
            _target = go;
            StartState(AgroToHero());
        }

        protected virtual IEnumerator AgroToHero()
        {
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(AlarmDelay);
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
                    SetDirectionToTarget();
                }
                yield return null;
            }
            if (!IsDead)
            {
                Particles.Spawn("MissHero");
                yield return new WaitForSeconds(_missHeroCooldown);
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

        public void OnDie()
        {
            IsDead = true;
            MobAnimator.SetBool(IsDeadKey, true);

            StopMoving();

            StopAllCoroutines();
        }

        protected void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;

            Mob.Direction = direction.normalized;
        }

        protected void StartState(IEnumerator coroutine)
        {
            StopMoving();
            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        private void StopMoving()
        {
            Mob.Direction = Vector2.zero;
        }

    }
}
