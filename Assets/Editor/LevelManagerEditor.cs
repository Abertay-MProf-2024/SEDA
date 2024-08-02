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

    SerializedProperty  RiverTileBase;
    //SerializedProperty MountainTileBase;

    SerializedProperty waterOutline;
    SerializedProperty energyOutline;
    SerializedProperty extraOutline;
    SerializedProperty waterSplash;

    SerializedProperty musicPlayer;
    SerializedProperty buildingCostUI;

    SerializedProperty startingMonth;
    SerializedProperty startingDay;


    private void OnEnable()
    {
        sceneMusic = serializedObject.FindProperty("sceneMusic");

        levelTime = serializedObject.FindProperty("levelTimeStore");

        startingFoodAmount = serializedObject.FindProperty("startingFoodAmount");
        startingConstructionMaterialAmount = serializedObject.FindProperty("startingConstructionMaterialAmount");
        
        successFoodAmount = serializedObject.FindProperty("successFoodAmount");
        successConstructionMaterialsAmount = serializedObject.FindProperty("successConstructionMaterialsAmount");
        successSoilHealth = serializedObject.FindProperty("successSoilHealth");

        RiverTileBase = serializedObject.FindProperty("RiverTileBase");
        //MountainTileBase = serializedObject.FindProperty("MountainTileBase");

        waterOutline = serializedObject.FindProperty("waterOutline");
        energyOutline = serializedObject.FindProperty("energyOutline");
        extraOutline = serializedObject.FindProperty("extraOutline");
        waterSplash = serializedObject.FindProperty("waterSplash");

        musicPlayer = serializedObject.FindProperty("musicPlayer");

        buildingCostUI = serializedObject.FindProperty("buildingCostUI");

        startingMonth = serializedObject.FindProperty("startingMonth");
        startingDay = serializedObject.FindProperty("startingDay");
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

        EditorGUILayout.PropertyField(RiverTileBase);
        //EditorGUILayout.PropertyField(MountainTileBase);

        EditorGUILayout.PropertyField(waterOutline);
        EditorGUILayout.PropertyField(energyOutline);
        EditorGUILayout.PropertyField(extraOutline);
        EditorGUILayout.PropertyField(waterSplash);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(musicPlayer);
        EditorGUILayout.PropertyField(buildingCostUI);

        EditorGUILayout.PropertyField(startingMonth);
        EditorGUILayout.PropertyField(startingDay);

        serializedObject.ApplyModifiedProperties();
    }
}
