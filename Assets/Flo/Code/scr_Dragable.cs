using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Dragable : MonoBehaviour, i_Interactable, i_Draggable {
    [SerializeField]
    private bool _CanOnlyMoveForward = false;
    [SerializeField]
    private Vector3 _BoxSize = new Vector3(1, 1, 1);

    private Vector3 targetMoveTo;
    private float moveSpeed;

    public scr_Stats.Interaction Interact(GameObject trigger)
    {
        return scr_Stats.Interaction.DraggableBox;
    }

    public bool MovementPossible(scr_Stats.Directions direction, scr_Stats.Directions relativePlayerDirectionToBox)
    {
        if (_CanOnlyMoveForward)
        {
            if(direction != relativePlayerDirectionToBox)
            {
                return false;
            }

            Vector3 tile = scr_Tilemap.Get.GetNextTile(direction, this.transform.position);
            return scr_Tilemap.Get.IsTileFree(tile, _BoxSize * 0.5f);
        }
        else
        {
            if (ReverseDirection(direction) == relativePlayerDirectionToBox)
            {
                return true;
            }
            else
            {
                Vector3 tile = scr_Tilemap.Get.GetNextTile(direction, this.transform.position);
                return scr_Tilemap.Get.IsTileFree(tile, _BoxSize * 0.5f);
            }
        }
    }

    private scr_Stats.Directions ReverseDirection(scr_Stats.Directions direction)
    {
        switch(direction)
        {
            case scr_Stats.Directions.Down:
                return scr_Stats.Directions.Up;
            case scr_Stats.Directions.Left:
                return scr_Stats.Directions.Right;
            case scr_Stats.Directions.Right:
                return scr_Stats.Directions.Left;
            case scr_Stats.Directions.Up:
                return scr_Stats.Directions.Down;
            default:
                return scr_Stats.Directions.None;
        }
    }
}
