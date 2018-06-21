using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Interactable_Result
{
    public scr_Stats.Interaction Interaction { get; private set; }
    public bool InteractionSuccessfull { get; private set; }
    public scr_Stats.ObjectType PickupObjectType { get; private set; }
    public i_Draggable Dragable;
    public i_Damageable Damagable;

    public scr_Interactable_Result(
        scr_Stats.Interaction interaction,
        bool interactionSuccessfull,
        scr_Stats.ObjectType pickupObjectType = scr_Stats.ObjectType.None,
        i_Draggable draggable = null,
        i_Damageable damagable = null)
    {
        this.Interaction = interaction;
        this.InteractionSuccessfull = interactionSuccessfull;
        this.PickupObjectType = pickupObjectType;
        this.Dragable = draggable;
        this.Damagable = damagable;

        Validate();
    }

    private void Validate()
    {
        int count = 0;
        count += PickupObjectType != scr_Stats.ObjectType.None ? 1 : 0;
        count += Dragable != null ? 1 : 0;
        count += Damagable != null ? 1 : 0;

        if(count > 1)
        {
            throw new System.Exception("Interactable result not valid!");
        }
    }
}
