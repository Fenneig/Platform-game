using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour, IUsable
    {
        [SerializeField] private int _delta;

        public int Delta
        {
            get => _delta;
            set => _delta = value;
        }

        //Вносит изменение в здоровье цели прибавляя здоровье при положительном value и отнимая при отрицательном value
        public void Apply(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent != null) healthComponent.ModifyHealthByDelta(Delta);
        }

        public void Use(GameObject target) => Apply(target);
    }
}