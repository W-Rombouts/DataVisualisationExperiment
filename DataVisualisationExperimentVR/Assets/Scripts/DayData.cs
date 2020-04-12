using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayData : MonoBehaviour
{
    public int day = 1;

    public bool playDay;
    public List<Datapoint> dayData = new List<Datapoint>();
    Datapoint firstDatapointOfDay; // Change to average of the day in the future
    TextMeshProUGUI tempText;
    TextMeshProUGUI fosfaatText;
    TextMeshProUGUI neerslagText;
    MeshRenderer tempMesh;
    MeshRenderer fosfaatMesh;
    MeshRenderer neerslagMesh;
    Vector3 tempHighColor = new Vector3((255f / 255f), (0f / 255f), (0f / 255f));
    Vector3 tempMediumColor = new Vector3((69f / 255f), (161f / 255f), (247f / 255f));
    Vector3 tempLowColor = new Vector3((41f / 255f), (86f / 255f), (213f / 255f));
    readonly int tempSwitchTreshold = 0;
    readonly float fosfaatSwitchTreshold = 1.5f;
    Color actualTempColor;
    Color actualFosfaatColor;



    Vector3 neerslagLowColor = new Vector3((255f / 255f), (255f / 255f), (255f / 255f));
    Vector3 neerslagHighColor = new Vector3((5f / 255f), (8f / 255f), (84f / 255f));
    Color actualNeerslagColor;


    Vector3 fosfaatLowColor = new Vector3((255f / 255f), (255f / 255f), (255f / 255f));
    Vector3 fosfaatHighColor = new Vector3((185f / 255f), (63f / 255f), (14f / 255f));

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI[] textMeshes = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            if (textMesh.name == "TempText")
            {
                tempText = textMesh;
            }
            else if(textMesh.name == "FosfaatText")
            {
                fosfaatText = textMesh;
            }
            else
            {
                neerslagText = textMesh;
            }
        }
        MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            if (meshRenderer.name == "Temp")
            {
                tempMesh = meshRenderer;
            }
            else if (meshRenderer.name == "Fosfaat")
            {
                fosfaatMesh = meshRenderer;
            }
            else
            {
                neerslagMesh = meshRenderer;
            }
        }

        
        dayData = gameObject.GetComponentInParent<MonthData>().GetDayData(day);
        firstDatapointOfDay = dayData[0];

        Vector3 Hsv;
        Color.RGBToHSV(V3toColor(neerslagHighColor), out Hsv.x, out Hsv.y, out Hsv.z);
        Hsv.y = (float)firstDatapointOfDay.DR / (DataStats.instance.rainMax + DataStats.instance.rainMin)+0.3f;
        Hsv.z = 1;
        actualNeerslagColor = Color.HSVToRGB(Hsv.x, Hsv.y, Hsv.z);

        if (firstDatapointOfDay.T >= tempSwitchTreshold )
        {
            Color.RGBToHSV(V3toColor(tempHighColor), out Hsv.x, out Hsv.y, out Hsv.z);
            Hsv.y = ((float)firstDatapointOfDay.T / (DataStats.instance.tempMax + tempSwitchTreshold))+.4f;
            Hsv.z = .85f;
            actualTempColor = Color.HSVToRGB(Hsv.x, Hsv.y, Hsv.z);
        }
        else
        {
            Color.RGBToHSV(V3toColor(tempLowColor), out Hsv.x, out Hsv.y, out Hsv.z);
            Hsv.y =1- ((float)firstDatapointOfDay.T / (tempSwitchTreshold + DataStats.instance.tempMin))-.4f;
            Hsv.z = .85f;
            actualTempColor = Color.HSVToRGB(Hsv.x, Hsv.y, Hsv.z);
        }

        if (firstDatapointOfDay.fosfaatMetingAT1 >= fosfaatSwitchTreshold)
        {
            Color.RGBToHSV(V3toColor(fosfaatHighColor), out Hsv.x, out Hsv.y, out Hsv.z);
            Hsv.z =((float)firstDatapointOfDay.fosfaatMetingAT1 / (DataStats.instance.fosfaatMax + fosfaatSwitchTreshold))+0.5f;
            Hsv.y = 1f;
            actualFosfaatColor = Color.HSVToRGB(Hsv.x, Hsv.y, Hsv.z);
        }
        else
        {
            Color.RGBToHSV(V3toColor(fosfaatHighColor), out Hsv.x, out Hsv.y, out Hsv.z);
            Hsv.y = ((float)firstDatapointOfDay.fosfaatMetingAT1 / (fosfaatSwitchTreshold + DataStats.instance.fosfaatMin))+.5f;
            Hsv.z = 1f;
            //Hsv.z = .85f;
            actualFosfaatColor = Color.HSVToRGB(Hsv.x, Hsv.y, Hsv.z);
        }

        tempMesh.material.color = actualTempColor;  
        neerslagMesh.material.color = actualNeerslagColor;
        fosfaatMesh.material.color = actualFosfaatColor;

        tempText.color = Color.white;
        tempText.SetText((firstDatapointOfDay.T / 10) + " Celcius");
        fosfaatText.SetText(firstDatapointOfDay.fosfaatMetingAT1.ToString("0.00") + " mg/l");
        neerslagText.SetText((firstDatapointOfDay.DR / 10) + " mm");
    }

     int playDayCounter = 0;
     float playDayTimer = 3f;

    // Update is called once per frame
    void Update()
    {
        if (playDay)
        {
            playDayTimer += Time.deltaTime;
            if (playDayTimer > 2f)
            {
                DataContainer.instance.currentDatapoint = dayData[playDayCounter];
                playDayCounter += 1;
                playDayTimer -= 2f;
                if (playDayCounter>= dayData.Count)
                {
                    playDay = false;
                    playDayCounter = 0;
                    playDayTimer = 3f;
                    DataContainer.instance.isPlaying = false;
                }
            }
            
        }
    }

    public Color V3toColor(Vector3 v3Color)
    {
        return new Color(v3Color.x, v3Color.y, v3Color.z);
    }

}
