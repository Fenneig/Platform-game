using PixelCrew.Components.GOBased;
using PixelCrew.Components.ColliderBased;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    [RequireComponent(typeof(SpawnListComponent))]
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Patrol.Patrol))]
    public class MobAI : MonoBehaviour
    {
        [Space]
        [Header("Cooldowns")]
        [SerializeField] protected float _alarmDelay = 0.5f;
        [SerializeField] protected float _attackCooldown = 1f;
        [SerializeField] protected float _missHeroCooldown = 1f;
        [Space]
        [Header("Layer checks")]
        [SerializeField] protected LayerCheck _vision;

        private Coroutine _current;
        protected GameObject Target;

        protected Creature Mob;
        protected Animator MobAnimator;
        protected SpawnListComponent Particles;
        protected bool IsDead;
        protected Patrol.Patrol Patrol;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        protected virtual void Awake()
        {
            Particles = GetComponent<SpawnListComponent>();
            Mob = GetComponent<Creature>();
            MobAnimator = GetComponent<Animator>();
            Patrol = GetComponent<Patrol.Patrol>();
        }

        private void Start()
        {
            StartState(Patrol.DoPatrol());
        }

        public virtual void OnHeroInVision(GameObject go) { }

        protected virtual IEnumerator AgroToHero() { yield return null; }

        public void OnDie()
        {
            IsDead = true;
            MobAnimator.SetBool(IsDeadKey, true);

            StopAllCoroutines();
            StopMoving();
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
            if (_current != null) StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        protected void StopMoving()
        {
            Mob.Direction = Vector2.zero;
        }

    }
}


