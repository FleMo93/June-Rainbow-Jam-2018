using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Altar : MonoBehaviour, i_Interactable {

    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
    {
        return new scr_Interactable_Result(scr_Stats.Interaction.Altar, true);
    }
}
