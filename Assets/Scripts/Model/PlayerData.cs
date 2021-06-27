using System;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    class PlayerData
    {
        public int Coins;
        public int Hp;
        public int Swords;
        public bool IsArmed;

        public PlayerData Clone() 
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}
