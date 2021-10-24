using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository.Items
{
    public class DefRepository<TDefType> : ScriptableObject where TDefType : IHaveId
    {
        [SerializeField] protected TDefType[] _collection;
        
        public TDefType Get(string id) =>
            string.IsNullOrEmpty(id) ? default : _collection.FirstOrDefault(x => x.Id == id);

        public TDefType[] All => new List<TDefType>(_collection).ToArray();
    }
}            
