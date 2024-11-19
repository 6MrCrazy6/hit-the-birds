using System.IO;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private SaveLoadSettings loadSettings;

    private void Awake()
    {
        loadSettings = FindObjectOfType<SaveLoadSettings>();

        CreateReadMeFile();
        loadSettings.LoadData();
    }

    private void CreateReadMeFile()
    {
        string readMePath = Path.Combine(Application.dataPath, "../README.txt");

        if (!File.Exists(readMePath))
        {
            string content = "To change the time, modify the value in the \"values\" line, line of the \"settings.json\" file.";
            File.WriteAllText(readMePath, content);
        }
    }
}
