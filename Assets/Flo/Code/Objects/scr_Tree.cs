using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tree : MonoBehaviour, i_Interactable, i_Damageable {
    scr_Stats stats;

    void Start () {
        stats = gameObject.GetComponent<scr_Stats>();
	}

    public void Damage(int damage)
    {
        stats.Health -= damage;
    }

    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
    {
        bool successfull = itemInInventory == scr_Stats.ObjectType.Axe && stats.Health > 0;

        return new scr_Interactable_Result(scr_Stats.Interaction.ChopTree, successfull, damagable: this);
    }

}
