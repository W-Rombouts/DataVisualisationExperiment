using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using UnityEngine.UI;

public class VRInput : MonoBehaviour
{
    bool hasShot;
    bool isGrabbed;
    GameObject grabbedGameobject;
    QuestionairManager questionair;
    // Start is called before the first frame update
    void Start()
    {
        questionair = QuestionairManager.Instance;
        hasShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.Questionair_Control.SelectAnswer.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            hasShot = false;
            grabbedGameobject = null;
            isGrabbed = false;
           
        }
        if (SteamVR_Actions.Questionair_Control.SelectAnswer.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            PushUiButton();
        }

        if (SteamVR_Actions.Questionair_Control.NextQuestion.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            questionair.gameObject.SetActive(true);
            questionair.AskQuestion();
        }

        if (isGrabbed)
        {
            Ray raycast = new Ray(transform.position, transform.forward);
            Physics.Raycast(raycast, out RaycastHit hit);
            grabbedGameobject.transform.position = new Vector3( grabbedGameobject.transform.position.x,  grabbedGameobject.transform.position.y, hit.point.z) ;
        }


    }

    private void PushUiButton()
    {
        Ray raycast = new Ray(transform.position, transform.forward);
        Physics.Raycast(raycast, out RaycastHit hit);
        //Debug.Log(hit.collider.gameObject.name);
        if (hit.collider.gameObject.name == "DragCube" || isGrabbed)
        {
            if (isGrabbed == false)
            {
                grabbedGameobject = hit.collider.gameObject;
                isGrabbed = true;
            }

        }
        else if (hit.collider.gameObject.GetComponent<Button>() != null)
        {
            if (hasShot == false)
            {
                hasShot = true;
                hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
