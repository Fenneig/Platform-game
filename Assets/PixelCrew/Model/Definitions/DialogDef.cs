using PixelCrew.Model.Data.Dialog;
using UnityEngine;

namespace Assets.PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/Dialog", fileName = "Dialog")]
    public class DialogDef : ScriptableObject

    {
        [SerializeField] private DialogData _data;
        public DialogData Data => _data;
    }
}