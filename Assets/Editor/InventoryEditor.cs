// Remy Pijuan, 2024

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    SerializedProperty overworldTime;
    SerializedProperty food;
    SerializedProperty materials;

    private void OnEnable()
    {
        overworldTime = serializedObject.FindProperty("initialOverworldTime");
        food = serializedObject.FindProperty("initialFood");
        materials = serializedObject.FindProperty("initialConstructionMaterials");
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 200;
        EditorGUILayout.PropertyField(overworldTime, new GUIContent("Initial Overworld Time (years)"));
        EditorGUILayout.PropertyField(food);
        EditorGUILayout.PropertyField(materials);
        serializedObject.ApplyModifiedProperties();
    }
}