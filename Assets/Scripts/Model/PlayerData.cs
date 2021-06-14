using System;

namespace PixelCrew.Model
{
    [Serializable]
    class PlayerData
    {
        public int Coins;
        public int Hp;
        public bool IsArmed;

        public PlayerData(PlayerData data) 
        {
            Coins = data.Coins;
            Hp = data.Hp;
            IsArmed = data.IsArmed;
        }

        public PlayerData() { }
    }
}
