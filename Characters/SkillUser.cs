using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillUser : Entity
{

    [Header("hat am I?")]
    public bool isSummon;
    public bool Monster;

    [Header("Stats")]
    public int Lvl;
    public int STR;
    public int DEX;
    public int INT;

    
    [Header("Damage")]
    public float CurrentMinDmg = 0;
    public float CurrentMaxDmg = 0;

    [Header("Crits")]
    protected float BaseCritMulti = 120f;
    protected float BaseCritChance = 0;
    protected float CurrentCritMulti = 0;
    public float CurrentCritChance = 0;
    public float LastCrit = 0;
    
    [Header("Damage Sounds")]
    public AudioSource DealDmgSound;
    public bool MyDmgSound;

    public int TotalHealing;
    
    [Header("Skills")]
    public GameObject[] skills;
    public GameObject[] SkillSpawnPosition;

    //==========================================
    [Tooltip("analytics stats")]
    [Header("Analytics")]
    public Analytics analytics;

    public abstract float Healing();

    //retrun in vectoor 3 the axis of the point reqested
    public Transform SpawnPointSkillLocation(int placement)
    {
        Transform LocationInfo = SkillSpawnPosition[placement].transform;
        return LocationInfo;
    }

    //spawning ther point
    public void skillsSapwn(GameObject skills, Vector3 spawnPlace, Transform rotation)
    {
        Instantiate(skills, spawnPlace, transform.rotation);
    }

    public float RollDmg()
    {
        int MinDmg = (int)CurrentMinDmg;
        int MaxDmg = (int)CurrentMaxDmg;
        return Random.Range(MinDmg, MaxDmg);
    }

    public virtual void DealDmg(GameObject Target, float dmg)// + skills damge 
    {
        Target.GetComponent<Entity>().TakeDmg(dmg);
    }

    public virtual void DealDmg(GameObject Target)// + weapon damge 
    {
        float Dmg = RollDmg();
        if ((CurrentCritChance + (DEX / 2)) >= Random.Range(0, 100))
        {
            float crit = (float)(BaseCritMulti + (DEX * 0.4));
            float reallCrit = crit / 100;

            Dmg = (int)(Dmg * reallCrit);
            LastCrit = Dmg;
        }

        if (DealDmgSound != null)
        {
            if (MyDmgSound == true)
            {
                Instantiate(DealDmgSound, Target.transform.position, transform.rotation);
            }
            else
            {
                Instantiate(DealDmgSound, this.transform.position, transform.rotation);
            }
        }

        if (Target.GetComponent<NPC>() == true)
        {
            Target.GetComponent<NPC>().TakeDmg(Dmg);
        }
        else
        if (Target.GetComponent<Building>() == true)
        {
            Target.GetComponent<Building>().TakeDmg(Dmg);
        }
    }

    public void HealTarget(GameObject Target, float HealAmount)
    {
        Target.GetComponent<NPC>().GetHealed(HealAmount);

        if (isSummon != true && !Monster)
        {
            analytics.OnDataChanged(this.gameObject, "Healing");//analytics
        }
    }
    
    // will use spellslot at random 
    public GameObject RandomSpellSlotSpawn()
    {
        int SpellSlotNum = Random.Range(1, 4);

        return SkillSpawnPosition[SpellSlotNum];
    }

    public void Skill_Reset(int i, float _Timer)
    {
        StartCoroutine(skills[i].GetComponent<Skills>().CountDown(_Timer));
    }

    public void Skill_Reset(Skills skill, float _Timer)
    {
        StartCoroutine(skill.CountDown(_Timer));
    }

    public void SkillTimer(CharEffect Effect)
    {
        StartCoroutine(Effect.CountUp());
    }

    public void DmgOverTimTimer(OffensiveAilment Ailment)
    {
        Ailment.HurtEverTim = StartCoroutine(Ailment.DmgOvrTim());
    }
}
