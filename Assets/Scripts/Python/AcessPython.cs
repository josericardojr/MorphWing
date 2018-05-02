using System.Collections;
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
            if (!File.Exists(fullFilename))
            {
                print(".py dont exists: " + fullFilename);
            }

            fullFilename += " " + args;
           
            if (!File.Exists(pathPythonEXE))
            {
                print("Python.exe dont exists: " + pathPythonEXE);
            }

            print("fullFilename: " + fullFilename);
            //print("pathPythonEXE: " + pathPythonEXE);

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
        file = xmlName + ".xml";
        string directory = Directory.GetCurrentDirectory() + @"\Assests\";

        instruction = GetInstruction(Directory.GetCurrentDirectory() + filePy, "do " + directory + " " + file, PlayerPrefs.GetString(AcessPython.KEYPATHPYTHON));

        print("Result: " + instruction);
    }
}
