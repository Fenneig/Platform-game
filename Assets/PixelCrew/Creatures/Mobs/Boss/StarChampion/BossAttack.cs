using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.StarChampion
{
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] protected Transform _leftPoint;
        [SerializeField] protected Transform _rightPoint;
        [SerializeField] protected float _moveTime;
        [SerializeField] protected AnimationCurve _curveX;
        
        protected Vector3 AttackTarget;
        protected Animator Animator;
        protected static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        protected float TimeElapsed;

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
        }

        public virtual void Attack()
        {
            CalculateCurve();
            TimeElapsed = 0;
            Animator.SetBool(IsAttacking, true);  
        }

        private void CalculateCurve()
        {
            _curveX.MoveKey(0, new Keyframe(0, transform.position.x));
            _curveX.MoveKey(1, new Keyframe(1, AttackTarget.x));
        }
    }
}