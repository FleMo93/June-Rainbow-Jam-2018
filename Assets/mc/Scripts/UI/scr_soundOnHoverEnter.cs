using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_soundOnHoverEnter : MonoBehaviour, IPointerEnterHandler
{

    public AudioClip Sound;

    AudioSource audioSource;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && Sound != null)
        {
            audioSource.clip = Sound;
            audioSource.Play();
        }
    }

    private void OnEnable()
    {
        if (Sound == null)
        {
            return;
        }
        if (audioSource == null)
        {
            GameObject camTarget = GameObject.Find("CamTarget");
            if (camTarget != null)
            {
                audioSource = camTarget.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    //audioSource.clip = Sound;
                }
                else
                {
                    Debug.LogWarning("Cant find AudioSource on CamTarget");
                }
            }
            else
            {
                Debug.LogWarning("Cant find CamTarget with AudioSource Component");
            }
        }
    }
    
}
