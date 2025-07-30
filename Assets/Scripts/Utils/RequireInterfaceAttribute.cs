﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GMTK2025.Utils
{
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        public Type type;

        public RequireInterfaceAttribute(Type _type)
        {
            type = _type;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
    public class InterfaceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            // First get the attribute since it contains the range for the slider
            var requiredInterface = attribute as RequireInterfaceAttribute;

            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                EditorGUI.ObjectField(position, property, requiredInterface.type);

                if (property.objectReferenceValue == null || requiredInterface.type.IsInstanceOfType(property.objectReferenceValue)) { return; }

                Object selected = null;

                var gameObj = property.objectReferenceValue as GameObject;
                if (gameObj != null)
                {
                    var components = gameObj.GetComponents<Component>();
                    foreach (var component in components)
                    {
                        if (requiredInterface.type.IsInstanceOfType(component))
                        {
                            selected = component;
                            break;
                        }
                    }
                }

                property.objectReferenceValue = selected;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use RequireInterface with Object.");
            }
        }
    }
#endif
}