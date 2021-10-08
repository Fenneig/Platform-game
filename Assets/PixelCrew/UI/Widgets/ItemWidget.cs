using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;


        public void SetData(ItemWithCount defPrice)
        {
            var def = DefsFacade.I.Items.Get(defPrice.ItemId);
            _icon.sprite = def.Icon;
            _value.text = defPrice.Count.ToString();
        }
    }
}