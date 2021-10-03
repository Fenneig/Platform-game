using Assets.PixelCrew.Utils.Editor;
using UnityEditor;

namespace PixelCrew.Components.Dialogue.Editor
{
    [CustomEditor(typeof(ShowDialogueComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);

            ShowDialogueComponent.Mode mode;
            if (_modeProperty.GetEnum(out mode))
            {
                switch (mode)
                {
                    case ShowDialogueComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case ShowDialogueComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                    default: break;
                }

            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}