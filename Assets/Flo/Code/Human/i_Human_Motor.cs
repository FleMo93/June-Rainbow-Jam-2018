using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface i_Human_Motor {
    void MoveUp();
    void MoveRight();
    void MoveDown();
    void MoveLeft();
    scr_Stats.Interaction Interact();
    Vector3 GetSize();
}
