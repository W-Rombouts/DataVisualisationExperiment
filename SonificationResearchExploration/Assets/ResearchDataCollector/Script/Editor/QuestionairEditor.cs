using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class QuestionairEditor : EditorWindow
{
    Editor editor;
    [SerializeField] Questionnaire questionnaire = new Questionnaire();


    [MenuItem("Window/Questionnaire")]
    public static void ShowWindow()
    {
        System.IO.Directory.CreateDirectory("Assets/ResearchDataCollector/Questionnaires");
        GetWindow<QuestionairEditor>("Questionnaire",true);
    }
    
    private void OnGUI()
    {
        if (!editor) { editor = Editor.CreateEditor(this); }
        if (editor) { editor.OnInspectorGUI(); }
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            File.WriteAllText("Assets/ResearchDataCollector/Questionnaires/" + questionnaire.Title + ".json", JsonConvert.SerializeObject(questionnaire));
        }
        if (GUILayout.Button("Reset"))
        {
            questionnaire = new Questionnaire();
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
        var list = serializedObject.FindProperty("questionnaire");
        EditorGUILayout.PropertyField(list, new GUIContent("Questionnaire"), true);
        serializedObject.ApplyModifiedProperties();
    }
}