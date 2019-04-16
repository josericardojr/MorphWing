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
    public static AcessPython Instance { get; private set; }

    private Process process;

    private const string KEY_PATH_PROV = "path_prov", KEY_PATH_SCHEMA= "path_schema";

    public static string KEYPATHPYTHON = "KEYPATHPYTHON", KEYFILEXML = "KEYFILEXML";

    public static string[] KEYENEMY = { "KEYENEMY1", "KEYENEMY2", "KEYENEMY3", "KEYENEMY4" }, KEYDIFMULTI = { "DIFMULTI1", "DIFMULTI2", "DIFMULTI3", "DIFMULTI4" };

    public static string PLAYERHITRATE = "PLAYERHITRATE";

    [SerializeField]
    private Text text;

    private string lastOutputPython;
    private string file;
    private string instruction;
    private string filePy = @"\Python\Prov.py";

    private int contVertx;

    private bool run;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            run = false;
            MyText = "start";
            contVertx = 0;

            string currentDirectory = Directory.GetCurrentDirectory();
            string path_root = Directory.GetParent(currentDirectory).FullName;

            string path_bing = path_root + @"\BinGTool";

            string filePyName = "Data.py";
            string pathPy = path_bing + @"\" + filePyName;
            lastOutputPython = "";

            try
            {
                if (!File.Exists(pathPy))
                {
                    print(".py dont exists: " + pathPy);
                }
                else
                {
                    string pathPythonEXE = PlayerPrefs.GetString(AcessPython.KEYPATHPYTHON);
                    if (!File.Exists(pathPythonEXE))
                    {
                        print("Python.exe dont exists: " + pathPythonEXE);
                    }

                    //print("fullFilename: " + pathPy);
                    //print("pathPythonEXE: " + pathPythonEXE);

                    string args = SetupArg(KEY_PATH_PROV, currentDirectory + @"\Assets\info.xml");
                    args += SetupArg(KEY_PATH_SCHEMA, path_bing + @"\schema.xml");

                    pathPy += " " + args;

                    process = new Process
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

                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.ErrorDataReceived += Process_ErrorDataReceived;

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit(-1);

                    //string output = p.StandardOutput.ReadToEnd();

                }
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }
        else
        {
            Instance.text = text;
            Destroy(this);
        }
    }

    private string SetupArg(string key, string arg)
    {
        return " " + key + ";" + arg;
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

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.Data))
        {
            lastOutputPython += e.Data + "\n"; 
        }
    }

    private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.Data))
        {
            lastOutputPython += "ERROR: " + e.Data + "\n"; 
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Box(lastOutputPython);
    } 
#endif

    /// <summary>
    /// Retorna uma string com todos os prints do arquivo .py
    /// </summary>
    /// <param name="fullFilename">caminho para onde o arquivo python está, Ex:...\print.py </param>
    /// <param name="args">argumentos para serem passados para o arquivo python ao ser executado</param>
    /// <param name="pathPythonEXE">caminho para onde o executável python está, Ex:C:\Program Files\Python36\python.exe </param>
    /// <returns></returns>
    public string GetInstruction(string fullFilename, string args, string pathPythonEXE)
    {
        return lastOutputPython;
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
                args += KEYDIFMULTI[i];// + "=" + balance.difficultyMultipliers[i] + " ";
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
                bool firstTime = false;
                if (!File.Exists(nameFile))
                    firstTime = true;
                TextWriter tw = new StreamWriter(Directory.GetCurrentDirectory() + @"\" + nameFile, true);
                if (firstTime)
                      tw.WriteLine("Date; Elapsed Milliseconds; Final Count;");
                tw.WriteLine(Characters_Player.GetDate() + ";" + elapsedMs + ";" + finalCount);

                    tw.Close();
                
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

    private Text Text
    {
        get
        {
            return text;
        }

        set
        {
            text = value;
        }
    }

    public void AddContVertx()
    {
        contVertx++;
    }
}
