using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    SerializedProperty sceneMusic;

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

    SerializedProperty musicPlayer;
    SerializedProperty buildingCostUI;


    private void OnEnable()
    {
        sceneMusic = serializedObject.FindProperty("sceneMusic");

        levelTime = serializedObject.FindProperty("levelTimeStore");

        startingFoodAmount = serializedObject.FindProperty("startingFoodAmount");
        startingConstructionMaterialAmount = serializedObject.FindProperty("startingConstructionMaterialAmount");
        
        successFoodAmount = serializedObject.FindProperty("successFoodAmount");
        successConstructionMaterialsAmount = serializedObject.FindProperty("successConstructionMaterialsAmount");
        successSoilHealth = serializedObject.FindProperty("successSoilHealth");

        waterOutline = serializedObject.FindProperty("waterOutline");
        energyOutline = serializedObject.FindProperty("energyOutline");
        extraOutline = serializedObject.FindProperty("extraOutline");

        musicPlayer = serializedObject.FindProperty("musicPlayer");

        buildingCostUI = serializedObject.FindProperty("buildingCostUI");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(sceneMusic);

        EditorGUIUtility.labelWidth = 250;
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

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(musicPlayer);
        EditorGUILayout.PropertyField(buildingCostUI);

        serializedObject.ApplyModifiedProperties();
    }
}
