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


    Color[] tempBinColors = new Color[5] { new Color(254f/255f,16f/255f,22f/255f), new Color(252f/255f, 88f/255f, 29f/255f), new Color(250f/255f, 170f/255f, 42f/255f), new Color(248f/255f, 229f/255f, 53f/255f), new Color(38f/255f, 152f/255f, 202f/255f) };
    Color[] fosfaatBinColors = new Color[7] { new Color(180f/255f, 255f/255f, 153f/255f), new Color(142f/255f, 255f/255f, 102f/255f), new Color(102f / 255f, 255f / 255f, 51f / 255f), new Color(70f / 255f, 255f / 255f, 0f / 255f), new Color(54f / 255f, 204f / 255f, 0f / 255f), new Color(41f / 255f, 153f / 255f, 0f / 255f), new Color(27f / 255f, 102f / 255f, 0f / 255f) };
    Color[] neerslagBinColors = new Color[5] { new Color(153f / 255f, 200f / 255f, 255f / 255f), new Color(77f / 255f, 159f / 255f, 255f / 255f), new Color(51f / 255f, 146f / 255f, 255f / 255f), new Color(26f / 255f, 131f / 255f, 255f / 255f), new Color(0f / 255f, 127f / 255f, 255f / 255f) };


    Color actualTempColor;
    Color actualFosfaatColor;



    Color actualNeerslagColor;


    // Start is called before the first frame update
    void Start()
    {
        dayData = gameObject.GetComponentInParent<MonthData>().GetDayData(day);
        firstDatapointOfDay = dayData[0];

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
                if (firstDatapointOfDay.T < 0)
                {
                    actualTempColor = tempBinColors[4]; ;
                }
                else if (firstDatapointOfDay.T < 100)
                {
                    actualTempColor = tempBinColors[3];
                }
                else if (firstDatapointOfDay.T < 200)
                {
                    actualTempColor = tempBinColors[2];
                }
                else if (firstDatapointOfDay.T < 300)
                {
                    actualTempColor = tempBinColors[1];
                }
                else
                {
                    actualTempColor = tempBinColors[0];
                }
                tempMesh.material.color = actualTempColor;
            }
            else if (meshRenderer.name == "Fosfaat")
            {
                fosfaatMesh = meshRenderer;
                if (firstDatapointOfDay.fosfaatMetingAT1 < 0.5)
                {
                    actualFosfaatColor = fosfaatBinColors[0]; ;
                }
                else if (firstDatapointOfDay.fosfaatMetingAT1 < 1)
                {
                    actualFosfaatColor = fosfaatBinColors[1];
                }
                else if (firstDatapointOfDay.fosfaatMetingAT1 < 1.5)
                {
                    actualFosfaatColor = fosfaatBinColors[2];
                }
                else if (firstDatapointOfDay.fosfaatMetingAT1 < 2)
                {
                    actualFosfaatColor = fosfaatBinColors[3];
                }
                else if (firstDatapointOfDay.fosfaatMetingAT1 < 2.5)
                {
                    actualFosfaatColor = fosfaatBinColors[4];
                }
                else if (firstDatapointOfDay.fosfaatMetingAT1 < 3)
                {
                    actualFosfaatColor = fosfaatBinColors[5];
                }
                else
                {
                    actualNeerslagColor = fosfaatBinColors[6];
                }

                fosfaatMesh.material.color = actualFosfaatColor;
            }
            else if (meshRenderer.name == "Neerslag")
            {
                neerslagMesh = meshRenderer;
                if (firstDatapointOfDay.DR < 2)
                {
                    actualNeerslagColor = neerslagBinColors[0]; ;
                }
                else if (firstDatapointOfDay.DR < 4)
                {
                    Debug.Log(firstDatapointOfDay.DR);
                    actualNeerslagColor = Color.white;
                    actualNeerslagColor = neerslagBinColors[1];
                }
                else if (firstDatapointOfDay.DR < 6)
                {
                    actualNeerslagColor = neerslagBinColors[2];
                }
                else if (firstDatapointOfDay.DR < 8)
                {
                    actualNeerslagColor = neerslagBinColors[3];
                }
                else
                {
                    actualNeerslagColor = neerslagBinColors[4];
                }
                neerslagMesh.material.color = actualNeerslagColor;
            }
            else
            {
                
            }
        }

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
        //TODO: Move to datamanager
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
