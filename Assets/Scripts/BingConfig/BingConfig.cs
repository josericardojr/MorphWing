using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;

namespace Bing
{
    public class BingConfig : EditorWindow
    {
        public static string KEY_PYTHON_PATH = "KEY_PATH_PYTHON";
        public static string KEY_BING_PATH = "KEY_PATH_BING";

        private XmlDocument bingXmlDocument;

        private string tempBingPath;
        private string tempPythonPath;
        private string tempSchemaPath;

        private bool changeConfigXml;

        [MenuItem("Bing/BingConfig")]
        static void Init()
        {
            BingConfig window = (BingConfig)EditorWindow.GetWindow(typeof(BingConfig));
        }

        private void Awake()
        {
            bingXmlDocument = null;

            tempBingPath = BingPath;
            tempPythonPath = PythonPath;

            changeConfigXml = false;



            if (File.Exists(ConfigXmlPath))
            {
                bingXmlDocument = new XmlDocument();
                bingXmlDocument.Load(ConfigXmlPath);
            }
        }

        private void OnGUI()
        {
            PythonSetup();
            BingSetup();
            SetupBingConfig();
        }

        private void SetupBingConfig()
        {
            if (Directory.Exists(BingPath))
            {
                if (bingXmlDocument == null)
                {
                    if (changeConfigXml == false)
                    {
                        GUILayout.Label("config.xml do not exists");

                        if (GUILayout.Button("Create new config.xml"))
                        {
                            changeConfigXml = true;
                            tempSchemaPath = Path.Combine(BingPath, "schema.xml");
                        }
                    }
                    else
                    {
                        SetupConfig();
                    }
                }
                else
                {
                    GUILayout.Label("config.xml exists");

                    GUILayout.BeginHorizontal();
                    GUILayout.Box(HelperXml.PrettyXml(bingXmlDocument.OuterXml));
                    GUILayout.EndHorizontal();

                    if (changeConfigXml == false)
                    {
                        if (GUILayout.Button("Update new config.xml"))
                        {
                            changeConfigXml = true;
                            tempSchemaPath = Path.Combine(BingPath, "schema.xml");
                        }
                    }
                    else
                    {
                        SetupConfig();
                    }
                }
            }
        }

        private void SetupConfig()
        {
            GUILayout.Label("please intert the schema's path");
            tempSchemaPath = GUILayout.TextArea(tempSchemaPath);


            if (GUILayout.Button("Save new config.xml"))
            {
                CreateNewConfig();
                bingXmlDocument.Save(ConfigXmlPath);
                changeConfigXml = false;
            }
        }

        private void CreateNewConfig()
        {
            bingXmlDocument = new XmlDocument();

            XmlDeclaration xmlDeclaration = bingXmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = bingXmlDocument.DocumentElement;
            bingXmlDocument.InsertBefore(xmlDeclaration, root);
            XmlElement configXml = bingXmlDocument.CreateElement(HelperXml.KEY_XML);

            HelperXml.SetText(HelperXml.KEY_XML_SCHEMA_PATH, tempSchemaPath, ref configXml, ref bingXmlDocument);

            bingXmlDocument.AppendChild(configXml);
        }

        private void BingSetup()
        {
            if (Directory.Exists(BingPath))
            {
                GUILayout.Label("Bing Path is setup");

                if (GUILayout.Button("Update Python Path"))
                {
                    tempBingPath = BingPath;
                    BingPath = "";
                }
            }
            else
            {
                GUILayout.Label("Bing Path is not setup");

                GUILayout.BeginHorizontal();

                tempBingPath = GUILayout.TextArea(tempBingPath);

                if (GUILayout.Button("Try find Bing Path"))
                {
                    tempBingPath = TryGetStandardBingPath();
                }

                GUILayout.EndHorizontal();

                if (GUILayout.Button("Save Bing Path"))
                {
                    BingPath = tempBingPath;
                }
            }
        }

        private void PythonSetup()
        {
            if (File.Exists(PythonPath))
            {
                GUILayout.Label("Python Path is setup");

                if (GUILayout.Button("Update Python Path"))
                {
                    tempPythonPath = PythonPath;
                    PythonPath = "";
                }
            }
            else
            {
                GUILayout.Label("Python Path is not setup");

                GUILayout.BeginHorizontal();

                tempPythonPath = GUILayout.TextArea(tempPythonPath);
                if (GUILayout.Button("Try find Python Path"))
                {
                    tempPythonPath = TryGetStandardPythonPath();
                }

                GUILayout.EndHorizontal();

                if (GUILayout.Button("Save Python Path"))
                {
                    PythonPath = tempPythonPath;
                }
            }
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

        public static string PythonPath
        {
            get
            {
                return PlayerPrefs.GetString(KEY_PYTHON_PATH);
            }

            set
            {
                PlayerPrefs.SetString(KEY_PYTHON_PATH, value);
            }
        }

        public static string BingPath
        {
            get
            {
                return PlayerPrefs.GetString(KEY_BING_PATH);
            }
            set
            {
                PlayerPrefs.SetString(KEY_BING_PATH, value);
            }
        }

        public string ConfigXmlPath
        {
            get
            {
                return Path.Combine(BingPath, "config.xml");
            }
        }

        class HelperXml
        {
            public const string KEY_XML = "config";
            public const string KEY_XML_SCHEMA_PATH = "schema_path";

            public static XmlElement SetText(string keyName, string text, ref XmlElement father, ref XmlDocument doc)
            {
                XmlElement element = doc.CreateElement(keyName);
                element.InnerText = text;
                father.AppendChild(element);
                return element;
            }

            public static string PrettyXml(string xml)
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = false;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return stringBuilder.ToString();
            }
        }
    }
}