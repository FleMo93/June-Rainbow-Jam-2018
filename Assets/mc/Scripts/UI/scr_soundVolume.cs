using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_soundVolume : MonoBehaviour {

   public void OnValueChanged()
    {
        AudioListener.volume = 2;
    }


}
