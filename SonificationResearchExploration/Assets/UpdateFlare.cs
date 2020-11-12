using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class UpdateFlare : MonoBehaviour
{
    public MeshRenderer flareMesh;
    private bool isFlareOn;
    private float counter;
    private float flareDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlareOn)
        {
            counter += Time.deltaTime;
            if (counter > flareDuration)
            {
                flareMesh.enabled = false;
            }
        }
    }

    public void EnableFlare(float duration =1f)
    {
        flareDuration = duration;
        flareMesh.enabled = true;
        isFlareOn = true;
    }

}
