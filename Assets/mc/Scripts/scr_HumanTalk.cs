using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_HumanTalk : MonoBehaviour, i_Interactable
{

    public scr_Stats reference;

    public string TextResponse = "...";

    public List<string> textComponents_struct_adj;
    public List<string> textComponents_struct_rel;
    public List<string> textComponents_adj;
    public List<string> textComponents_rel;
    public List<string> textComponents_obj;

    int scriptDelay = 10;

    void Awake()
    {
        if (reference == null)
        {
            reference = GetComponent<scr_Stats>();
        }
        if (reference == null)
        {
            Destroy(this);
        }

        initTextComponents();
    }

    private void initTextComponents()
    {
        textComponents_adj.Add("cool");
        textComponents_adj.Add("nice");
        textComponents_adj.Add("ugly");
        textComponents_adj.Add("fast");
        textComponents_adj.Add("big");
        textComponents_adj.Add("wierd");
        textComponents_adj.Add("ok");

        textComponents_rel.Add("like");
        textComponents_rel.Add("dislike");
        textComponents_rel.Add("connect to");
        textComponents_rel.Add("admire");
        textComponents_rel.Add("hate");

        textComponents_struct_adj.Add("i think {0} is {1}");
        textComponents_struct_adj.Add("{0} is {1}");

        textComponents_struct_rel.Add("i {0} {1}");
        textComponents_struct_rel.Add("i dont {0} {1}");

        textComponents_obj.Add("maybe there is somthing here that can help us");
        textComponents_obj.Add("there are too many trees here");
        textComponents_obj.Add("I would give everything to reach my goals");
        textComponents_obj.Add("the story about the Altar is nothing but a myth");
        textComponents_obj.Add("an Axe is indeed a usefull tool,... sometimes");
    }

    private void Update()
    {
        if (scriptDelay <= 0)
        {
            return;
        }
        else if (scriptDelay > 1)
        {
            scriptDelay--;
            if (scriptDelay == 1)
            {
                GenerateTextResponse();
            }

        }
    }



    private void GenerateTextResponse()
    {
        if (reference && reference.myCurrentPOI && reference.myCurrentPOI.reference)
        {
            if (reference.myCurrentPOI.reference.InteractionType == scr_Stats.Interaction.TalkToHuman)
            {
                if (UnityEngine.Random.Range(0f, 1f) > .5f)
                {
                    TextResponse = string.Format(textComponents_struct_adj[UnityEngine.Random.Range(0, textComponents_struct_adj.Count)], reference.myCurrentPOI.reference.Name, textComponents_adj[UnityEngine.Random.Range(0, textComponents_adj.Count)]);
                }
                else
                {
                    TextResponse = string.Format(textComponents_struct_rel[UnityEngine.Random.Range(0, textComponents_struct_rel.Count)], textComponents_rel[UnityEngine.Random.Range(0, textComponents_rel.Count)], reference.myCurrentPOI.reference.Name);
                }
            }

            else
            {
                TextResponse = textComponents_obj[UnityEngine.Random.Range(0, textComponents_obj.Count)];
            }
        }
    }

    public scr_Stats.Interaction Interact(GameObject trigger)
    {
        Debug.Log(TextResponse);
        return scr_Stats.Interaction.TalkToHuman;
    }
}
