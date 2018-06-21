using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flo_scr_Switch : MonoBehaviour, i_Interactable {

    bool on = false;
    [SerializeField]
    private GameObject changeColorOn;

    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
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

        return new scr_Interactable_Result(scr_Stats.Interaction.ChangeSwitch, true);
    }
}
