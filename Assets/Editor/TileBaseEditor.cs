using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileBase))]
public class TileBaseEditor : Editor
{
    //general
    SerializedProperty tileName;
    SerializedProperty icon;
    SerializedProperty inGameAsset;
    SerializedProperty sizeWidth;
    SerializedProperty sizeLength;
    SerializedProperty structureType;
    SerializedProperty tileUnder;
    SerializedProperty biomeTypes;
    SerializedProperty tileTerrainTypes;

    //creatureSection
    SerializedProperty CreatureTile;
    SerializedProperty bribeCostFood;
    SerializedProperty bribeCostConstruction;

    //buildSection
    SerializedProperty canBuild;
    SerializedProperty buildingCostFood;
    SerializedProperty buildingCostMaterial;
    SerializedProperty upKeepCostEnergy;
    SerializedProperty upKeepCostWater;
    SerializedProperty upKeepCostFood;
    SerializedProperty upKeepCostMaterial;

    //Building
    SerializedProperty isResourceTapped;
    SerializedProperty baseOutputEnergy;
    SerializedProperty baseOutputWater;
    SerializedProperty baseOutputFood;
    SerializedProperty baseOutputMaterial;
    SerializedProperty buildingOutputMulti;

    //Impact
    SerializedProperty impactSource;
    SerializedProperty impactRadiusTiles;
    SerializedProperty buffAmount;
    SerializedProperty nerfAmount;
    SerializedProperty tileImpactBuff;
    SerializedProperty tileImpactNerf;
    SerializedProperty isImpactSoilGrade;
    SerializedProperty buffSoilGradeAmount;
    SerializedProperty nerfSoilGradeAmount;


    private void OnEnable()
    {
        //pre-loading the properties
        //General
        tileName = serializedObject.FindProperty("tileName");
        icon = serializedObject.FindProperty("icon");
        inGameAsset = serializedObject.FindProperty("inGameAsset");
        sizeWidth = serializedObject.FindProperty("sizeWidthTile");
        sizeLength = serializedObject.FindProperty("sizeLengthTile");
        structureType = serializedObject.FindProperty("structureType");
        tileUnder = serializedObject.FindProperty("tileUnder");
        biomeTypes = serializedObject.FindProperty("biomeTypes");
        tileTerrainTypes = serializedObject.FindProperty("tileTerrainTypes");

        //Creature
        CreatureTile = serializedObject.FindProperty("CreatureTile");
        bribeCostFood = serializedObject.FindProperty("bribeCostFood");
        bribeCostConstruction = serializedObject.FindProperty("bribeCostConstruction");

        //Build
        canBuild = serializedObject.FindProperty("canBuild");
        buildingCostFood = serializedObject.FindProperty("buildingCostFood");
        buildingCostMaterial = serializedObject.FindProperty("buildingCostMaterial");
        upKeepCostEnergy = serializedObject.FindProperty("upKeepCostEnergy");
        upKeepCostWater = serializedObject.FindProperty("upKeepCostWater");
        upKeepCostFood = serializedObject.FindProperty("upKeepCostFood");
        upKeepCostMaterial = serializedObject.FindProperty("upKeepCostMaterial");

        //Building
        isResourceTapped = serializedObject.FindProperty("isResourceTapped");
        baseOutputEnergy = serializedObject.FindProperty("baseOutputEnergy");
        baseOutputWater = serializedObject.FindProperty("baseOutputWater");
        baseOutputFood = serializedObject.FindProperty("baseOutputFood");
        baseOutputMaterial = serializedObject.FindProperty("baseOutputMaterial");
        buildingOutputMulti = serializedObject.FindProperty("buildingOutputMulti");

        //Impact
        impactSource = serializedObject.FindProperty("impactSource");
        impactRadiusTiles = serializedObject.FindProperty("impactRadiusTiles");
        buffAmount = serializedObject.FindProperty("buffAmount");
        nerfAmount = serializedObject.FindProperty("nerfAmount");
        tileImpactBuff = serializedObject.FindProperty("tileImpactBuff");
        tileImpactNerf = serializedObject.FindProperty("tileImpactNerf");
        isImpactSoilGrade = serializedObject.FindProperty("isImpactSoilGrade");
        buffSoilGradeAmount = serializedObject.FindProperty("buffSoilGradeAmount");
        nerfSoilGradeAmount = serializedObject.FindProperty("nerfSoilGradeAmount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        // Always visible properties
        EditorGUILayout.PropertyField(tileName);
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(inGameAsset);
        EditorGUILayout.PropertyField(sizeWidth);
        EditorGUILayout.PropertyField(sizeLength);
        EditorGUILayout.PropertyField(structureType);
        EditorGUILayout.PropertyField(tileUnder);
        EditorGUILayout.PropertyField(biomeTypes);
        EditorGUILayout.PropertyField(tileTerrainTypes);

        EditorGUILayout.PropertyField(CreatureTile);
        if(CreatureTile.boolValue)
        {
            //build
            EditorGUILayout.PropertyField(bribeCostFood, true);
            //build
            EditorGUILayout.PropertyField(bribeCostConstruction, true);
        }

        //BoolValue
        EditorGUILayout.PropertyField(canBuild);
        // Conditional visible properties
        if (canBuild.boolValue)
        {
            //build
            EditorGUILayout.PropertyField(buildingCostFood, true);
            EditorGUILayout.PropertyField(buildingCostMaterial, true);
            EditorGUILayout.PropertyField(upKeepCostEnergy, true);
            EditorGUILayout.PropertyField(upKeepCostWater, true);
            EditorGUILayout.PropertyField(upKeepCostFood, true);
            EditorGUILayout.PropertyField(upKeepCostMaterial, true);
        }

        // Always visible properties
        EditorGUILayout.PropertyField(isResourceTapped, true);
        EditorGUILayout.PropertyField(baseOutputEnergy, true);
        EditorGUILayout.PropertyField(baseOutputWater, true);
        EditorGUILayout.PropertyField(baseOutputFood, true);
        EditorGUILayout.PropertyField(baseOutputMaterial, true);
        EditorGUILayout.PropertyField(buildingOutputMulti, true);

        EditorGUILayout.PropertyField(impactSource);

        if (impactSource.boolValue)
        {
            EditorGUILayout.PropertyField(impactRadiusTiles, true);
            EditorGUILayout.PropertyField(buffAmount, true);
            EditorGUILayout.PropertyField(nerfAmount, true);
            EditorGUILayout.PropertyField(tileImpactBuff, true);
            EditorGUILayout.PropertyField(tileImpactNerf, true);
            EditorGUILayout.PropertyField(isImpactSoilGrade, true);
            EditorGUILayout.PropertyField(buffSoilGradeAmount, true);
            EditorGUILayout.PropertyField(nerfSoilGradeAmount, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}