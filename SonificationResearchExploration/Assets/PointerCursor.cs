using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class PointerCursor : MonoBehaviour
{
    public float distance=0f;
    public Transform source;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

        //TODO: Create button interaction
        distance += SteamVR_Actions.default_Extend.GetAxis(SteamVR_Input_Sources.RightHand).y*.01f;

        if (distance > 1f)
        {
            distance = 1f;
        }
        if (distance<0.005f)
        {
            distance = 0.005f;
        }
        //localPosition
        Vector3 min = new Vector3(source.transform.localPosition.x, source.transform.localPosition.y, source.transform.localPosition.z + 0.1f);
        Vector3 max = new Vector3(source.transform.localPosition.x, source.transform.localPosition.y, source.transform.localPosition.z + 5f);
        gameObject.transform.localPosition = Vector3.Lerp(min, max, distance); ;
    }

    public void SavePosition()
    {
        Debug.Log(gameObject.transform.position);
    }
}
