using HereBeElements.Components;
using UnityEditor;

namespace com.herebedragons.herebeelements.Editor
{
    
    [CustomEditor(typeof(ShaderControl))]
    public class ShaderControlEditor : UnityEditor.Editor
    {
        private SerializedProperty _shaderConf;
        private SerializedProperty _isStatic;

        private SerializedProperty _opacity;
        private SerializedProperty _tintColor;
        private SerializedProperty _alphaClip;
        private SerializedProperty _border;
        private SerializedProperty _borderWidth;
        private SerializedProperty _borderColor;


        private void OnEnable()
        {
            _shaderConf = serializedObject.FindProperty("config");
            _opacity = _shaderConf.FindPropertyRelative("opacity");
            _tintColor = _shaderConf.FindPropertyRelative("tintColor");
            _alphaClip = _shaderConf.FindPropertyRelative("alphaClipThreshold");
            _border = _shaderConf.FindPropertyRelative("border");
            _borderWidth = _shaderConf.FindPropertyRelative("borderWidth");
            _borderColor = _shaderConf.FindPropertyRelative("borderColor");
            
            _isStatic = serializedObject.FindProperty("isStatic");
          
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_isStatic);
            if (!_isStatic.boolValue)
            {
                EditorGUILayout.PropertyField(_opacity);
                EditorGUILayout.PropertyField(_tintColor);
                EditorGUILayout.PropertyField(_alphaClip);

                EditorGUILayout.PropertyField(_border);
                if (_border.boolValue)
                {
                    EditorGUILayout.PropertyField(_borderWidth);
                    EditorGUILayout.PropertyField(_borderColor);
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}