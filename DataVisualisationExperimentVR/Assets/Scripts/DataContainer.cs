using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public bool isPlaying;
    public Datapoint currentDatapoint;
    public GameObject NorthWall;
    public GameObject EastWall;
    public GameObject SouthWall;
    public GameObject WestWall;


    private Datapoint SampleDatapoint = new Datapoint { T = 250, Day = 1, Time = "06:00:00", fosfaatMetingAT1 = 1.5, DR = 3 };
    public Datapoint DefaultDatapoint = new Datapoint { T = 250, Day = 1, Time = "12:00:00", fosfaatMetingAT1 = 1.5, DR = 0 };
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
        currentDatapoint = DefaultDatapoint;
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

    public void GetByYear()
    {
        string jsonData = File.ReadAllText("Assets/Data/CleanedMergedData.json");
        dataBase = JsonConvert.DeserializeObject<List<Datapoint>>(jsonData, settings);
        currentDatapoint = SampleDatapoint;
        NorthWall.GetComponent<MonthData>().GenerateMonth();
        EastWall.GetComponent<MonthData>().GenerateMonth();
        SouthWall.GetComponent<MonthData>().GenerateMonth();
        WestWall.GetComponent<MonthData>().GenerateMonth();
    }



}
