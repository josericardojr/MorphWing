using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BingAcess : MonoBehaviour
{
    private const string BINGTOOLPATH = "BinGTool";
    private const string DATABINGTOOLPATH = "Data.py";

    void Start()
    {

        Acess();
    }

    private void Acess()
    {
        string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, BINGTOOLPATH);
        path = Path.Combine(path, DATABINGTOOLPATH);

        string bingReturn = AccessPython.Instance.GetChanges("Test");

        print(bingReturn);
    }
}
