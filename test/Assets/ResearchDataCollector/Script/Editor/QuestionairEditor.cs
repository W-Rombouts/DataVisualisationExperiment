using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class QuestionairEditor : EditorWindow
{
    Editor editor;
    [SerializeField] Questionair questionair = new Questionair();


    [MenuItem("Window/Questionair")]
    public static void ShowWindow()
    {
        System.IO.Directory.CreateDirectory("Assets/ResearchDataCollector/Questionairs");
        GetWindow<QuestionairEditor>("Questionair",true);
    }
    
    private void OnGUI()
    {
        if (!editor) { editor = Editor.CreateEditor(this); }
        if (editor) { editor.OnInspectorGUI(); }
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            File.WriteAllText("Assets/ResearchDataCollector/Questionairs/" + questionair.Title + ".json", JsonConvert.SerializeObject(questionair));
        }
        if (GUILayout.Button("Reset"))
        {
            questionair = new Questionair();
        }
    }
    void OnInspectorUpdate() 
    {
       Repaint(); 
    }
}

[CustomEditor(typeof(QuestionairEditor), true)]
public class QuestionBlockEditorDrawer : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var list = serializedObject.FindProperty("questionair");
        EditorGUILayout.PropertyField(list, new GUIContent("Questionair"), true);
        serializedObject.ApplyModifiedProperties();
    }
}