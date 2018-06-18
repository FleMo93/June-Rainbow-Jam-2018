using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Player_Motor : MonoBehaviour
{
    [SerializeField]
    private float _PlayerHeight = 2;
    [SerializeField]
    private float _PlayerWidth = 1;
    [SerializeField]
    private GameObject _Model;

    private scr_Stats.Directions actualMoveDirection = scr_Stats.Directions.None;
    private scr_Stats.Directions pressedMoveDirection = scr_Stats.Directions.None;
    private scr_Stats.Directions lookAtDirection = scr_Stats.Directions.None;

    private i_Player_Input input;
    private scr_Stats stats;

    private Vector3 halfExtents;
    private i_Draggable draggable = null;
    private GameObject draggableGameObject = null;
    private scr_Stats.Directions relativePlayerDirectionToBox = scr_Stats.Directions.None;

    void Start ()
    {
        input = GetComponent<i_Player_Input>();
        stats = GetComponent<scr_Stats>();
        halfExtents = new Vector3(
            _PlayerWidth / 2,
            _PlayerHeight / 2,
            _PlayerWidth / 2
            );
    }
	
	void Update ()
    {
        if (input.IsMovingUp())
        {
            pressedMoveDirection = scr_Stats.Directions.Up;
        }
        else if (input.IsMovingRight())
        {
            pressedMoveDirection = scr_Stats.Directions.Right;
        }
        else if(input.IsMovingDown())
        {
            pressedMoveDirection = scr_Stats.Directions.Down;
        }
        else  if(input.IsMovingLeft())
        {
            pressedMoveDirection = scr_Stats.Directions.Left;
        }
        else
        {
            pressedMoveDirection = scr_Stats.Directions.None;
        }


        Move();
        Rotate();

        if(pressedMoveDirection == scr_Stats.Directions.None &&
            actualMoveDirection == scr_Stats.Directions.None &&
            input.IsInteracting())
        {
            Interact();   
        }
    }

    Vector3 targetMoveTo;
    private void Move()
    {
        if (actualMoveDirection == scr_Stats.Directions.None && pressedMoveDirection != scr_Stats.Directions.None)
        {
            targetMoveTo = scr_Tilemap.Get.GetNextTile(pressedMoveDirection, this.transform.position);

            if (draggable == null)
            {
                lookAtDirection = pressedMoveDirection;
            }

            if(draggable == null && scr_Tilemap.Get.IsTileFree(targetMoveTo, halfExtents))
            {
                actualMoveDirection = pressedMoveDirection;
                
            }
            else if(draggable != null && IsDragDirectionFree())
            {
                actualMoveDirection = pressedMoveDirection;
                //draggable.Move(pressedMoveDirection, stats.MoveSpeed);
            }
        }

        if (actualMoveDirection != scr_Stats.Directions.None)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetMoveTo, stats.MoveSpeed * Time.deltaTime);
        }

        if(targetMoveTo == this.transform.position)
        {
            actualMoveDirection = scr_Stats.Directions.None;
        }
    }

    private bool IsDragDirectionFree()
    {
        Collider[] colliders = scr_Tilemap.Get.GetCollidersOnTile(targetMoveTo, halfExtents);
        bool playerCanMove = true;

        foreach(Collider collider in colliders)
        {
            if(collider.gameObject != draggableGameObject)
            {
                playerCanMove = false;
                break;
            }
        }

        return draggable.MovementPossible(pressedMoveDirection, relativePlayerDirectionToBox) && playerCanMove;
    }

    private void Rotate()
    {
        Vector3 dir;

        switch(lookAtDirection)
        {
            case scr_Stats.Directions.Up:
                dir = Vector3.forward;
                break;

            case scr_Stats.Directions.Right:
                dir = Vector3.right;
                break;

            case scr_Stats.Directions.Down:
                dir = Vector3.back;
                break;

            case scr_Stats.Directions.Left:
                dir = Vector3.left;
                break;
            default:
                return;
        }

        _Model.transform.rotation = Quaternion.Slerp(
            _Model.transform.rotation,
            Quaternion.LookRotation(dir),
            stats.RotationSpeed * Time.deltaTime);
    }

    private KeyValuePair<GameObject, i_Interactable>[] GetInteractable()
    {
        Vector3 target = scr_Tilemap.Get.GetNextTile(lookAtDirection, this.transform.position);
        Collider[] colliders = scr_Tilemap.Get.GetCollidersOnTile(target, halfExtents);

        List<KeyValuePair<GameObject, i_Interactable>> list = new List<KeyValuePair<GameObject, i_Interactable>>();

        foreach(Collider collider in colliders)
        {
            i_Interactable interactable = collider.gameObject.GetComponent<i_Interactable>();
            if(interactable != null)
            {
                list.Add(new KeyValuePair<GameObject, i_Interactable>(collider.gameObject, interactable));
            }
        }

        return list.ToArray();
    }

    private void Interact()
    {
        foreach (KeyValuePair<GameObject, i_Interactable> interactable in GetInteractable())
        {
            scr_Stats.Interaction interaction = interactable.Value.Interact(this.gameObject);

            switch(interaction)
            {
                case scr_Stats.Interaction.DraggableBox:
                    i_Draggable drag = interactable.Key.GetComponent<i_Draggable>();

                    if(drag != null)
                    {
                        Drag(drag, interactable.Key);
                    }

                    break;
            }
        }
    }

    private void Drag(i_Draggable drag, GameObject draggableGameObject)
    {
        if(draggable == drag)
        {
            draggableGameObject.transform.parent = null;
            draggable = null;
            draggableGameObject = null;
            relativePlayerDirectionToBox = scr_Stats.Directions.None;
        }
        else
        {
            draggable = drag;
            this.draggableGameObject = draggableGameObject;
            relativePlayerDirectionToBox = lookAtDirection;
            draggableGameObject.transform.SetParent(this.transform);
        }
    }
}
