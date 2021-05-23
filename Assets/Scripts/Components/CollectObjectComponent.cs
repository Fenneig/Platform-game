using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class CollectObjectComponent : MonoBehaviour
    {
        //Сбор объектов, в зависимости от добавления тэгов будет расширяться функционал скрипта
        [SerializeField] private Hero _hero; 
        //[SerializeField] private UnityEvent _destroyAction;
        public void CollectObject() 
        {
            switch (tag) 
            {
                case "Gold": 
                    {
                        _hero.CollectCoin(10);
                        _hero.SayCoins();
                        Destroy(this.gameObject);
                        break;
                    }
                case "Silver": 
                    {
                        _hero.CollectCoin(1);
                        _hero.SayCoins();
                        Destroy(this.gameObject);
                        break;
                    }
                default:
                    {
                        Destroy(this.gameObject);
                        break;
                    }
            }
        }

    }
}