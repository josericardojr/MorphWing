﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class AcessPython : MonoBehaviour
{
    public static string KEYPATHPYTHON = "KEYPATHPYTHON";

    private string file, instruction, filePy = @"\Python\Prov.py";

    /// <summary>
    /// Retorna uma string com todos os prints do arquivo .py
    /// </summary>
    /// <param name="fullFilename">caminho para onde o arquivo python está, Ex:...\print.py </param>
    /// <param name="args">argumentos para serem passados para o arquivo python ao ser executado</param>
    /// <param name="pathPythonEXE">caminho para onde o executável python está, Ex:C:\Program Files\Python36\python.exe </param>
    /// <returns></returns>
    public string GetInstruction(string fullFilename, string args, string pathPythonEXE)
    {
        try
        {
            fullFilename += " " + args;
         
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
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public void GetChanges(string xmlName)
    {
        file = xmlName;

        instruction = GetInstruction(Directory.GetCurrentDirectory() + filePy, "do " + xmlName, PlayerPrefs.GetString(AcessPython.KEYPATHPYTHON));

        print("Result: " + instruction);
    }
}