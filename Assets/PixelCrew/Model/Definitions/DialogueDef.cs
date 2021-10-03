using PixelCrew.Model.Data.Dialogue;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/Dialog", fileName = "Dialog")]
    public class DialogueDef : ScriptableObject
    {
        [SerializeField] private DialogueData _data;
        public DialogueData Data => _data;
    }
}