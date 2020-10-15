using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class VrSlider : MonoBehaviour
{
    public GameObject DragCube;
    public GameObject Min;
    public GameObject Max;
    public Slider slider;

    Vector3 lastDragCubePostion;
    float sliderWidth;
    // Start is called before the first frame update
    void Awake()
    {
        lastDragCubePostion = DragCube.transform.position;
        sliderWidth = Vector3.Distance(Min.transform.position, Max.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float minToDragDistance;
        if (DragCube.transform.position != lastDragCubePostion)
        {
            minToDragDistance = Vector3.Distance(Min.transform.position, DragCube.transform.position);
            //Clamping the value's between min and max
            if (Vector3.Distance(Max.transform.position, DragCube.transform.position) > sliderWidth)
            {
                minToDragDistance = 0;
            }
            if (minToDragDistance > sliderWidth)
            {
                minToDragDistance = sliderWidth;
            }
            slider.value = minToDragDistance / sliderWidth;
        }
        
    }
}
