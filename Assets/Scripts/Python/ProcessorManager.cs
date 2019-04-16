using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.IO;

public class ProcessorManager
{
    private const string KEY_PATH_PROV = "path_prov", KEY_PATH_SCHEMA = "path_schema";
    private const string KEY_TEST_PYTHON = "key_test_python";

    public Process Process { get; private set; }

    private StreamWriter myStreamWriter;

    public string LastOutputPython { get; private set; }

    public ProcessorManager(string pathPythonEXE)
    {
        LastOutputPython = "";
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

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!System.String.IsNullOrEmpty(e.Data))
        {
            if (e.Data.Contains(KEY_TEST_PYTHON))
            {
                System.Random r = new System.Random();
                Thread t = new Thread(() => SendMessagePython(r.Next(2) < 0.1f ? "continue" : "repeat"));
                t.Start();
            }
            else
            {
                LastOutputPython += e.Data + "\n";
            }

        }
    }

    private void SendMessagePython(string s)
    {
        myStreamWriter.WriteLine(s);
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
}
