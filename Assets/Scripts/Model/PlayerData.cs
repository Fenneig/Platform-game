using System;

namespace PixelCrew.Model
{
    [Serializable]
    class PlayerData
    {
        public int Coins;
        public int Hp;
        public int Swords;

        public PlayerData(PlayerData data)
        {
            Coins = data.Coins;
            Hp = data.Hp;
            Swords = data.Swords;
        }

        public PlayerData() { }
    }
}
