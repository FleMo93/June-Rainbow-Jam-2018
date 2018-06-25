using System.Collections.Generic;
using UnityEngine;

public class scr_Human_Motor : MonoBehaviour, i_Human_Motor
{
    [SerializeField]
    private float _PlayerHeight = 2;
    [SerializeField]
    private float _PlayerWidth = 1;
    [SerializeField]
    private GameObject _Model;
    [SerializeField]
    private Transform _PickupTransform;

    private scr_Stats stats;
    private i_Human_Animator animator;
    private bool waitForAnimationFinished = false;

    //movement
    private scr_Stats.Directions move_ActualMoveDirection = scr_Stats.Directions.None;
    private scr_Stats.Directions move_PressedMoveDirection = scr_Stats.Directions.None;
    private scr_Stats.Directions move_LookAtDirection = scr_Stats.Directions.None;
    private Vector3 move_HalfExtents;

    //dragable
    private i_Draggable drag_Draggable = null;
    private GameObject drag_DraggableGameObject = null;
    private scr_Stats.Directions drag_RelativePlayerDirectionToDragable = scr_Stats.Directions.None;

    //intentory
    private scr_Stats.ObjectType pickup_ObjectType = scr_Stats.ObjectType.None;
    private GameObject pickup_Object = null;
    private bool pickup_SomethingInHand = false;

    //attack
    private i_Damageable damage_ObjectToDamage = null;
    private int damage_DamageStrength = 0;

    void Start ()
    {
        stats = GetComponent<scr_Stats>();
        animator = GetComponentInChildren<i_Human_Animator>();
        animator.OnPickup += Animator_OnPickup;
        animator.OnAttack += Animator_OnAttack;
        animator.OnAnimationTransition += Animator_OnAnimationFinished;

        move_HalfExtents = new Vector3(
            _PlayerWidth / 2,
            _PlayerHeight / 2,
            _PlayerWidth / 2
            );
    }

    public void MoveUp()
    {
        move_PressedMoveDirection = scr_Stats.Directions.Up;
    }

    public void MoveRight()
    {
        move_PressedMoveDirection = scr_Stats.Directions.Right;
    }

    public void MoveDown()
    {
        move_PressedMoveDirection = scr_Stats.Directions.Down;
    }

    public void MoveLeft()
    {
        move_PressedMoveDirection = scr_Stats.Directions.Left;
    }

    scr_Stats.Interaction i_Human_Motor.Interact()
    {
        if (move_ActualMoveDirection == scr_Stats.Directions.None && animator.ReadyForInteraction()) 
        {
            return Interact();
        }
        else
        {
            return scr_Stats.Interaction.None;
        }
    }

    void Update ()
    {
        if(waitForAnimationFinished)
        {
            return;
        }

        Move();
        move_PressedMoveDirection = scr_Stats.Directions.None;
        Rotate();
    }

    Vector3 targetMoveTo;
    private void Move()
    {
        if (move_ActualMoveDirection == scr_Stats.Directions.None && move_PressedMoveDirection != scr_Stats.Directions.None)
        {
            targetMoveTo = scr_Tilemap.Get.GetNextTile(move_PressedMoveDirection, this.transform.position);

            if (drag_Draggable == null)
            {
                move_LookAtDirection = move_PressedMoveDirection;
            }

            if(drag_Draggable == null && scr_Tilemap.Get.IsTileFree(targetMoveTo, move_HalfExtents))
            {
                move_ActualMoveDirection = move_PressedMoveDirection;
                
            }
            else if(drag_Draggable != null && IsDragDirectionFree())
            {
                move_ActualMoveDirection = move_PressedMoveDirection;
            }
        }

        if (move_ActualMoveDirection != scr_Stats.Directions.None)
        {
            animator.Walk();
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetMoveTo, stats.MoveSpeed * Time.deltaTime);
        }
        else
        {
            animator.Idle();
        }

