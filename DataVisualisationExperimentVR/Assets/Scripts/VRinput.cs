﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRinput : MonoBehaviour
{
    bool hasShot;
    // Start is called before the first frame update
    void Start()
    {
        hasShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.DataVisualisation.SelectCube.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            if (hasShot == false)
            {
                PlayDay();
                //DataContainer.instance.currentDatapoint = DataContainer.instance.dataBase[1000];
                hasShot = true;
            }
        }
        if (SteamVR_Actions.DataVisualisation.SelectCube.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            hasShot = false;
        }

        


    }

    private void PlayDay()
    {
        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(raycast, out hit);
        if (!DataContainer.instance.isPlaying)
        {
            hit.collider.gameObject.GetComponentInParent<DayData>().playDay = true;
            DataContainer.instance.isPlaying = true;
        }
    }
}
