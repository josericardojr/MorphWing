using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System;

namespace Bing
{
    public class ProcessorManager
    {
        public static string KEYPATHPYTHON = "KEYPATHPYTHON";
        public static string KEY_PATH_READY = "KEY_PATH_READY";
        private const string KEY_PATH_PROV = "path_prov", KEY_PATH_SCHEMA = "path_schema";
        private const string KEY_TEST_PYTHON = "key_test_python";

        private int count;

        public bool Ready { get; private set; }

        public Process Process { get; private set; }

        private StreamWriter myStreamWriter;

        public string LastOutputPython { get; private set; }
        public string AllOutputPython { get; private set; }

        public ProcessorManager(string pathPythonEXE)
        {
            count = 0;
            Ready = false;
            LastOutputPython = "";
            AllOutputPython = "";
            Thread t = new Thread(() => SetupProcessor(pathPythonEXE));
            t.Start();
        }

        private void SetupProcessor(string pathPythonEXE)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string path_root = Directory.GetParent(currentDirectory).FullName;

            string path_bing = path_root + @"\BinGTool";

            string filePyName = "Data.py";
            string pathPy = path_bing + @"\" + filePyName;

            try
            {
                if (!File.Exists(pathPy))
                {
                    UnityEngine.Debug.Log(".py dont exists: " + pathPy);
                }
                else
                {
                    if (!File.Exists(pathPythonEXE))
                    {
                        UnityEngine.Debug.Log("Python.exe dont exists: " + pathPythonEXE);
                    }

                    //print("fullFilename: " + pathPy);
                    //print("pathPythonEXE: " + pathPythonEXE);

                    string args = SetupArg(KEY_PATH_PROV, currentDirectory + @"\Assets\info.xml");
                    args += SetupArg(KEY_PATH_SCHEMA, path_bing + @"\schema.xml");

                    pathPy += " " + args;

                    Process = new Process
                    {
                        EnableRaisingEvents = true,
                        StartInfo = new ProcessStartInfo(pathPythonEXE, pathPy)
                        {
                            RedirectStandardInput = true,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };

                    Process.OutputDataReceived += Process_OutputDataReceived;
                    Process.ErrorDataReceived += Process_ErrorDataReceived;

                    Process.Start();

                    myStreamWriter = Process.StandardInput;
                    Process.BeginOutputReadLine();
                    Process.BeginErrorReadLine();

                    //string output = p.StandardOutput.ReadToEnd();

                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }
        }

        public string GetInfo(string instructions)
        {
            LastOutputPython = string.Empty;
            SendMessagePython(instructions);

#if UNITY_EDITOR
            int safe = DateTime.Now.Second;
#endif
            while (string.IsNullOrEmpty(LastOutputPython))
            {
#if UNITY_EDITOR
                if (Math.Abs(DateTime.Now.Second - safe) > 3)
                {
                    UnityEngine.Debug.Log(LastOutputPython);
                    LastOutputPython = "safe < 0";
                }
#endif
            }

            return LastOutputPython;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!System.String.IsNullOrEmpty(e.Data))
            {
                //UnityEngine.Debug.Log("Received: " + e.Data);
                if (e.Data.Contains(KEY_TEST_PYTHON))
                {
                    System.Random r = new System.Random();
                    count++;
                    if (count > 2)
                    {
                        count = 0;
                    }
                    else
                    {
                        SendMessagePython(KEY_TEST_PYTHON);
                    }
                }
                else if (e.Data.Contains(KEY_PATH_READY))
                {
                    Ready = true;
                }
                else
                {
                    LastOutputPython = e.Data;
                    AllOutputPython = LastOutputPython + "\n" + AllOutputPython;
                }

            }
        }

        public void SendMessagePython(string s)
        {
            Thread t = new Thread(() => myStreamWriter.WriteLine(s));
            t.Start();
            //UnityEngine.Debug.Log("Sended: " + s);
        }

        public bool CheckSendMessagePython(string s)
        {
            SendMessagePython(s);
            return true;
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!System.String.IsNullOrEmpty(e.Data))
            {
                LastOutputPython += "ERROR: " + e.Data + "\n";
            }
        }

        private string SetupArg(string key, string arg)
        {
            return " " + key + ";" + arg;
        }

        public static string GetPythonPath()
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
    }
}
