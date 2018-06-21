using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flo_scr_Tree : MonoBehaviour, i_Damageable, i_Interactable {

    scr_Stats stats;

    void Start ()
    {
        stats = GetComponent<scr_Stats>();
	}



    public void Damage(int damage)
    {
        stats.Health -= damage;

        if(stats.Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
    {
        return new scr_Interactable_Result(scr_Stats.Interaction.ChopTree, true);
    }

}
