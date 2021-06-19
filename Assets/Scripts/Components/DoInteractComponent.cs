using UnityEngine;

namespace PixelCrew.Components
{
    public class DoInteractComponent : MonoBehaviour
    {
        public void DoInteract(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();
            if (interactable != null) interactable.Interact();
        }
    }
}
