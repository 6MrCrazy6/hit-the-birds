using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SettingsData
{
    public List<Data> options = new List<Data>();
    public SettingsData(SaveLoadSettingsStorage storage)
    {
        options = storage.datas;
    }


    [Serializable]
    public struct Data
    {
        public string displayTitle;
        public bool multiSelectionEnabled;// if true allows multiple values selected
        public string valueType;// "string" or "number"
        public Values[] values;
        public int[] selected;
    }

    [Serializable]
    public struct Values
    {
        public int id; // int (0+) always unique for specific game option
        public string title;// "Value Title",
                            // 'value' could be a string or number (int, float...)
                            // value types should not be mixed for a single option object
                            // game logic should handle this properly,
                            // depending on 'valueType' property of the option object
        public string value;// "actual value"
    }
}
