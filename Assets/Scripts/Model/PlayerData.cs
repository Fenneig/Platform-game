using System;

namespace PixelCrew.Model
{
    [Serializable]
    class PlayerData
    {
        public int Coins;
        public int Hp;
        public int Swords;
        public bool IsArmed;

        public PlayerData(PlayerData data)
        {
            Coins = data.Coins;
            Hp = data.Hp;
            Swords = data.Swords;
            IsArmed = data.IsArmed;
        }

        public PlayerData() { }
    }
}
