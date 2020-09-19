using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BehaviorDesigner.Runtime;
public abstract class CharEffect : Skills
{
    public enum Considred
    {
        Friendly,
        Malicious
    };

    public double duration; //how long the effect lasts
    [System.NonSerialized]
    public double timer;    //how long the effect has been acitve
    public Considred considred;
    protected List<GameObject> npc_list;
    Behavior behavior;
    public abstract void ExecuteConEffect();
    public abstract void StopConEffect();
    public AudioSource HitAudio;

    public override void SkillAbility(GameObject Target)
    {
        if (AOE == true)
        {
            Collider[] AllAround = Physics.OverlapSphere(transform.position, AOE_Radius);
            foreach (Collider Around in AllAround)
            {
                switch (considred)
                {
                    case Considred.Friendly:
                        {
                            if (Around.tag == TheCaster.tag)
                            {
                                BuffExecute(Around.gameObject);
                                Instantiate(SkillObject, Target.transform.position, Target.transform.rotation);
                            }
                            break;
                        }
                    case Considred.Malicious:
                        {
                            if (Around.tag != TheCaster.tag && Around.tag != "Untagged" && Around.tag != "Projectile" && Around.tag != TheCaster.tag && Around.tag != "Shop")
                            {
                                BuffExecute(Around.gameObject);
                                Instantiate(SkillObject, Target.transform.position, Target.transform.rotation);
                            }
                            break;
                           
                        }
                }
            }
        }
        else
        {
        
            BuffExecute(Target);
        }
       

    }

    public void BuffExecute(GameObject Target)
    {
        if (HitAudio != null)
        {
            Instantiate(HitAudio, Target.transform.position, Target.transform.rotation);
        }
        switch (considred)
        {

            case Considred.Friendly:
                {
                    npc_list = Target.GetComponent<NPC>().Friendly_Effects;
                    break;
                }
            default:
            case Considred.Malicious:
                {
                    npc_list = Target.GetComponent<NPC>().Malicious_Effects;
                    break;
                }
        }
        GameObject temp = npc_list.Find(AttachedSkill => AttachedSkill.name == this.gameObject.ToString());
        if (!temp)
        {

            npc_list.Add(this.gameObject);
            TheCaster.GetComponent<SkillUser>().SkillTimer(this);
            //CountUp();
            ExecuteConEffect();
        }
        else
        {
            temp.GetComponent<CharEffect>().timer = duration;
        }
    }
    
    protected override void SpawnObject()
    {
    }

    public IEnumerator CountUp()
    {
        timer = 0;
        while (timer < duration)
        {

            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        StopConEffect();
        npc_list.Remove(npc_list.Where(obj => obj.name == this.gameObject.ToString()).SingleOrDefault());
    }
}
