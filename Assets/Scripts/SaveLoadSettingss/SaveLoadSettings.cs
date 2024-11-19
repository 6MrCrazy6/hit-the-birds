
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
//using Newtonsoft;
using System.Data;

public class SaveLoadSettings : MonoBehaviour
{
    [SerializeField] private SaveLoadSettingsStorage storage;
    [HideInInspector] public SettingsData Data;
    string fileName = "settings.json";

    public void LoadData()
    {
        string gameFolder = Path.GetDirectoryName(Application.dataPath);
        string filePath = Path.Combine(gameFolder, fileName);
        byte[] bytes = new byte[1024];
        try
        {
            bytes = File.ReadAllBytes(filePath);
        }
        catch (Exception)
        {
            SaveToGameFolder();
            Debug.Log("File config.json not found, new file created");
            return;
            throw;
        }


        var ppData = Encoding.UTF8.GetString(bytes);

        if (string.IsNullOrEmpty(ppData))
        {
            SaveToGameFolder();
        }
        else
        {
            Data = JsonUtility.FromJson<SettingsData>(ppData);
        }
        storage.datas = Data.options;
        //Debug.Log("LOAD TEXT:  " + Data.datas[0].valueName);
    }


    public void SaveToGameFolder()
    {
        Data = new SettingsData(storage);
        string gameFolder = Path.GetDirectoryName(Application.dataPath);
        string filePath = Path.Combine(gameFolder, fileName);

        var json = JsonUtility.ToJson(Data);
        File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(json));
    }

    //TODO 	multiSelection!!
    public List<int> getDatasIntById(int id)
    {
        List<int> result = new List<int>();
        id = Mathf.Clamp(id, 0, Data.options[id].values.Length);
        foreach(var item in Data.options[id].selected)
        {
            result.Add(int.Parse(Data.options[id].values[item].value));
        }
        return result;
    }
    public int getDataIntById(int id)
    {
        int result = 0;
        id = Mathf.Clamp(id, 0, Data.options[id].values.Length);
        return int.Parse(Data.options[id].values[Data.options[id].selected[0]].value);
    }

    public string getDataStringById(int id)
    {
        id = Mathf.Clamp(id, 0, Data.options[id].values.Length);
        return Data.options[id].values[Data.options[id].selected[0]].value;
    }


    public string getRandomImage(int idColum)
    {
        int idImage = 0;
        int prevImage = 0;
        if (PlayerPrefs.HasKey("PrevImage"))
        {
            prevImage = PlayerPrefs.GetInt("PrevImage");
            idImage = UnityEngine.Random.Range(0, Data.options[idColum].values.Length);
            Debug.Log("prevImage " + prevImage + " idImage " + idImage);
            for (int i = 0; i < 5; i++)
            {
                if (idImage == prevImage)
                {
                    idImage = UnityEngine.Random.Range(0, Data.options[idColum].values.Length);
                }
                else break;
            }
        }
        else
        {
            idImage = UnityEngine.Random.Range(0, Data.options[idColum].values.Length);
        }
        PlayerPrefs.SetInt("PrevImage", idImage);
        //idColum = Mathf.Clamp(idColum, 0, Data.datas[idColum].variantsName.Length);
        //return Data.options[idColum].variantsName[idImage];
        Debug.Log("LOAD IMAGE ID " + Data.options[idColum].values[idImage].value);
        return Data.options[idColum].values[idImage].value;
    }
}
