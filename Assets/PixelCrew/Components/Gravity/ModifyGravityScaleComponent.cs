using UnityEngine;

namespace PixelCrew.Components.Gravity
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ModifyGravityScaleComponent : MonoBehaviour
    {
        public void MultiplyGravityByValue(float value) => gameObject.GetComponent<Rigidbody2D>().gravityScale *= value;
        public void DivideGravityByValue(float value) => gameObject.GetComponent<Rigidbody2D>().gravityScale /= value;
    }
}
