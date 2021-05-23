using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(DestroyObjectComponent))]
    public class CollectObjectComponent : MonoBehaviour
    {
        [SerializeField] private Hero _hero; 
        [SerializeField] private UnityEvent _destroyAction;
        public void CollectObject() 
        {
            switch (tag) 
            {
                case "Gold": 
                    {
                        _hero.CollectCoin(10);
                        _hero.SayCoins();
                        _destroyAction.Invoke();
                        break;
                    }
                case "Silver": 
                    {
                        _hero.CollectCoin(1);
                        _hero.SayCoins();
                        _destroyAction.Invoke();
                        break;
                    }
                default: 
                    {
                        _destroyAction.Invoke();
                        break;
                    }
            }
        }

    }
}