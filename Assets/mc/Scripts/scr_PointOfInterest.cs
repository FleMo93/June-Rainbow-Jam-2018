using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class scr_PointOfInterest : MonoBehaviour
{

    public scr_Stats reference;
    public TextMesh tm;
    float chanceToGiveTip = 0.63f;
    // Use this for initialization
    void Awake()
    {
        if (reference == null)
        {
            reference = GetComponent<scr_Stats>();
        }
        if (reference == null)
        {
            Destroy(this);
        }
        if (tm)
        {
            tm.text = reference.Name;
        }
    }

    private void Start()
    {
        scr_PointOfInterest[] poi = GetListOfPointOfInterests();
        reference.myCurrentPOI = poi[Random.Range(0, poi.Length)];

        if (Random.Range(0f, 1f) > (1 - chanceToGiveTip))  // chance to give TIP instead of talk about other Human
        {
            reference.myCurrentPOI = null;
        }

    }

    static scr_PointOfInterest[] GetListOfPointOfInterests()
    {
        scr_PointOfInterest[] result = Object.FindObjectsOfType<scr_PointOfInterest>();
        return result;
    }
}
