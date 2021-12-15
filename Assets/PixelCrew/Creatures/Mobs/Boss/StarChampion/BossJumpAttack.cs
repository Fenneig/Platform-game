using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.StarChampion
{
    public class BossJumpAttack : BossAttack
    {
        [SerializeField] private AnimationCurve _curveY;
        [SerializeField] [Range(3, 5)] private float _attackRangeMin;
        [SerializeField] [Range(5, 8)] private float _attackRangeMax;

        public override void Attack()
        {
            CalculateTargetPosition();
            base.Attack();
            StartCoroutine(AttackMove());
        }

        private void CalculateTargetPosition()
        {
            var attackRange = Random.Range(_attackRangeMin, _attackRangeMax);
            if (transform.lossyScale.x < 0) attackRange *= -1;
            Debug.Log(attackRange);
            var xPosition = transform.position.x + attackRange;
            if (xPosition <= _leftPoint.position.x) xPosition = _leftPoint.position.x;
            if (xPosition >= _rightPoint.position.x) xPosition = _rightPoint.position.x;
            AttackTarget = new Vector3(xPosition, transform.position.y, transform.position.z);
        }

        private IEnumerator AttackMove()
        {
            while (TimeElapsed <= _moveTime)
            {
                TimeElapsed += Time.deltaTime;

                var newX = _curveX.Evaluate(TimeElapsed / _moveTime);
                var newY = _curveY.Evaluate(TimeElapsed / _moveTime);

                transform.position = new Vector3(newX, newY, transform.position.z);

                yield return null;
            }

            Animator.SetBool(IsAttacking, false);
        }
    }
}