using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tree : MonoBehaviour, i_Interactable, i_Damageable {
    scr_Stats stats;
    [SerializeField]
    private ParticleSystem _ParticleOnDamage;
    [SerializeField]
    private GameObject _MainModel;
    [SerializeField]
    private GameObject _Stub;
    [SerializeField]
    private GameObject _FelledBody;

    void Start () {
        stats = gameObject.GetComponent<scr_Stats>();
        _Stub.SetActive(false);
        _FelledBody.SetActive(false);
	}

    public void Damage(int damage) 
    {
        stats.Health -= damage;
        _ParticleOnDamage.Play();

        if(stats.Health <= 0)
        {
            _Stub.SetActive(true);
            _FelledBody.SetActive(true);
            _MainModel.SetActive(false);
        }
    }

    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
    {
        bool successfull = itemInInventory == scr_Stats.ObjectType.Axe && stats.Health > 0;

        return new scr_Interactable_Result(scr_Stats.Interaction.ChopTree, successfull, damagable: this);
    }

}