        if(targetMoveTo == this.transform.position)
        {
            move_ActualMoveDirection = scr_Stats.Directions.None;
        }
    }

    private bool IsDragDirectionFree()
    {
        Collider[] colliders = scr_Tilemap.Get.GetCollidersOnTile(targetMoveTo, move_HalfExtents);
        bool playerCanMove = true;

        foreach(Collider collider in colliders)
        {
            if(collider.gameObject != drag_DraggableGameObject)
            {
                playerCanMove = false;
                break;
            }
        }

        return drag_Draggable.MovementPossible(move_PressedMoveDirection, drag_RelativePlayerDirectionToDragable) && playerCanMove;
    }

    private void Rotate()
    {
        Vector3 dir;

        switch(move_LookAtDirection)
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
        Vector3 target = scr_Tilemap.Get.GetNextTile(move_LookAtDirection, this.transform.position);
        Collider[] colliders = scr_Tilemap.Get.GetCollidersOnTile(target, move_HalfExtents);

        List<KeyValuePair<GameObject, i_Interactable>> list = new List<KeyValuePair<GameObject, i_Interactable>>();

        foreach(Collider collider in colliders)
        {
            i_Interactable[] interactables = collider.gameObject.GetComponents<i_Interactable>();
            if(interactables != null)
            {
                foreach (var inta in interactables)
                {
                    if (inta != null)
                    {
                        list.Add(new KeyValuePair<GameObject, i_Interactable>(collider.gameObject, inta));
                    }
                }
                
            }
        }

        return list.ToArray();
    }

    private scr_Stats.Interaction Interact()
    {
        scr_Stats.Interaction? firstInteraction = null;
        KeyValuePair<GameObject, i_Interactable>[] interactables = GetInteractable();

        foreach (KeyValuePair<GameObject, i_Interactable> interactable in interactables)
        {
            scr_Interactable_Result result = interactable.Value.Interact(this.gameObject, pickup_ObjectType);

            if (!result.InteractionSuccessfull)
            {
                continue;
            }

            scr_Stats.Interaction interaction = result.Interaction;

            if(!firstInteraction.HasValue)
            {
                firstInteraction = interaction;
            }

            if(result.Dragable != null)
            {
                Drag(result.Dragable, interactable.Key);
            }
             
            if(result.PickupObjectType != scr_Stats.ObjectType.None && this.pickup_Object == null && pickup_ObjectType == scr_Stats.ObjectType.None)
            {
                pickup_Object = interactable.Key;
                pickup_ObjectType = result.PickupObjectType;
                pickup_SomethingInHand = true;
                animator.Pickup();
                waitForAnimationFinished = true;
            }

            if (result.Damagable != null)
            {
                damage_ObjectToDamage = result.Damagable;
                switch(interaction)
                {
                    case scr_Stats.Interaction.ChopTree:
                        damage_DamageStrength = stats.ChopTreeStrength;
                        break;
                }

                animator.Attack();
                waitForAnimationFinished = true;
            }
        }

        if(!firstInteraction.HasValue && pickup_Object != null)
        {
            Vector3 position = scr_Tilemap.Get.GetNextTile(move_LookAtDirection, this.transform.position);

            if (scr_Tilemap.Get.IsTileFree(position, new Vector3(0.5f, 0.5f, 0.5f)))
            {
                pickup_SomethingInHand = false;
                animator.Pickup();
                waitForAnimationFinished = true;
            }
        }

        return firstInteraction.HasValue ? firstInteraction.Value : scr_Stats.Interaction.None;
    }

    private void Animator_OnPickup(object sender)
    {
        if (pickup_SomethingInHand)
        {
            pickup_Object.transform.position = _PickupTransform.transform.position;
            pickup_Object.transform.SetParent(_PickupTransform);
            pickup_Object.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            pickup_Object.transform.SetParent(null);
            pickup_Object.transform.position = scr_Tilemap.Get.GetNextTile(move_LookAtDirection, this.transform.position);
            pickup_Object.transform.rotation = Quaternion.identity;
            pickup_Object.GetComponent<BoxCollider>().enabled = true;
            pickup_Object = null;
            pickup_ObjectType = scr_Stats.ObjectType.None;
        }
    }

    private void Animator_OnAttack(object sender)
    {
        damage_ObjectToDamage.Damage(damage_DamageStrength);

    }

    private void Animator_OnAnimationFinished(object sender)
    {
        waitForAnimationFinished = false;
    }

    private void Drag(i_Draggable drag, GameObject draggableGameObject)
    {
        if(drag_Draggable == drag)
        {
            draggableGameObject.transform.parent = null;
            drag_Draggable = null;
            draggableGameObject = null;
            drag_RelativePlayerDirectionToDragable = scr_Stats.Directions.None;
        }
        else
        {
            drag_Draggable = drag;
            this.drag_DraggableGameObject = draggableGameObject;
            drag_RelativePlayerDirectionToDragable = move_LookAtDirection;
            draggableGameObject.transform.SetParent(this.transform);
        }
    }

    public Vector3 GetSize()
    {
        return new Vector3(_PlayerWidth, _PlayerHeight, _PlayerWidth);
    }

}