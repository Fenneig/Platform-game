using PixelCrew.Utils.Editor;
using UnityEditor;

namespace PixelCrew.Components.Dialogue.Editor
{
    [CustomEditor(typeof(ShowDialogueComponent))]
    public class ShowDialogueComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;
        private SerializedProperty _onCompleteProperty;
        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
            _onCompleteProperty = serializedObject.FindProperty("_onComplete");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);

            if (_modeProperty.GetEnum(out ShowDialogueComponent.Mode mode))
            {
                switch (mode)
                {
                    case ShowDialogueComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case ShowDialogueComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                }

            }
            
            EditorGUILayout.PropertyField(_onCompleteProperty);
            serializedObject.ApplyModifiedProperties();
        }

    }
}