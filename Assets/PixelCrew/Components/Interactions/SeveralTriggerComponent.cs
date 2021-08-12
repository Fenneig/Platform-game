using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    public class SeveralTriggerComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;
        [SerializeField] private int[] _trigger;

        public void SwitchTrigger(int triggerNumber) 
        {
            _trigger[triggerNumber]++;
            
            CheckActivate();
        }

        private void CheckActivate() 
        {
            bool flag = true;
            
            foreach (int i in _trigger) if (i % 2 == 0) flag = false;

            if (flag) _action?.Invoke();
        }
    }
}
