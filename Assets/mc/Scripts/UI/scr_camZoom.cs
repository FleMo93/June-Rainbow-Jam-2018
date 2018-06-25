using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_camZoom : MonoBehaviour
{
    public float zoomspeed =1;
    public float minZoom;
    public float maxZoom;
    private Camera c;


    // Use this for initialization
    void Start()
    {
        c = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (c == null)
        {
            Debug.Log("Error, no Camera found on this object");
            return;
        }
        float inputPosNeg = Input.GetAxis("Mouse ScrollWheel");
        float delta = zoomspeed / 10;
        if (inputPosNeg != 0)
        {
            if (inputPosNeg > 0)
            {
                if (c.orthographicSize + delta > maxZoom)
                {
                    c.orthographicSize = maxZoom;
                }
                else
                {
                    c.orthographicSize += delta;
                }
            }

            else if (inputPosNeg < 0)
            {
                if (c.orthographicSize - delta < minZoom)
                {
                    c.orthographicSize = minZoom;
                }
                else
                {
                    c.orthographicSize -= delta;
                }
            }
        }
    }
}
