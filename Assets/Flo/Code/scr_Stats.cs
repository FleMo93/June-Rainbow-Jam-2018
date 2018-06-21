using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Stats : MonoBehaviour {
    public enum Interaction { None, ChangeSwitch, ChopTree, Drag, TalkToHuman, Altar, Pickup, DropPickup }
    [HideInInspector]
    public enum Directions { Up, Right, Down, Left, None }
    public enum ObjectType { None, Axe }

    public Interaction InteractionType = Interaction.None;
    public int ChopTreeStrength = 0;
    public int Health = 0;
    public float MoveSpeed = 0;
    public string Name = "CoolNameBob";
    public string Story = "Has an Interestring story to tell.";
    public scr_PointOfInterest myCurrentPOI = null;
    public float RotationSpeed = 0;
}
