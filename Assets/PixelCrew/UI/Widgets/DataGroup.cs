using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class DataGroup<TDataType, TItemType> where  TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        protected List<TItemType> CreatedItems = new List<TItemType>();

        private TItemType _prefab;
        private Transform _container;

        public DataGroup(TItemType prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public virtual void SetData(IList<TDataType> data)
        {
            //создаются запрашиваемые предметы
            for (var i = CreatedItems.Count; i < data.Count(); i++)
            {
                var item = Object.Instantiate(_prefab, _container);
                CreatedItems.Add(item);
            }

            //обнавляются данные и включаются предметы
            for (var i = 0; i < data.Count; i++) 
            {
                CreatedItems[i].SetData(data[i], i);
                CreatedItems[i].gameObject.SetActive(true);
            }
            
            //скрываются неиспользуемые предметы
            for (var i = data.Count; i < CreatedItems.Count; i++)
            {
                CreatedItems[i].gameObject.SetActive(false);
            }
        }
    }

    public interface IItemRenderer<TDataType>
    {
        void SetData(TDataType dataInfo, int index);
    }
}