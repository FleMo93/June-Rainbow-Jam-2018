using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ui_state : MonoBehaviour
{

    public uiState UI_State = uiState.MainMenu;

    public float speed;

    public RectTransform reMainMenu;
    public RectTransform reOptions;

    public Transform leftPos;
    public Transform midPos;
    public Transform rightPos;


    // Update is called once per frame
    void Update()
    {

        if (UI_State == uiState.MainMenu ) 
        {

            //if (reMainMenu.gameObject.activeSelf == false)
            //{
            //    reOptions.gameObject.SetActive(false);
            //    reMainMenu.gameObject.SetActive(true);
            //}

            float step = speed * Time.deltaTime;
            reMainMenu.position = Vector3.MoveTowards(reMainMenu.position, midPos.position, step);
            reOptions.position = Vector3.MoveTowards(reOptions.position, rightPos.position, step);
        }
        else if (UI_State == uiState.Options )
        {
            //if (reOptions.gameObject.activeSelf == false)
            //{
            //    reMainMenu.gameObject.SetActive(false);
            //    reOptions.gameObject.SetActive(true);
            //}

            float step = speed * Time.deltaTime;
            reOptions.position = Vector3.MoveTowards(reOptions.position, midPos.position, step);
            reMainMenu.position = Vector3.MoveTowards(reMainMenu.position, leftPos.position, step);
        }

    }
}

public enum uiState
{
    NONE,
    MainMenu,
    Options
}