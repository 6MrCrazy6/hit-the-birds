#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveLoadSettingsStorage)), CanEditMultipleObjects]
public class SettingsEditor : Editor
{
    SerializedProperty _phase;
    public override void OnInspectorGUI()
    {
        SaveLoadSettingsStorage save = (SaveLoadSettingsStorage)target;

        DrawDefaultInspector();

        GUILayout.Space(20);

        if (GUILayout.Button("SAVE"))
        {
            var sls = FindObjectOfType<SaveLoadSettings>();
            if (sls==null)
            {
                Debug.LogError("Please add a SaveLoadSettings on scene!");
            }
            else
            {
                sls.SaveToGameFolder();
            }
        }
     
     

        Undo.RecordObject(target, "Change");
    }

}
#endif