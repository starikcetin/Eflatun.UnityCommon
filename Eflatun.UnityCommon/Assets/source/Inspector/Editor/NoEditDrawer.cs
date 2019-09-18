using UnityEditor;
using UnityEngine;

namespace starikcetin.Eflatun.UnityCommon.Inspector.Editor
{
    /// <summary>
    /// Property drawer for <see cref="NoEdit"/> attribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(NoEdit))]
    public class NoEditDrawer : PropertyDrawer
    {
        /// <summary>
        /// Display attribute and his value in inspector depending on the type
        /// Fill attribute needed
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //
            // Source: http://answers.unity.com/answers/801283/view.html
            //

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;

            //
            // The below approach also works, and works nicer, but it is too tedious.
            //

            //switch (property.propertyType)
            //{
            //    case SerializedPropertyType.AnimationCurve:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.animationCurveValue.ToString()));
            //        break;
            //    case SerializedPropertyType.ArraySize:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.arraySize.ToString()));
            //        break;
            //    case SerializedPropertyType.Boolean:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.boolValue.ToString()));
            //        break;
            //    case SerializedPropertyType.Bounds:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.boundsValue.ToString()));
            //        break;
            //    case SerializedPropertyType.Character:
            //        break;
            //    case SerializedPropertyType.Color:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.colorValue.ToString()));
            //        break;
            //    case SerializedPropertyType.Enum:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.enumDisplayNames[property.enumValueIndex]));
            //        break;
            //    case SerializedPropertyType.Float:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.floatValue.ToString()));
            //        break;
            //    case SerializedPropertyType.Generic:
            //        break;
            //    case SerializedPropertyType.Gradient:
            //        break;
            //    case SerializedPropertyType.Integer:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.intValue.ToString()));
            //        break;
            //    case SerializedPropertyType.LayerMask:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.intValue.ToString()));
            //        break;
            //    case SerializedPropertyType.ObjectReference:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.objectReferenceValue.name.ToString()));
            //        break;
            //    case SerializedPropertyType.Quaternion:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.quaternionValue.ToString()));
            //        break;
            //    case SerializedPropertyType.Rect:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.rectValue.ToString()));
            //        break;
            //    case SerializedPropertyType.String:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.stringValue));
            //        break;
            //    case SerializedPropertyType.Vector2:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.vector2Value.ToString()));
            //        break;
            //    case SerializedPropertyType.Vector3:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.vector3Value.ToString()));
            //        break;
            //    case SerializedPropertyType.Vector4:
            //        EditorGUI.LabelField(position, label, new GUIContent(property.vector4Value.ToString()));
            //        break;
            //}
        }
    }
}
