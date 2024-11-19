using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SaveLoadSettingsStorage", fileName = "SaveLoadSettingsStorage")]
public class SaveLoadSettingsStorage : ScriptableObject
{
    public List<SettingsData.Data> datas = new List<SettingsData.Data>();

    private void OnValidate()
    {
        int id = 0;
        foreach(var data in datas)
        {
            id = 0;
            for (int i=0;i< data.values.Length;i++)
            {
                SettingsData.Values v = data.values[i];
                v.id = id;
                data.values[i] =  v;
                id++;
            }
        }
    }
}
