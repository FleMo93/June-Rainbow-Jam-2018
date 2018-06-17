using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_soundVolume : MonoBehaviour {
    
    public void OnSlider_ValueChanged(float input)
    {
        AudioListener.volume = input;
    }


}
