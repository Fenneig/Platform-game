using PixelCrew.Components.GOBased;
using PixelCrew.Creatures.Patroling;
using PixelCrew.Components.ColliderBased;
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
        [Space]
        [Header("Cooldowns")]
        [SerializeField] protected float AlarmDelay = 0.5f;
        [SerializeField] protected float AttackCooldown = 1f;
        [SerializeField] protected float MissHeroCooldown = 1f;
        [Space]
        [Header("Layer checks")]
        [SerializeField] protected LayerCheck Vision;

        private Coroutine _current;
        protected GameObject Target;

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

        public virtual void OnHeroInVision(GameObject go) { }

        protected virtual IEnumerator AgroToHero() { yield return null; }

        public void OnDie()
        {
            IsDead = true;
            MobAnimator.SetBool(IsDeadKey, true);

            StopMoving();

            StopAllCoroutines();
        }

        protected virtual void SetDirectionToTarget()
        {
            var direction = Target.transform.position - transform.position;
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

        protected void StopMoving()
        {
            Mob.Direction = Vector2.zero;
        }

    }
}
