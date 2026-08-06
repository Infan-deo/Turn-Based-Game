using System;
using System.Collections.Generic;
using System.Diagnostics;
using GGCustomToolbar;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.IO;
using BayatGames.SaveGameFree;

// using MansionEscape3D.SaveSystem;

namespace MansionEscape3D.Editor
{
    public static class EditorCustomToolbar
    {
        private static bool _isCustomPlayMode;

        [EditorToolbarButton("Cancel", "Reset Save Data", 0, EditorToolbarPosition.LeftLeft, true)]
        public static void ClearSaveDataButton()
        {
            // SaveManager.DeleteSaveDataFiles();
            SaveGame.DeleteAll();
        }

        [EditorToolbarButton("Folder Icon", "Open Project Folder", 1)]
        public static void OpenProjectFolder()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }

        [EditorToolbarButton("UnityEditor.VersionControl", "Open Version Control")]
        public static void OpenVersionControl()
        {
            string githubPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\GitHubDesktop\GitHubDesktop.exe";

            if (File.Exists(githubPath))
            {
                Process.Start(githubPath);
            }
        }

        [EditorToolbarButton("cs Script Icon", "Open C# Project")]
        public static void OpenScriptIDE()
        {
            EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        }

        [EditorToolbarButton("Scene", "Open Scene", 0, EditorToolbarPosition.LeftRight, true)]
        public static void OpenScene()
        {
            var a = new GenericMenu();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                int slash = scene.path.LastIndexOf('/');
                string sceneName = slash >= 0 ? scene.path[(slash + 1)..] : scene.path;
                a.AddItem(new GUIContent(sceneName), false, () =>
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        Debug.Log($"Opening scene: {scene.path}");
                        EditorSceneManager.OpenScene(scene.path);
                    }
                });
            }

            string[] levelPaths = Directory.GetFiles("Assets/Scenes", "*.unity", SearchOption.AllDirectories);
            foreach (string scene in levelPaths)
            {
                int slash = scene.LastIndexOf('\\');
                string sceneName = slash >= 0 ? scene[(slash + 1)..] : scene;
                string[] name = sceneName.Split(".");
                sceneName = name[0];
                a.AddItem(new GUIContent(sceneName), false, () =>
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        Debug.Log($"Opening scene: {scene}");
                        EditorSceneManager.OpenScene(scene);
                    }
                });
            }

            a.ShowAsContext();
        }

        [EditorToolbarButton("Animation Icon", "Play From Start", 0, EditorToolbarPosition.RightLeft, true)]
        public static void PlayFromStart()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                _isCustomPlayMode = true;
                EditorApplication.EnterPlaymode();
            }
        }

        [InitializeOnLoadMethod]
        public static void OnEditorStarted()
        {
            EditorApplication.playModeStateChanged += OnPlayModeEntered;
        }

        // private static void OnPlayModeEntered(PlayModeStateChange state)
        // {
        //     if (state == PlayModeStateChange.ExitingEditMode)
        //     {
        //         if (_isCustomPlayMode)
        //         {
        //             EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
        //         }
        //     }
        //     else if (state == PlayModeStateChange.EnteredEditMode)
        //     {
        //         if (_isCustomPlayMode)
        //         {
        //             _isCustomPlayMode = false;
        //             EditorSceneManager.playModeStartScene = null;
        //         }
        //     }
        // }
        private static void OnPlayModeEntered(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:

                    if (_isCustomPlayMode)
                    {
                        EditorSceneManager.playModeStartScene =
                            AssetDatabase.LoadAssetAtPath<SceneAsset>(
                                EditorBuildSettings.scenes[0].path);
                    }
                    else
                    {
                        EditorSceneManager.playModeStartScene = null;
                    }

                    break;

                case PlayModeStateChange.EnteredEditMode:

                    _isCustomPlayMode = false;
                    EditorSceneManager.playModeStartScene = null;

                    break;
            }
        }
    }
}