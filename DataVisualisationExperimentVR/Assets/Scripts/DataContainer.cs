using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine; 

public class DataContainer : MonoBehaviour
{
    public bool isPlaying;
    public Datapoint currentDatapoint;
    public List<Datapoint> dataBase = new List<Datapoint>();
    JsonSerializerSettings settings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };
    public static DataContainer instance;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }




    // Start is called before the first frame update
    void Start()
    {
        string jsonData = File.ReadAllText("Assets/Data/CleanedMergedData.json");
        dataBase = JsonConvert.DeserializeObject<List<Datapoint>>(jsonData,settings);
        currentDatapoint = dataBase[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public List<Datapoint> GetByMonth(string month)
    {
        List<Datapoint> monthList = new List<Datapoint>();
        foreach  (Datapoint datapoint in dataBase)
        {
            if (datapoint.Month == month)
            {
                monthList.Add(datapoint);
            }
        }


        return monthList;
    }





}
