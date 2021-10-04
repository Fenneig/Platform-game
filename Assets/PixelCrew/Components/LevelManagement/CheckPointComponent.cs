using System;
using PixelCrew.Components.GOBased;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.LevelManagement
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private SpawnComponent _hero;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;

        public string Id => _id;

        private GameSession _session;

        private void Awake()
        {
            _session = FindObjectOfType<GameSession>();
            if (_session.IsChecked(_id))
                _setChecked?.Invoke();
            else
                _setUnchecked?.Invoke();
        }

        public void Check()
        {
            _session.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _hero.Spawn();
        }
    }
}