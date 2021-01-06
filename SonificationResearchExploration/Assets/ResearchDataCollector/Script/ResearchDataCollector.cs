using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using RenderHeads.Media.AVProMovieCapture;
using Valve.VR;
using UnityEngine.UIElements;

public class ResearchDataCollector : MonoBehaviour
{
    
    public List<GameObject> trackedGameObject = new List<GameObject>();
    public static GameObject cameraRig;
    [Tooltip("The frequency with wich data is recorded in seconds")]
    public float DataRecordingFreq;
    public int subjectID;
    public string researchID;
    private float frequencyCounter = 0f;
    private string filename;
    public bool LocalLogging = false;
    private List<DataContainer> LocalLoggerList = new List<DataContainer>();
    public bool screenCapture = false;
    private CaptureFromCamera Recorder;
    public SysSpec sysSpec;
    #region SINGLETON PATTERN
    public static ResearchDataCollector _instance;

    public static ResearchDataCollector Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ResearchDataCollector>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("ResearchDataCollector");
                    _instance = container.AddComponent<ResearchDataCollector>();
                }
            }

            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        filename = researchID.ToString() + subjectID.ToString();
        cameraRig = GameObject.Find("[CameraRig]");
        Camera HMD = cameraRig.GetComponentInChildren<Camera>();
        GameObject leftController = GameObject.Find("Controller (left)");
        GameObject rightController = GameObject.Find("Controller (right)");
        trackedGameObject.Add(HMD.gameObject);
        trackedGameObject.Add(leftController);
        trackedGameObject.Add(rightController);
        sysSpec = new SysSpec()
        {
            CPU = SystemInfo.processorType,
            GPU = SystemInfo.graphicsDeviceName,
            Ram = SystemInfo.systemMemorySize.ToString()+"Mb",
            OS = SystemInfo.operatingSystem
        };
        Debug.Log(sysSpec.CPU+"/"+sysSpec.GPU+"/" + sysSpec.Ram+"/"+sysSpec.OS);
        Recorder = gameObject.GetComponent<CaptureFromCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        System.IO.Directory.CreateDirectory("Assets/ResearchDataCollector/questionnaire/Answers/");
        if (screenCapture)
        {
            Recorder.FilenamePrefix ="["+ Time.realtimeSinceStartup.ToString()+"]"+ researchID + subjectID;
            Recorder.StartCapture();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        frequencyCounter += Time.deltaTime;
        if (frequencyCounter > DataRecordingFreq)
        {
            frequencyCounter = 0;
            List<SerialisableTransform> transformList = GetTransformsFromObjectList(trackedGameObject);
            DataContainer researchDataContainter = new DataContainer();
            researchDataContainter.TimeFromStart = Time.realtimeSinceStartup;
            researchDataContainter.SubjectID = subjectID;
            researchDataContainter.ResearchName = researchID;
            researchDataContainter.TransformList = transformList;
            if (LocalLogging)
            {
                LocalLoggerList.Add(researchDataContainter);
            }


            //TODO: Add Web Component,  
            //string jsonString = JsonConvert.SerializeObject(researchDataContainter);
            //Debug.Log(jsonString);
        }



    }


    private void OnApplicationQuit()
    {
        File.WriteAllText("Assets/ResearchDataCollector/questionnaire/Answers/" + filename + "log.json", JsonConvert.SerializeObject(LocalLoggerList));
    }


    private List<SerialisableTransform>  GetTransformsFromObjectList(List<GameObject> trackedGameObject)
    {
        List<SerialisableTransform> trackedTransformList = new List<SerialisableTransform>();
        foreach (var item in trackedGameObject)
        {
            trackedTransformList.Add(new SerialisableTransform(item.transform));
        }
        return (trackedTransformList);
    }

    public void LogThisObject(GameObject toBeLoggedObject)
    {
        trackedGameObject.Add(toBeLoggedObject);
    }
}
