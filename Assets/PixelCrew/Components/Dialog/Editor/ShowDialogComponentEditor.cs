using Assets.PixelCrew.Utils.Editor;
using UnityEditor;

namespace PixelCrew.Components.Dialog.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
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

            ShowDialogComponent.Mode mode;
            if (_modeProperty.GetEnum(out mode))
            {
                switch (mode)
                {
                    case ShowDialogComponent.Mode.bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case ShowDialogComponent.Mode.external:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                    default: break;
                }

            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}