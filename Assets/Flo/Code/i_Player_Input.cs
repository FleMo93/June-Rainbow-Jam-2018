using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface i_Player_Input {

    bool IsInteracting();
    bool IsMovingUp();
    bool IsMovingRight();
    bool IsMovingDown();
    bool IsMovingLeft();

    bool IsSelectHumanUp();
    bool IsSelectHumanRight();
    bool IsSelectHumanDown();
    bool IsSelectHumanLeft();
    bool IsSelectHuman();
    bool IsStopSelectHuman();
}
