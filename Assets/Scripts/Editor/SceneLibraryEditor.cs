using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// todo: Draw a description above each column (e.g. Index, Scene, Enabled)
[CustomEditor(typeof(SceneLibrary))]
public class SceneLibraryEditor : Editor
{
    private ReorderableList _sceneList;

    private void OnEnable()
    {
        _sceneList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("Scenes"),
            true, true, true, true);

        // Draw 'SceneAssets' property
        // Define header
        _sceneList.drawHeaderCallback = area => {
            EditorGUI.LabelField(area, "Scenes");
        };

        // Draw each SceneLibraryItem element
        _sceneList.drawElementCallback = (area, index, isActive, isFocused) =>
        {
            var element = _sceneList.serializedProperty.GetArrayElementAtIndex(index);
            area.y += 2;
            EditorGUI.LabelField(
                new Rect(area.x, area.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Index").intValue.ToString(), GUIStyle.none);
            EditorGUI.PropertyField(
                new Rect(area.x + 60, area.y, area.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Scene"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(area.x + area.width - 30, area.y, 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Enabled"), GUIContent.none);
        };

        // Initialize newly created element
        _sceneList.onAddCallback = reordableList => 
        {
            var index = reordableList.serializedProperty.arraySize;
            reordableList.index = index;

            reordableList.serializedProperty.arraySize++;

            var element = reordableList.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Index").intValue = index;
            element.FindPropertyRelative("Enabled").boolValue = true;
        };

        // Set the index of element accordingly
        _sceneList.onChangedCallback = reordableList =>
        {
            var arraySize = reordableList.serializedProperty.arraySize;
            for (var i = 0; i < arraySize; i++)
            {
                var element = reordableList.serializedProperty.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("Index").intValue = i;
            }            
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _sceneList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        Save();
    }

    private void Save()
    {
        var arraySize = _sceneList.serializedProperty.arraySize;
        var newEditorBuildSettingScenes = new EditorBuildSettingsScene[arraySize];
        for (var i = 0; i < arraySize; i++)
        {
            var element = _sceneList.serializedProperty.GetArrayElementAtIndex(i);

            var sceneAsset = (SceneAsset)element.FindPropertyRelative("Scene").objectReferenceValue;
            var scenePath = AssetDatabase.GetAssetOrScenePath(sceneAsset);
            var enabled = element.FindPropertyRelative("Enabled").boolValue;

            newEditorBuildSettingScenes[i] = new EditorBuildSettingsScene(scenePath, enabled);
        }

        EditorBuildSettings.scenes = newEditorBuildSettingScenes;
    }
}
