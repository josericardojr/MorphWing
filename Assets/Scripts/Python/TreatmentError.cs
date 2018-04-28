using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AcessPython))]
[RequireComponent(typeof(Canvas))]
public class TreatmentError : MonoBehaviour {
    
    private AcessPython acess;
    
    private Canvas canvas;

    [SerializeField]
    private InputField input;

    void Start()
    {
        acess = GetComponent<AcessPython>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

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
        return (acess.GetInstruction(Directory.GetCurrentDirectory() + @"\print.py", "Test", PlayerPrefs.GetString(AcessPython.KEYPATHPYTHON)));
    }

    string GetPythonPath()
    {
        string pathPython = @"C:\Users\nasci\AppData\Local\Programs\Python\Python36\python.exe";
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
        print("Final: " + pathPython);
        return pathPython;
    }

    public void UpdatePath()
    {
        PlayerPrefs.SetString(AcessPython.KEYPATHPYTHON, input.text);
        Start();
    }
}
