using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueRuntimeNode))]
public class DialogueNodeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float line = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        Rect rect = new Rect(position.x, position.y, position.width, line);

        // Foldout
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);
        rect.y += line + spacing;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            SerializedProperty nameProp     = property.FindPropertyRelative("name");
            SerializedProperty guidProp     = property.FindPropertyRelative("GUID");
            SerializedProperty typeProp     = property.FindPropertyRelative("nodeType");
            SerializedProperty dialogueProp = property.FindPropertyRelative("dialogue");
            SerializedProperty pathsProp    = property.FindPropertyRelative("nodePaths");
            SerializedProperty speakerNameProp = property.FindPropertyRelative("speakerName");
            SerializedProperty wordColoursProp = property.FindPropertyRelative("wordColours");

            // Name
            EditorGUI.PropertyField(rect, nameProp);
            rect.y += line + spacing;

            // GUID
            EditorGUI.PropertyField(rect, guidProp);
            rect.y += line + spacing;

            // Node Type
            EditorGUI.PropertyField(rect, typeProp);
            rect.y += line + spacing;

            DialogueRuntimeNode.NodeType nodeType =
                (DialogueRuntimeNode.NodeType)typeProp.enumValueIndex;

            // Conditional fields
            switch (nodeType)
            {
                case DialogueRuntimeNode.NodeType.START:
                    EditorGUI.PropertyField(rect, pathsProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(pathsProp, true) + spacing;
                    break;

                case DialogueRuntimeNode.NodeType.DIALOGUE:
                    EditorGUI.PropertyField(rect, dialogueProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(dialogueProp, true) + spacing;

                    EditorGUI.PropertyField(rect, pathsProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(pathsProp, true) + spacing;

                    EditorGUI.PropertyField(rect, speakerNameProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(speakerNameProp, true) + spacing;

                    EditorGUI.PropertyField(rect, wordColoursProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(wordColoursProp, true) + spacing;
                    break;

                case DialogueRuntimeNode.NodeType.MODIFIER:
                    //EditorGUI.PropertyField(rect, tbdProp);
                    rect.y += line + spacing;
                    break;

                case DialogueRuntimeNode.NodeType.END:
                    // No extra fields
                    break;
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        // Collapsed: foldout only
        if (!property.isExpanded)
            return line;

        float height = 0f;

        // Foldout
        height += line + spacing;

        // name, GUID, nodeType
        height += (line + spacing) * 3;

        SerializedProperty typeProp = property.FindPropertyRelative("nodeType");
        DialogueRuntimeNode.NodeType nodeType =
            (DialogueRuntimeNode.NodeType)typeProp.enumValueIndex;

        switch (nodeType)
        {
            case DialogueRuntimeNode.NodeType.START:
            {
                SerializedProperty paths = property.FindPropertyRelative("nodePaths");
                height += EditorGUI.GetPropertyHeight(paths, true) + spacing;
                break;
            }

            case DialogueRuntimeNode.NodeType.DIALOGUE:
            {
                SerializedProperty dialogue = property.FindPropertyRelative("dialogue");
                SerializedProperty paths = property.FindPropertyRelative("nodePaths");
                SerializedProperty speakerName = property.FindPropertyRelative("speakerName");
                SerializedProperty wordColours = property.FindPropertyRelative("wordColours");

                height += EditorGUI.GetPropertyHeight(dialogue, true) + spacing;
                height += EditorGUI.GetPropertyHeight(paths, true) + spacing;
                height += EditorGUI.GetPropertyHeight(speakerName, true) + spacing;
                height += EditorGUI.GetPropertyHeight(wordColours, true) + spacing;
                break;
            }

            case DialogueRuntimeNode.NodeType.MODIFIER:
                height += line + spacing;
                break;

            case DialogueRuntimeNode.NodeType.END:
                // nothing extra
                break;
        }

        return height;
    }
}
