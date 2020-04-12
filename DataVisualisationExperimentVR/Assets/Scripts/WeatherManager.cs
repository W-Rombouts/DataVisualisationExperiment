using DigitalRuby.RainMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeatherManager : MonoBehaviour
{
    GameObject sun;
    private Light sunLight;
    public static WeatherManager instance;
    public Datapoint currentDatapoint;
    private Boolean dataIsChanged = true;
    private RainScript rainMaker;

    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Sun");
        sunLight = sun.GetComponent<Light>();
        rainMaker = gameObject.GetComponent<RainScript>();
        currentDatapoint = DataContainer.instance.currentDatapoint;
    }

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



    // Update is called once per frame
    void Update()
    {
        if (DataContainer.instance.currentDatapoint != currentDatapoint)
        {
            dataIsChanged = true;
            currentDatapoint = DataContainer.instance.currentDatapoint;
        }

        if (dataIsChanged)
        {
            rainMaker.RainIntensity = (float)(currentDatapoint.DR/ (DataStats.instance.rainMax +DataStats.instance.rainMin));
            sunLight.intensity = (float)currentDatapoint.T/(DataStats.instance.tempMax + DataStats.instance.tempMin );
            sun.transform.rotation=(GetRotationBasedOnTime(currentDatapoint.Time));
            dataIsChanged = false;
        }
    }

    private Quaternion GetRotationBasedOnTime(string time)
    {
        int hourinminutes = int.Parse(time.Substring(0, 2)) * 60;
        int minutes = int.Parse(time.Substring(3, 2));
        int totalminutes = hourinminutes + minutes;
        float timeInDegrees = (totalminutes / 4);//split hours and minutes conver to int. *60 the hours to get hour in minutes. add hours in minutes to minutes. divide by 4 to 15min intervals and scale to 360
        int adjustment = -90;
        //Debug.Log(time+" = " +hourinminutes+" + "+minutes+" = "+totalminutes+" wich /4  = " +timeInDegrees);
        return Quaternion.Euler(new Vector3(timeInDegrees-90, 0, 0));
    }
}
