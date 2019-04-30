using UnityEngine;
using Bing;

public class AccessPython : MonoBehaviour
{
    public static AccessPython Instance { get; private set; }

    private ProcessorManager processorManager;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            processorManager = new ProcessorManager(PlayerPrefs.GetString(ProcessorManager.KEYPATHPYTHON));
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        try
        {
            processorManager.Process.Kill();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (processorManager != null)
        {
            GUILayout.Box(processorManager.AllOutputPython); 
        }
    }
#endif

    public string GetChanges(string instructions)
    {
        return processorManager.GetInfo(instructions);
    }

    public bool Ready
    {
        get
        {
            if (processorManager != null)
            {
                return processorManager.Ready; 
            }
            return false;
        }
    }
}
