using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface i_Human_Motor {
    void MoveUp();
    void MoveRight();
    void MoveDown();
    void MoveLeft();
    void Interact();
    Vector3 GetSize();
}
