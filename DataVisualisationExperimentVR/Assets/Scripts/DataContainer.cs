using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine; 

public class DataContainer : MonoBehaviour
{
    List<Datapoint> dataBase = new List<Datapoint>();
    JsonSerializerSettings settings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };


    // Start is called before the first frame update
    void Start()
    {
        string jsonData = File.ReadAllText("Assets/Data/CleanedMergedData.json");
        dataBase = JsonConvert.DeserializeObject<List<Datapoint>>(jsonData,settings);
        Debug.Log(dataBase[1].Year);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
