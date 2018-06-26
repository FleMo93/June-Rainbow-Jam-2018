using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface i_Human_Animator
{
    void Idle();
    void Walk();
    void Pickup();
    void Attack();
    void Cast();
    bool ReadyForInteraction();

    event scr_Human_AnimatorEvents.PickedUpHandler OnPickup;
    event scr_Human_AnimatorEvents.OnAttackHandler OnAttack;
    event scr_Human_AnimatorEvents.OnAnimationFinished OnAnimationTransition;
}

public class scr_Human_AnimatorEvents
{
    public delegate void PickedUpHandler(object sender);
    public delegate void OnAttackHandler(object sender);
    public delegate void OnAnimationFinished(object sender);
}