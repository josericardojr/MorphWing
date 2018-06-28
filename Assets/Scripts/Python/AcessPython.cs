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

    public static string[] KEYENEMY = { "KEYENEMY1", "KEYENEMY2", "KEYENEMY3", "KEYENEMY4" }, KEYDIFMULTI = { "DIFMULTI1", "DIFMULTI2", "DIFMULTI3", "DIFMULTI4" };

    public static string PLAYERHITRATE = "PLAYERHITRATE";

    [SerializeField]
    private Text text;

    private string file, instruction, filePy = @"\Python\Prov.py";

    private int contVertx;

    private bool run;

    private void Awake()
    {
        run = false;
        MyText = "start";
        contVertx = 0;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            MyText = "";
        }
        else
        {
            MyText = instruction;
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
        string args = GetArgs(xmlName);

        if (!run)
        {
            StartCoroutine(MakeChanges(args, contVertx));
            contVertx = 0;
        }
    }

    private string GetArgs(string xmlName)
    {
        file = Application.dataPath + @"/" + xmlName;
        PlayerPrefs.SetString(AcessPython.KEYFILEXML, file);
        string args = "";

        for (int i = 0; i < KEYENEMY.Length; i++)
        {
            args += KEYENEMY[i] + " ";
        }
        BalanceApplier balance = FindObjectOfType<BalanceApplier>();
        if (balance != null)
        {
            for (int i = 0; i < KEYDIFMULTI.Length; i++)
            {
                args += KEYDIFMULTI[i] + "=" + balance.difficultyMultipliers[i] + " ";
            }
        }

        return args;
    }

    private IEnumerator MakeChanges(string args, int finalCount)
    {
        file = PlayerPrefs.GetString(AcessPython.KEYFILEXML);
        run = true;
        instruction = ("Start MakeChanges: " + file);
        string pyInstruction = "";
        do
        {
            if (!File.Exists(file))
            {
                instruction = (Application.dataPath + " xml dont exists: " + file);
            }
            else
            {
#if UNITY_EDITOR
                var watch = System.Diagnostics.Stopwatch.StartNew();
                // the code that you want to measure comes here
#endif
                pyInstruction = GetInstruction(Directory.GetCurrentDirectory() + filePy, "do " + file + " " + args, PlayerPrefs.GetString(AcessPython.KEYPATHPYTHON));
#if UNITY_EDITOR
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                //print("Time to get Instruction: " + (elapsedMs));

                string nameFile = "LogTime.txt";
                if (File.Exists(nameFile))
                {
                    TextWriter tw = new StreamWriter(Directory.GetCurrentDirectory() + @"\" + nameFile, true);

                    tw.WriteLine(Characters_Player.GetDate() + ";" + elapsedMs + ";" + finalCount);

                    tw.Close();
                }
#endif
                instruction = ("Result: " + pyInstruction);
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

        FindChanges(pyInstruction);

        //print("File = " + file);
        //print("PlayerPrefs = " + PlayerPrefs.GetString(AcessPython.KEYFILEXML));
        //print("________");
        run = false;
    }

    private static void FindChanges(string pyInstruction)
    {
        string[] splitReturn = pyInstruction.Split(new char[] { ';' }), split;
        float valueBalance;
        BalanceApplier balance = FindObjectOfType<BalanceApplier>();
        if (balance)
        {
            #region enemy
            for (int i = 0; i < KEYENEMY.Length; i++)
            {
                for (int j = 0; j < splitReturn.Length; j++)
                {
                    if (splitReturn[j].Contains(KEYENEMY[i]))
                    {
                        split = splitReturn[j].Split(new char[] { ':' });
                        if (split.Length > 1)
                        {
                            try
                            {
                                valueBalance = float.Parse(split[split.Length - 1]);
                                print(i + " " + valueBalance.ToString());
                                balance.ApplyDifficulty(i, valueBalance);
                                //print("__________");
                                //print(split[j] + " find -> " + valueBalance);
                            }
                            catch (System.Exception e)
                            {
                                print(e.Message);
                                print(i);
                                //print("Dont find value on:" + splitReturn[j]);
                            }
                        }

                    }
                }
            }
            #endregion

            #region dif multi

            string aux = "";
            for (int i = 0; i < KEYDIFMULTI.Length; i++)
            {
                for (int j = 0; j < splitReturn.Length; j++)
                {
                    if (splitReturn[j].Contains(KEYDIFMULTI[i]))
                    {

                        split = splitReturn[j].Split(new char[] { ':' });
                        if (split.Length > 1)
                        {
                            try
                            {
                                aux = split[split.Length - 1];
                                valueBalance = float.Parse(aux);
                                balance.SetItemDistances(i, valueBalance);
                                //print("__________");
                                //print(split[0] + " find -> " + valueBalance);
                            }
                            catch (System.Exception e)
                            {
                                print(e.Message);
                                print(i);
                                //print("Dont find value on:" + splitReturn[j]);
                            }
                        }

                    }
                }
            }
            #endregion


            for (int j = 0; j < splitReturn.Length; j++)
            {
                if (splitReturn[j].Contains(PLAYERHITRATE))
                {

                    split = splitReturn[j].Split(new char[] { ':' });

                    if (split.Length > 1)
                    {
                        try
                        {
                            valueBalance = float.Parse(split[split.Length - 1]);
                            balance.ModifyDamage(valueBalance);

                            //print("__________");
                            //print(split[0] + " find -> " + valueBalance);
                        }
                        catch (System.Exception e)
                        {
                            print(e.Message);
                            print("Dont find value on:" + split[split.Length - 1]);
                        }
                    }

                }
            }

            if (!balance.DontApplyBalance)
            {
                FindObjectOfType<Managers_WriteText>().WriteResults();
            }
        }

    }
    

    public string MyText
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

    public void AddContVertx()
    {
        contVertx++;
    }
}
