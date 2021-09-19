using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    public class SeveralTriggerComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;
        [SerializeField] private bool[] _trigger;

        public void SwitchTrigger(int triggerNumber) 
        {
            _trigger[triggerNumber] = !_trigger[triggerNumber];
            
            CheckActivate();
        }

        private void CheckActivate() 
        {
            bool flag = true;
            
            foreach (bool checkTrigger in _trigger) if (!checkTrigger) flag = false;

            if (flag) _action?.Invoke();
        }
    }
}
