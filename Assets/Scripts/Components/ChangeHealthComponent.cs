using UnityEngine;

namespace PixelCrew.Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _value;
        //Вносит изменение в здоровье цели прибавляя здоровье при положительном value и отнимая при отрицательном value
        public void ApplyChanges(GameObject target) 
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            
            if (healthComponent != null) healthComponent.ChangeHealth(_value);
        }
    }
}