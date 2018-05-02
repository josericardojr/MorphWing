using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AcessPython : MonoBehaviour
{
    public static string KEYPATHPYTHON = "KEYPATHPYTHON", KEYFILEXML = "KEYFILEXML";

    [SerializeField]
    private Text text;

    private string file, instruction, filePy = @"\Python\Prov.py";

    private bool run;

    private void Start()
    {
        run = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                myText = instruction;
            }
            else
            {
                myText = "";
            }
        }
        else
        {
            myText = "";
        }
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

            //print("fullFilename: " + fullFilename);
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
        xmlName += ".xml";
        file = Application.dataPath + @"\" + xmlName;
        PlayerPrefs.SetString(AcessPython.KEYFILEXML, file);

        if (!run)
        {
            StartCoroutine(MakeChanges());
        }
    }

    private IEnumerator MakeChanges()
    {
        file = PlayerPrefs.GetString(AcessPython.KEYFILEXML);
        run = true;
        instruction = ("Start MakeChanges: " + file);
        do
        {
            if (!File.Exists(file))
            {
                instruction = (Application.dataPath + " xml dont exists: " + file);
            }
            else
            {
                instruction = GetInstruction(Directory.GetCurrentDirectory() + filePy, "do " + file, PlayerPrefs.GetString(AcessPython.KEYPATHPYTHON));

                instruction = ("Result: " + instruction);

                if (file == PlayerPrefs.GetString(AcessPython.KEYFILEXML))
                {
                    run = false;
                }
                else
                {
                    file = PlayerPrefs.GetString(AcessPython.KEYFILEXML);
                }
            }
            yield return new WaitForFixedUpdate();
        } while (run);
        instruction += ("Over MakeChanges");
        //print("File = " + file);
        //print("PlayerPrefs = " + PlayerPrefs.GetString(AcessPython.KEYFILEXML));
        //print("________");
        run = false;        
    }

    public string myText
    {
        get
        {
            return text.text;
        }

        set
        {
            if (text != null)
            {
                text.text = value;
            }
        }
    }

}
