using UnityEditor;
using UnityEngine;

namespace UnityCSCommon.Utils.InspectorWrappers
{
    /// <summary>
    /// Property drawer for <see cref="LayerWrapper"/> class.
    /// </summary>
    [CustomPropertyDrawer (typeof (LayerWrapper))]
    public class LayerWrapperPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty (_position, GUIContent.none, _property);
            SerializedProperty layerIndex = _property.FindPropertyRelative ("_layerIndex");
            _position = EditorGUI.PrefixLabel (_position, GUIUtility.GetControlID (FocusType.Passive), _label);

            if (layerIndex != null)
            {
                layerIndex.intValue = EditorGUI.LayerField (_position, layerIndex.intValue);
            }

            EditorGUI.EndProperty();
        }
    }
}