using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class TreatmentError : MonoBehaviour {
    
    private Canvas canvas;

    [SerializeField]
    private InputField input;

    string fileTest = @"\Python\Check.py";

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TestPython();
            } 
        }
    }

    private void TestPython()
    {
        if (!File.Exists(Directory.GetCurrentDirectory() + fileTest))
        {
            print("File dont Exists: " + Directory.GetCurrentDirectory() + fileTest);
        }

        string msg = TestePath();
        print(msg); 

        if (!msg.Contains("OK"))
        {
            canvas.enabled = true;
            input.text = msg;
        }
    }

    private string TestePath()
    {
        return (AcessPython.Instance.GetInstruction(Directory.GetCurrentDirectory() + fileTest, "Test", PlayerPrefs.GetString(AcessPython.KEYPATHPYTHON)));
    }

    private string GetPythonPath()
    {
        string pathPython = "did not find";
        IDictionary environmentVariables = Environment.GetEnvironmentVariables();
        string pathVariable = environmentVariables["Path"] as string;
        if (pathVariable != null)
        {
            string[] allPaths = pathVariable.Split(';');
            foreach (var path in allPaths)
            {
                string pythonPathFromEnv = path + @"python.exe";
                //& !pythonPathFromEnv.Contains("Python2")
                if (File.Exists(pythonPathFromEnv) )
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

    public void UpdatePath()
    {
        PlayerPrefs.SetString(AcessPython.KEYPATHPYTHON, input.text);
        Start();
    }

    public void AutomaticPath()
    {
        input.text = GetPythonPath();
    }
}
