using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_btnStart : MonoBehaviour {

    public void click()
    {
        SceneManager.LoadScene("scn_Level_1");
        //SceneManager.LoadScene("Dev");
    }
}
