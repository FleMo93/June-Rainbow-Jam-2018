using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flo_scr_Switch : MonoBehaviour, i_Interactable {

    bool on = false;
    [SerializeField]
    private GameObject changeColorOn;

    public scr_Stats.Interaction Interact()
    {
        on = !on;

        if(on)
        {
            changeColorOn.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            changeColorOn.GetComponent<Renderer>().material.color = Color.red;
        }

        return scr_Stats.Interaction.ChangeSwitch;
    }
}
