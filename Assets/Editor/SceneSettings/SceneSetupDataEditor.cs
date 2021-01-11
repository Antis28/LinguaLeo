// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#region

using System;
using System.Linq;
using SceneSettings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;

#endregion


namespace SceneSettings
{
    [CustomEditor(typeof(SceneSetupData))]
    [CanEditMultipleObjects]
    public class SceneSetupDataEditor : Editor
    {
        SerializedProperty setupProperty;
        SceneSetupData sceneSetupData;

        void OnEnable()
        {
            setupProperty = serializedObject.FindProperty("setup");
            sceneSetupData = (SceneSetupData) target;
        }


        public override void OnInspectorGUI()
        {
            if (sceneSetupData.setup != null && sceneSetupData.setup.Count > 0)
            {
                //foreach (var itemData in )
                for (var i = 0; i < sceneSetupData.setup.Count; i++)
                {
                    var setupData = sceneSetupData.setup[i];
                    EditorGUILayout.BeginVertical("box");

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(15)))
                    {
                        sceneSetupData.setup.Remove(setupData);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();

                    setupData.name = EditorGUILayout.TextField("Имя установки", setupData.name);
                    EditorGUILayout.IntField("Индекс", setupData.index);

                    EditorGUILayout.SelectableLabel("Подсцены:");
                    setupData.pathToScenes =  EditorGUILayout.TextField("Путь к сценам:", setupData.pathToScenes);
                    
                    foreach (var setupWrap in setupData.setup)
                    {
                        EditorGUILayout.BeginVertical("box");
                        setupWrap.PathToScenes = setupData.pathToScenes;

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(15)))
                        {
                            Array.Resize(ref setupData.setup, setupData.setup.Length - 1);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        setupWrap.Path =
                            (SceneAsset) EditorGUILayout.ObjectField("Путь к сцене", setupWrap.Path, typeof(SceneAsset),
                                                                     false);
                        setupWrap.IsActive = EditorGUILayout.Toggle("Сцена активна?", setupWrap.IsActive);
                        setupWrap.IsLoaded = EditorGUILayout.Toggle("Сцена загружена?", setupWrap.IsLoaded);
                        EditorGUILayout.EndVertical();
                    }

                    if (GUILayout.Button("Add sub scene"))
                    {
                        Array.Resize(ref setupData.setup, setupData.setup.Length + 1);
                        setupData.setup[setupData.setup.Length - 1] = new SceneSetupWrap();
                    }

                    EditorGUILayout.EndVertical();
                    sceneSetupData.setup[i] = setupData;
                }
            }

            if (GUILayout.Button("Add scene for settings"))
            {
                var item = new SceneSetupData.SetupData
                {
                    name = "not name",
                    index = sceneSetupData.setup.Count + 1,
                    setup = new SceneSetupWrap[1] { new SceneSetupWrap()
                    {
                        IsLoaded = true,
                        IsActive = true
                    }},
                    pathToScenes = "Assets/Scenes/"
                };
                sceneSetupData.setup.Add(item);
            }
        }

        public static void SetObjectDirty(GameObject obj)
        {
            EditorUtility.SetDirty(obj);
            EditorSceneManager.MarkSceneDirty(obj.scene);
        }
    }
}
