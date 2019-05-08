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
        private GUIStyle guiStyle;

        private bool showPythonOptions;
        private bool showBingOptions;

        public static string KEY_PYTHON_PATH = "KEY_PATH_PYTHON";
        public static string KEY_BING_PATH = "KEY_PATH_BING";

        private XmlDocument bingXmlDocument;

        private string tempBingPath;
        private string tempPythonPath;
        private string tempSchemaPath;

        private bool changeConfigXml;

        [MenuItem("Bing/BingConfig")]
        public static void ShowWindow()
        {
            GetWindow<BingConfig>("BingConfig");
            
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
            GetWindow<BingConfig>().minSize = new Vector2(350, 400);
            //GetWindow<BingConfig>().maxSize = new Vector2(350, 400);

            GUILayout.BeginHorizontal();

            //Buttons Category
            if (GUILayout.Button("Python Options", GUILayout.Height(40)))
            {
                showPythonOptions = true;
                showBingOptions = false;
            }
            if (GUILayout.Button("Bing Options", GUILayout.Height(40)))
            {
                showPythonOptions = false;
                showBingOptions = true;
            }

            GUILayout.EndHorizontal();

            if (showPythonOptions)
                PythonSetup();
            if (showBingOptions)
            {
                BingSetup();
                SetupBingConfig();
            } 
        }

        private void DrawLabel(string text, int size, Color color, FontStyle fontStyle)
        {
            guiStyle = new GUIStyle();
            guiStyle.fontSize = size;
            guiStyle.normal.textColor = color;
            guiStyle.fontStyle = fontStyle;
            GUILayout.Label(text, guiStyle);
        }

        private void SetupBingConfig()
        {
            if (Directory.Exists(BingPath))
            {
                if (bingXmlDocument == null)
                {
                    if (changeConfigXml == false)
                    {

                        GUILayout.Space(15);

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();

                        DrawLabel("config.xml do not exists", 13, Color.red, FontStyle.Bold);

                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();

                        GUILayout.Space(5);

                        if (GUILayout.Button("Create new config.xml", GUILayout.Height(30)))
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
                    GUILayout.Space(15);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();

                    DrawLabel("config.xml exists", 13, Color.blue, FontStyle.Bold);

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.Space(5);

                    GUILayout.BeginHorizontal();
                    //GUILayout.Box(HelperXml.PrettyXml(bingXmlDocument.OuterXml));
                    GUILayout.EndHorizontal();

                    if (changeConfigXml == false)
                    {
                        if (GUILayout.Button("Update new config.xml", GUILayout.Height(30)))
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
            DrawLabel("Please intert the schema's path", 13, Color.red, FontStyle.Bold);
            tempSchemaPath = GUILayout.TextArea(tempSchemaPath);

            if (GUILayout.Button("Save new config.xml", GUILayout.Height(30)))
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
                GUILayout.Space(15);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                DrawLabel("Bing Path is setup", 13, Color.blue, FontStyle.Bold);
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Update Bing Path", GUILayout.Height(30)))
                {
                    tempBingPath = BingPath;
                    BingPath = "";
                }
            }
            else
            {
                GUILayout.Space(15);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                DrawLabel("Bing Path is not setup", 13, Color.red, FontStyle.Bold);

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                // GUILayout.FlexibleSpace();
                tempBingPath = GUILayout.TextField(tempBingPath, GUILayout.Width(Screen.width/2));

                if (GUILayout.Button("Try find Bing Path",GUILayout.Height(20)))
                {
                    tempBingPath = TryGetStandardBingPath();
                }
                //GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                if (GUILayout.Button("Save Bing Path", GUILayout.Height(30)))
                {
                    BingPath = tempBingPath;
                }
                
            }
        }

        private void PythonSetup()
        {
            if (File.Exists(PythonPath))
            {
                GUILayout.Space(15);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                DrawLabel("Python Path is setup", 13, Color.blue, FontStyle.Bold);

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Update Python Path", GUILayout.Height(30)))
                {
                    tempPythonPath = PythonPath;
                    PythonPath = "";
                }
            }
            else
            {
                GUILayout.Space(15);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                DrawLabel("Python Path is not setup", 13, Color.red, FontStyle.Bold);

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                GUILayout.BeginHorizontal();

                tempPythonPath = GUILayout.TextField(tempPythonPath, GUILayout.Width(Screen.width / 2));

                if (GUILayout.Button("Try find Python Path", GUILayout.Height(20)))
                {
                    tempPythonPath = TryGetStandardPythonPath();
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Save Python Path", GUILayout.Height(30)))
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