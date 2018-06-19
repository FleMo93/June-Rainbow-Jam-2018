using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class scr_PointOfInterest : MonoBehaviour
{

    public scr_Stats reference;
    public TextMesh tm;
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

        bool humansInscene = false;

        foreach (var p in poi)
        {
            if (p.reference.InteractionType == scr_Stats.Interaction.TalkToHuman)
            {
                humansInscene = true;
                break;
            }
        }


        reference.myCurrentPOI = poi[Random.Range(0, poi.Length)];

        if (humansInscene && Random.Range(0f,1f) > .4f)
        {
            scr_PointOfInterest[] poi_humans = poi.Where(x => x.reference.InteractionType == scr_Stats.Interaction.TalkToHuman).ToArray();

            reference.myCurrentPOI = poi_humans[Random.Range(0, poi_humans.Length)];
        }

    }

    static scr_PointOfInterest[] GetListOfPointOfInterests()
    {
        scr_PointOfInterest[] result = Object.FindObjectsOfType<scr_PointOfInterest>();
        return result;
    }
}
