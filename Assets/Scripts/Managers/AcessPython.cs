using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class AcessPython : MonoBehaviour
{
    void Start()
    {
        print("ue");
        print(GetInstruction(Directory.GetCurrentDirectory() + @"\print.py", "HelloWorld1 HelloWorld2 HelloWorld3", @"C:\Users\nasci\AppData\Local\Programs\Python\Python36-32\python.exe"));        
    }

    /// <summary>
    /// Retorna uma string com todos os prints do arquivo .py
    /// </summary>
    /// <param name="fullFilename">caminho para onde o arquivo python está, Ex:...\print.py </param>
    /// <param name="args">argumentos para serem passados para o arquivo python ao ser executado</param>
    /// <param name="pathPythonEXE">caminho para onde o executável python está, Ex:C:\Program Files\Python36\python.exe </param>
    /// <returns></returns>
    public string GetInstruction(string fullFilename, string args, string pathPythonEXE)
    {

        //Directory.GetCurrentDirectory() + @"\print.py HelloWorld 2 3";
        //@"C:\Program Files\Python36\python.exe"
        //print(fullFilename);
        //print(pathPythonEXE);
        fullFilename += " " + args;
        //print(fullFilename);
        Process p = new Process();
        p.StartInfo = new ProcessStartInfo(pathPythonEXE, fullFilename)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        p.Start();

        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        return (output);
    }
}
