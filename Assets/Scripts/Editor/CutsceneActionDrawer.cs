using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CutsceneAction))]
public class CutsceneActionDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight + 2;

        SerializedProperty actionType = property.FindPropertyRelative("actionType");

        if (actionType.enumValueIndex == (int)CutsceneActionType.Wait)
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("waitDuration"), true) + 2;

        else if (actionType.enumValueIndex == (int)CutsceneActionType.Dialogue)
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("dialogueData"), true) + 2;

        else if (actionType.enumValueIndex == (int)CutsceneActionType.MoveActors)
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("actorsToMove"), true) + 2;


        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        SerializedProperty actionType = property.FindPropertyRelative("actionType");

        EditorGUI.PropertyField(rect, actionType);
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        if (actionType.enumValueIndex == (int)CutsceneActionType.Wait)
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("waitDuration"), true);

        else if (actionType.enumValueIndex == (int)CutsceneActionType.Dialogue)
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("dialogueData"), true);

        else if (actionType.enumValueIndex == (int)CutsceneActionType.MoveActors)
        {
            SerializedProperty actorsProp = property.FindPropertyRelative("actorsToMove");
            EditorGUI.PropertyField(rect, actorsProp, true);
        }

        EditorGUI.EndProperty();
    }
}