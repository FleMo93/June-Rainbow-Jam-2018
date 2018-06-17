using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_btnUIState_MainMenu : MonoBehaviour {

    public scr_ui_state stateScript;

    public void click()
    {
        stateScript.UI_State = uiState.MainMenu;
    }
}
