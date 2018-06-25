using System.Linq;
using UnityEngine;

public class scr_Player_Animator : MonoBehaviour, i_Human_Animator {
    private enum Animations { Idle, Walk, Pickup, Attack }

    [SerializeField]
    private Animations animationState = Animations.Idle;

    [SerializeField]
    private float _MinTimeToIdle2 = 2f;
    [SerializeField]
    private float _MaxTimeToIdle2 = 5f;

    private float timeToIdle2;
    private Animator antr;


    public event scr_Human_AnimatorEvents.PickedUpHandler OnPickup;
    public event scr_Human_AnimatorEvents.OnAttackHandler OnAttack;
    public event scr_Human_AnimatorEvents.OnAnimationFinished OnAnimationTransition;

    void Start () {
        timeToIdle2 = Random.Range(_MinTimeToIdle2, _MaxTimeToIdle2);
        antr = GetComponent<Animator>();

        //Pickup
        AnimationEvent ev = new AnimationEvent();
        ev.functionName = "FirePickup";
        ev.time = 0.4f;
        AnimationClip cl = antr.runtimeAnimatorController.animationClips.Where(x => x.name == "pickup").First();

        if (cl.events.Count() == 0) 
        {
            cl.AddEvent(ev);
        }

        //Attack
        ev = new AnimationEvent();
        ev.functionName = "FireAttack";
        ev.time = 0.7f;
        cl = antr.runtimeAnimatorController.animationClips.Where(x => x.name == "attack").First();
         
        if (cl.events.Count() == 0)
        {
            cl.AddEvent(ev);
        }
    }

    bool transitionGoing = false;

    void Update () {
        switch (animationState)
        {
            case Animations.Idle:
                timeToIdle2 -= Time.deltaTime;
                if (timeToIdle2 <= 0)
                {
                    antr.SetBool("Idle2", true);
                    timeToIdle2 = Random.Range(_MinTimeToIdle2, _MaxTimeToIdle2);
                }
                else
                {
                    antr.SetBool("Idle2", false);
                }

                antr.SetBool("Walk", false);
                antr.SetBool("Pickup", false);
                antr.SetBool("Attack", false);
                break;

            case Animations.Attack:
                antr.SetBool("Attack", true);
                break;

            case Animations.Walk:
                antr.SetBool("Walk", true);
                break;

            case Animations.Pickup:
                antr.SetBool("Pickup", true);
                break;
        }

        if (antr.IsInTransition(0) && antr.GetNextAnimatorClipInfo(0).First().clip.name == "idle")
        {
            transitionGoing = true;
            animationState = Animations.Idle;
            antr.SetBool("Idle2", false);
            antr.SetBool("Pickup", false);
            antr.SetBool("Attack", false);
            antr.SetBool("Walk", false);
        }
        else if(transitionGoing && !antr.IsInTransition(0))
        {
            FireAnimationFinished();
            transitionGoing = false;
        }
	}

    public void Idle()
    {
        animationState = Animations.Idle;
    }

    public void Walk()
    {
        animationState = Animations.Walk;
    }

    public void Pickup()
    {
        animationState = Animations.Pickup;
    }

    public void Attack()
    {
        animationState = Animations.Attack;
    }

    public void FirePickup()
    {
        if(OnPickup != null)
        {
            OnPickup(this);
        }
    }

    public void FireAttack()
    {
        if(OnAttack != null)
        {
            OnAttack(this);
        }
    }

    public void FireAnimationFinished()
    {
        timeToIdle2 = Random.Range(_MinTimeToIdle2, _MaxTimeToIdle2);
        if (OnAnimationTransition != null)
        {
            OnAnimationTransition(this);
        }
    }

    public bool ReadyForInteraction()
    {
        return !antr.IsInTransition(0) || antr.IsInTransition(0) && antr.GetNextAnimatorClipInfo(0).First().clip.name.StartsWith("idle") ;
    }
}
