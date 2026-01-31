using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueNode))]
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
            SerializedProperty tbdProp      = property.FindPropertyRelative("tbd");

            // Name
            EditorGUI.PropertyField(rect, nameProp);
            rect.y += line + spacing;

            // GUID
            EditorGUI.PropertyField(rect, guidProp);
            rect.y += line + spacing;

            // Node Type
            EditorGUI.PropertyField(rect, typeProp);
            rect.y += line + spacing;

            DialogueNode.NodeType nodeType =
                (DialogueNode.NodeType)typeProp.enumValueIndex;

            // Conditional fields
            switch (nodeType)
            {
                case DialogueNode.NodeType.START:
                    EditorGUI.PropertyField(rect, pathsProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(pathsProp, true) + spacing;
                    break;

                case DialogueNode.NodeType.DIALOGUE:
                    EditorGUI.PropertyField(rect, dialogueProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(dialogueProp, true) + spacing;

                    EditorGUI.PropertyField(rect, pathsProp, true);
                    rect.y += EditorGUI.GetPropertyHeight(pathsProp, true) + spacing;
                    break;

                case DialogueNode.NodeType.MODIFIER:
                    //EditorGUI.PropertyField(rect, tbdProp);
                    rect.y += line + spacing;
                    break;

                case DialogueNode.NodeType.END:
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
        DialogueNode.NodeType nodeType =
            (DialogueNode.NodeType)typeProp.enumValueIndex;

        switch (nodeType)
        {
            case DialogueNode.NodeType.START:
            {
                SerializedProperty paths = property.FindPropertyRelative("nodePaths");
                height += EditorGUI.GetPropertyHeight(paths, true) + spacing;
                break;
            }

            case DialogueNode.NodeType.DIALOGUE:
            {
                SerializedProperty dialogue = property.FindPropertyRelative("dialogue");
                SerializedProperty paths = property.FindPropertyRelative("nodePaths");

                height += EditorGUI.GetPropertyHeight(dialogue, true) + spacing;
                height += EditorGUI.GetPropertyHeight(paths, true) + spacing;
                break;
            }

            case DialogueNode.NodeType.MODIFIER:
                height += line + spacing;
                break;

            case DialogueNode.NodeType.END:
                // nothing extra
                break;
        }

        return height;
    }
}
