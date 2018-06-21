using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface i_Interactable {
    scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory);
}
