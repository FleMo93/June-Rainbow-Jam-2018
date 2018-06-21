using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface i_Draggable {
    bool MovementPossible(scr_Stats.Directions directons, scr_Stats.Directions relativePlayerDirectionToBox);
}
