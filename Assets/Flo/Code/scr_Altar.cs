using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Altar : MonoBehaviour, i_Interactable {

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public scr_Stats.Interaction Interact(GameObject trigger)
    {
        return scr_Stats.Interaction.Altar;
    }
}
