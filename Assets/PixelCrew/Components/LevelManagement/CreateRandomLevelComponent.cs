using System.Linq;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{
    public class CreateRandomLevelComponent : MonoBehaviour
    {
        [SerializeField] private int _roomHeight;
        [SerializeField] private int _roomWidth;
        private ChunkDef[] _possibleChunks;
        private ChunkDef[,] _rooms;

        private void Start()
        {
            _rooms = new ChunkDef[_roomHeight, _roomWidth];
            _possibleChunks = DefsFacade.I.Chunks.All;
            for (var i = 0; i < _roomHeight; i++)
            {
                for (var j = 0; j < _roomWidth; j++)
                {
                    do
                    {
                        var random = Random.Range(0, _possibleChunks.Length);
                        _rooms[i, j] = _possibleChunks[random];
                    } while (_rooms[i, j].ChunkType != ChunkType.Entrance);
                }
            }
            _rooms[1, 0] = _possibleChunks.FirstOrDefault(chunk => chunk.ChunkType == ChunkType.Entrance);
        }
    }
}