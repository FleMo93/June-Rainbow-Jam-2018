using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Axe : MonoBehaviour, i_Interactable
{
    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
    {
        return new scr_Interactable_Result(scr_Stats.Interaction.Pickup, true, scr_Stats.ObjectType.Axe);
    }
}
