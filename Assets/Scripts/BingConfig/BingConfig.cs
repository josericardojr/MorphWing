using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Bing
{
    public class BingConfig : EditorWindow
    {
        public static string KEY_PYTHON_PATH = "KEY_PATH_PYTHON";
        public static string KEY_BING_PATH = "KEY_PATH_BING";

        private string temp_bing_path;
        private string temp_python_path;

        [MenuItem("Bing/BingConfig")]
        static void Init()
        {
            BingConfig window = (BingConfig)EditorWindow.GetWindow(typeof(BingConfig));
        }

        private void Awake()
        {
            temp_bing_path = GetBingPath();
            temp_python_path = GetPythonPath();
        }

        private void OnGUI()
        {
            if (File.Exists(GetPythonPath()))
            {
                GUILayout.Label("Python Path is setup");

                if (GUILayout.Button("Update Python Path"))
                {
                    SetPythonPath("");
                }
            }
            else
            {
                GUILayout.Label("Python Path is not setup");

                GUILayout.BeginHorizontal();

                temp_python_path = GUILayout.TextArea(temp_python_path);
                if (GUILayout.Button("Try find Python Path"))
                {
                    temp_python_path = TryGetStandardPythonPath();
                }

                GUILayout.EndHorizontal();

                if (GUILayout.Button("Save Python Path"))
                {
                    SetPythonPath(temp_python_path);
                }
            }

            if (Directory.Exists(GetBingPath()))
            {
                GUILayout.Label("Bing Path is setup");

                if (GUILayout.Button("Update Python Path"))
                {
                    SetBingPath("");
                }
            }
            else
            {
                GUILayout.Label("Bing Path is not setup"); GUILayout.BeginHorizontal();

                temp_bing_path = GUILayout.TextArea(temp_bing_path);

                if (GUILayout.Button("Try find Bing Path"))
                {
                    temp_bing_path = TryGetStandardBingPath();
                }

                GUILayout.EndHorizontal();

                if (GUILayout.Button("Save Bing Path"))
                {
                    SetBingPath(temp_bing_path);
                }
            }
        }

        public static string GetPythonPath()
        {
            return PlayerPrefs.GetString(KEY_PYTHON_PATH);
        }

        public static void SetPythonPath(string path)
        {
            PlayerPrefs.SetString(KEY_PYTHON_PATH, path);
        }

        public static string GetBingPath()
        {
            return PlayerPrefs.GetString(KEY_BING_PATH);
        }

        public static void SetBingPath(string path)
        {
            PlayerPrefs.SetString(KEY_BING_PATH, path);
        }

        public static string TryGetStandardPythonPath()
        {
            string pathPython = "could not find automatically the path";
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            string pathVariable = environmentVariables["Path"] as string;
            if (pathVariable != null)
            {
                string[] allPaths = pathVariable.Split(';');
                foreach (var path in allPaths)
                {
                    string pythonPathFromEnv = path + @"python.exe";
                    //& !pythonPathFromEnv.Contains("Python2")
                    if (File.Exists(pythonPathFromEnv))
                    {
                        //print("Change: ");
                        pathPython = pythonPathFromEnv;
                        //print("Path: " + pathPython);
                    }
                }
            }
            // 
            //print("___________________");
            //print("Final: " + pathPython);
            return pathPython;
        }

        private string TryGetStandardBingPath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string path_root = Directory.GetParent(currentDirectory).FullName;

            return path_root + @"\BinGTool";
            //string filePyName = "Data.py";
            //return path_bing + @"\" + filePyName;
        }
    }
}