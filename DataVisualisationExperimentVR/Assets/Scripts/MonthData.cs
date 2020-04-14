using System.Collections.Generic;
using UnityEngine;

public class MonthData : MonoBehaviour
{
    public string month = "July";
    public GameObject Anker;
    public GameObject Day;
    List<int> daylist = new List<int>();
    List<Datapoint> monthData = new List<Datapoint>();
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<Datapoint> GetDayData(int day)
    {
        List<Datapoint> dayData = new List<Datapoint>();
        foreach (Datapoint datapoint in monthData)
        {
            if (datapoint.Day == day)
            {
                dayData.Add(datapoint);              
            }
           
        }        
        return dayData;
    }


    public void GenerateMonth()
    {
        var pos = transform.position;
        var rot = transform.rotation;
        monthData = DataContainer.instance.GetByMonth(month);
        foreach (Datapoint datapoint in monthData)
        {
            if (!daylist.Contains(datapoint.Day))
            {
                daylist.Add(datapoint.Day);
            }
        }
        int countery = 7;
        int counterx = 6;
        foreach (int dayNumber in daylist)
        {
            if (counterx == 6)
            {
                counterx = 0;
                countery--;
            }
            float x = counterx * 0.4f;
            float y = countery * 0.4f;

            GameObject createdDay = Instantiate(Day, new Vector3(x, y, 0f), Quaternion.Euler(0f, 90f, 0f), transform);
            createdDay.GetComponent<DayData>().day = dayNumber;
            counterx++;

        }
        transform.rotation = transform.rotation * Anker.transform.rotation;
        transform.position = Anker.transform.position;
    }

}
