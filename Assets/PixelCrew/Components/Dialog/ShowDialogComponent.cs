using Assets.PixelCrew.Model.Definitions;
using PixelCrew.Model.Data.Dialog;
using PixelCrew.UI.HUD.Dialog;
using System;
using UnityEngine;

namespace PixelCrew.Components.Dialog
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxController _dialogBox;
        public void Show()
        {
            if (_dialogBox == null) _dialogBox = FindObjectOfType<DialogBoxController>();

            _dialogBox.ShowDialog(Data);
        }

        public void Show(DialogDef def) 
        {
            _external = def;
            Show();
        }

        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.bound:
                        return _bound;
                    case Mode.external:
                        return _external.Data;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum Mode
        {
            bound,
            external
        }
    }
}