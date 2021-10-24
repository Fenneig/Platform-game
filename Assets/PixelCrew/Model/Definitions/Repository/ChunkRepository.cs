using System;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository
{
    [CreateAssetMenu(menuName = "Defs/Repository/Chunks", fileName = "Chunk")]
    public class ChunkRepository : DefRepository<ChunkDef>
    {
    }

    [Serializable]
    public struct ChunkDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private ConnectionSide[] _connections;
        [SerializeField] private ChunkType _chunkType;

        public ChunkType ChunkType => _chunkType;

        public ConnectionSide[] Connections => _connections;

        public string Id => _id;

        public GameObject Prefab => _prefab;
    }

    [Serializable]
    public enum ChunkType
    {
        Normal,
        Entrance,
        Exit
    }

    [Serializable]
    public enum ConnectionSide
    {
        Left,
        Up,
        Right,
        Down
    }
}