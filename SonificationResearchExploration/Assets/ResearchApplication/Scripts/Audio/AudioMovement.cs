using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMovement : MonoBehaviour
{
    float timeCounter;
    public float depth;
    public float width;
    public float height; 
    //public float radius=2;
    public float speed =1;
    public bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z+2f);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Optimise/ find more performant alternative
        if (move)
        {
            timeCounter += Time.deltaTime * speed;

            float x = (Mathf.Cos(timeCounter) * depth);
            float z = (Mathf.Sin(timeCounter) * width);
            float y = Mathf.Sin(timeCounter) * height;

            transform.localPosition = new Vector3(x, y, z);
        }
 
    }

    public void SetAudioLocation(Vector3 location)
    {
        move = false;
        this.gameObject.transform.position = location;
    }
}
