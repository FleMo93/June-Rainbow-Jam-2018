using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_soundButtonGeneral : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    public AudioClip ac_hoverEnter;
    public AudioClip ac_click;

    AudioSource audioSource;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && ac_click != null)
        {
            audioSource.clip = ac_click;
            audioSource.Play();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && ac_hoverEnter != null)
        {
            audioSource.clip = ac_hoverEnter;
            audioSource.Play();
        }
    }

    private void OnEnable()
    {
        if (audioSource == null)
        {
            GameObject camTarget = GameObject.Find("CamTarget");
            if (camTarget != null)
            {
                audioSource = camTarget.GetComponent<AudioSource>();
                if (audioSource == null)
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
