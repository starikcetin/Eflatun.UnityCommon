using UnityEditor;
using UnityEngine;

namespace starikcetin.Eflatun.UnityCommon.Inspector.Editor
{
    /// <summary>
    /// Property drawer for <see cref="LayerWrapper"/> class. <para />
    /// Draws a drop-down list of all available layers to choose from.
    /// </summary>
    [CustomPropertyDrawer(typeof(LayerWrapper))]
    public class LayerWrapperDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            SerializedProperty layerIndex = property.FindPropertyRelative("_layerIndex");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (layerIndex != null)
            {
                layerIndex.intValue = EditorGUI.LayerField(position, layerIndex.intValue);
            }

            EditorGUI.EndProperty();
        }
    }
}
