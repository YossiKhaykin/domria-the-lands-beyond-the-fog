using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class OffensiveAilment : CharEffect
{
    public enum Ailments
    {
        Burn,
        Poison,
        Curse,
        Freeze,
        Stun,
    }

    public Coroutine HurtEverTim;
    private float DOT;
    public float Hit_Rate;
    public Ailments ailment;
    private float TargetSpeed = 7;

    public override void ExecuteConEffect()
    {
        switch (ailment)
        {
            case Ailments.Poison:
                {
                    DOT = Target.GetComponent<NPC>().RollDmg();
                    TheCaster.GetComponent<SkillUser>().DmgOverTimTimer(this);
                    break;
                }
            case Ailments.Burn:
            case Ailments.Curse:
                {
                    TheCaster.GetComponent<SkillUser>().DmgOverTimTimer(this);
                    break;
                }
            case Ailments.Freeze:
                {
                    Target.GetComponent<NPC>().Frozen = true;
                    TargetSpeed = Target.GetComponent<NavMeshAgent>().speed;
                    Target.GetComponent<NavMeshAgent>().speed = 0;
                    // set the targets Behavior tree to skip the movment node, can do it by adding a state in the Behavior tree that checks the "Frozen" variable in NPC
                    break;
                }
            case Ailments.Stun:
                {
                    Target.GetComponent<NPC>().Stunned = true;
                    Target.GetComponent<BehaviorTree>().DisableBehavior();
                    Target.GetComponent<Animator>().speed = 0;
                    // set the targets Behavior tree to stop taking action, can do it by adding a state in the Behavior tree that checks the "Stunned" variable in NPC
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public override void StopConEffect()
    {
        switch (ailment)
        {
            case Ailments.Burn:
            case Ailments.Poison:
            case Ailments.Curse:
                {
                    break;
                }
            case Ailments.Freeze:
                {
                    Target.GetComponent<NavMeshAgent>().speed = TargetSpeed;
                    Target.GetComponent<NPC>().Frozen = false;
                    break;
                }
            case Ailments.Stun:
                {
                    Target.GetComponent<NPC>().Stunned = false;
                    Target.GetComponent<BehaviorTree>().EnableBehavior();
                    Target.GetComponent<Animator>().speed = 1;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public IEnumerator DmgOvrTim()
    {
        timer = 0;
        while (timer < duration && Target.GetComponent<NPC>().CurrentHP > 0)
        {
            Target.GetComponent<NPC>().TakeDmg(DOT);
            yield return new WaitForSeconds(Hit_Rate);
            timer += Time.deltaTime;
        }
        HurtEverTim = null;
    }
}
