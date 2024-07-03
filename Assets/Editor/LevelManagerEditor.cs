// Remy Pijuan, 2024

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    SerializedProperty inputs;

    SerializedProperty levelTime;

    SerializedProperty startingFoodAmount;
    SerializedProperty startingConstructionMaterialAmount;

    // Level Success Conditions
    SerializedProperty successFoodAmount;
    SerializedProperty successConstructionMaterialsAmount;
    SerializedProperty successSoilHealth;

    SerializedProperty waterOutline;
    SerializedProperty energyOutline;
    SerializedProperty extraOutline;

    private void OnEnable()
    {
        inputs = serializedObject.FindProperty("inputs");

        levelTime = serializedObject.FindProperty("levelTimeStore");

        startingFoodAmount = serializedObject.FindProperty("startingFoodAmount");
        startingConstructionMaterialAmount = serializedObject.FindProperty("startingConstructionMaterialAmount");
        
        successFoodAmount = serializedObject.FindProperty("successFoodAmount");
        successConstructionMaterialsAmount = serializedObject.FindProperty("successConstructionMaterialsAmount");
        successSoilHealth = serializedObject.FindProperty("successSoilHealth");

        waterOutline = serializedObject.FindProperty("waterOutline");
        energyOutline = serializedObject.FindProperty("energyOutline");
        extraOutline = serializedObject.FindProperty("extraOutline");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(inputs);

        EditorGUIUtility.labelWidth = 300;
        EditorGUILayout.PropertyField(levelTime, new GUIContent("Time Taken By Level (months)"));

        EditorGUILayout.PropertyField(startingFoodAmount);
        EditorGUILayout.PropertyField(startingConstructionMaterialAmount);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Success Conditions", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(successFoodAmount);
        EditorGUILayout.PropertyField(successConstructionMaterialsAmount);
        EditorGUILayout.PropertyField(successSoilHealth);

        EditorGUILayout.PropertyField(waterOutline);
        EditorGUILayout.PropertyField(energyOutline);
        EditorGUILayout.PropertyField(extraOutline);

        serializedObject.ApplyModifiedProperties();
    }
}
