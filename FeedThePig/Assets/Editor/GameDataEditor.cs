using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameData))]
public class GameDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameData data = (GameData)target;
        if(GUILayout.Button("Reset Data"))
        {
            data.ResetData();
            SaveController.DeleteSaveFile();
        }
    }
}
