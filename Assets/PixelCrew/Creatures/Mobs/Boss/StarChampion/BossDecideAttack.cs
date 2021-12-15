using PixelCrew.Components.ColliderBased;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Creatures.Mobs.Boss.StarChampion
{
    public class BossDecideAttack : MonoBehaviour
    {
        [SerializeField] private LayerCheck _layerCheck;
        [SerializeField] private Animator _animator;
        [SerializeField] [Range(0, 100)] private int _chanceToJumpAttack;
        private static readonly int IsJumpAttack = Animator.StringToHash("IsJumpAttack");
        private Hero.Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero.Hero>();
        }

        public void Decide()
        {
            if (_layerCheck.IsTouchingLayer && Random.Range(0, 100) <= _chanceToJumpAttack)
                _animator.SetBool(IsJumpAttack, true);
            else _animator.SetBool(IsJumpAttack, false);
        }

        public void FaceHero()
        {
            var scale = transform.localScale;
            transform.localScale = _hero.transform.position.x < transform.position.x
                ? new Vector3(Mathf.Abs(scale.x) * -1, scale.y, scale.z)
                : new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
    }
}