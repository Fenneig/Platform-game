using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.StarChampion
{
    public class BossRollAttack : BossAttack
    {
        public override void Attack()
        {
            FindNextPoint();
            base.Attack();
            StartCoroutine(AttackMove());
        }

        private void FindNextPoint()
        {
            AttackTarget = transform.localScale.x < 0
                ? _leftPoint.position
                : _rightPoint.position;
        }

        private IEnumerator AttackMove()
        {
            while (TimeElapsed <= _moveTime)
            {
                TimeElapsed += Time.deltaTime;

                var newX = _curveX.Evaluate(TimeElapsed / _moveTime);

                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                yield return null;
            }

            Animator.SetBool(IsAttacking, false);
        }
    }
}