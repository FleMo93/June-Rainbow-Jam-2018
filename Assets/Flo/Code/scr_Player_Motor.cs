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

    private enum Directions { Up, Right, Down, Left, None }
    private Directions actualMoveDirection = Directions.None;
    private Directions pressedMoveDirection = Directions.None;
    private Directions lookAtDirection = Directions.None;

    private i_Player_Input input;
    private scr_Stats stats;
    

	void Start ()
    {
        input = GetComponent<i_Player_Input>();
        stats = GetComponent<scr_Stats>();
	}
	
	void Update ()
    {
        if (input.IsMovingUp())
        {
            pressedMoveDirection = Directions.Up;
        }
        else if (input.IsMovingRight())
        {
            pressedMoveDirection = Directions.Right;
        }
        else if(input.IsMovingDown())
        {
            pressedMoveDirection = Directions.Down;
        }
        else  if(input.IsMovingLeft())
        {
            pressedMoveDirection = Directions.Left;
        }
        else
        {
            pressedMoveDirection = Directions.None;
        }


        Move();
        Rotate();

        if(pressedMoveDirection == Directions.None &&
            actualMoveDirection == Directions.None &&
            input.IsInteracting())
        {
            foreach(i_Interactable interactable in GetInteractable())
            {
                interactable.Interact();
            }
        }
    }

    Vector3 target;
    private void Move()
    {
        if (actualMoveDirection == Directions.None && pressedMoveDirection != Directions.None)
        {
            target = GetTarget(pressedMoveDirection);
            lookAtDirection = pressedMoveDirection;

            if(IsTargetFree(target))
            {
                actualMoveDirection = pressedMoveDirection;
            }
        }

        if (actualMoveDirection != Directions.None)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, stats.MoveSpeed * Time.deltaTime);
        }

        if(target == this.transform.position)
        {
            actualMoveDirection = Directions.None;
        }
    }

    private Vector3 GetTarget(Directions direction)
    {
        Vector3 target = this.transform.position;

        switch(direction)
        {
            case Directions.Up:
                target = new Vector3(
                    target.x,
                    target.y,
                    RoundToDecimal5(target.z + 1)
                    );
                break;

            case Directions.Right:
                target = new Vector3(
                    RoundToDecimal5(target.x + 1),
                    target.y,
                    target.z
                    );
                break;

            case Directions.Down:
                target = new Vector3(
                    target.x,
                    target.y,
                    RoundToDecimal5(target.z - 1)
                    );
                break;

            case Directions.Left:
                target = new Vector3(
                    RoundToDecimal5(target.x - 1),
                    target.y,
                    target.z
                    );
                break;
        }

        return target;
    }

    private float RoundToDecimal5(float val)
    {
        val = val * 10;
        val = Mathf.Round(val / 5.0f) * 5;
        return val / 10;
    }

    private Collider[] GetTargetObjects(Vector3 targetTile)
    {
        Vector3 center = new Vector3(
            targetTile.x,
            targetTile.y + _PlayerHeight / 2,
            targetTile.z
            );
        Vector3 halfExtents = new Vector3(
            _PlayerWidth / 2 - 0.001f,
            _PlayerHeight / 2 - 0.001f,
            _PlayerWidth / 2 - 0.001f
            );

        return Physics.OverlapBox(center, halfExtents);
    }

    private bool IsTargetFree(Vector3 targetTile)
    {
        return GetTargetObjects(targetTile).Length == 0;
    }

    private void Rotate()
    {
        Vector3 dir;

        switch(lookAtDirection)
        {
            case Directions.Up:
                dir = Vector3.forward;
                break;

            case Directions.Right:
                dir = Vector3.right;
                break;

            case Directions.Down:
                dir = Vector3.back;
                break;

            case Directions.Left:
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

    private i_Interactable[] GetInteractable()
    {
        Vector3 target = GetTarget(lookAtDirection);
        Collider[] colliders = GetTargetObjects(target);

        List<i_Interactable> list = new List<i_Interactable>();

        foreach(Collider collider in colliders)
        {
            i_Interactable interactable = collider.gameObject.GetComponent<i_Interactable>();
            if(interactable != null)
            {
                list.Add(interactable);
            }
        }

        return list.ToArray();
    }
}
