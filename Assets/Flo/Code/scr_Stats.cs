﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Stats : MonoBehaviour {
    public enum Interaction { None, ChangeSwitch, ChopTree }

    public Interaction InteractionType = Interaction.None;
    public int ChopTreeStrength = 0;
    public int Health = 0;
    public float MoveSpeed = 0;
    public float RotationSpeed = 0;
}
