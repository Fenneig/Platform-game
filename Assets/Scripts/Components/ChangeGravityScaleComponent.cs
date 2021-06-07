using UnityEngine;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ChangeGravityScaleComponent : MonoBehaviour
    {
        public void MultiplyGravityByValue(float value) => gameObject.GetComponent<Rigidbody2D>().gravityScale *= value;
        public void DivideGravityByValue(float value) => gameObject.GetComponent<Rigidbody2D>().gravityScale /= value;
    }
}
