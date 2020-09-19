using UnityEngine;
using Enums;

public class OffenceSkill : Skills
{
    private float DMG;
    public GameObject Extra_Skill_CharEffect;
    public float speed = 25f;
    public GameObject EfectOnHit;
    public AudioSource HitAudio;
    public OffacneType OffacneType;
    public float StormAoe;
    public float Duraion;
    public float DMGTick;
    public bool LeechSummon;
    public float LeechSummonAmount;

    public float Calc_Dmg()
    {

        if (TheCaster.gameObject.GetComponent<NPC>() != null || Target.GetComponent<NPC>() != null)
        {
            var SkillUser = TheCaster.gameObject.GetComponent<NPC>();
            DMG = SkillUser.RollDmg();
            switch (Main_Stat)
            {
                case StatType.STR:
                    DMG += SkillUser.STR * Main_Stat_Multi;
                    break;
                case StatType.INT:
                    DMG += SkillUser.INT * Main_Stat_Multi;
                    break;
                case StatType.DEX:
                    DMG += SkillUser.DEX * Main_Stat_Multi;
                    break;
                default:
                    break;
            }

            switch (Second_Stat)
            {
                case StatType.STR:
                    DMG += SkillUser.STR * Second_Stat_Multi;
                    break;
                case StatType.INT:
                    DMG += SkillUser.INT * Second_Stat_Multi;
                    break;
                case StatType.DEX:
                    DMG += SkillUser.DEX * Second_Stat_Multi;
                    break;
                default:
                    break;
            }
        }
        
        if (ToLifeSteel == true)
        {
            float ReturnHealth = (DMG / 100) * LifeStrealPrecent;

            Debug.Log(TheCaster.name + " is leeching");
            TheCaster.GetComponent<SkillUser>().GetHealed(ReturnHealth);

        }
        if (LeechSummon == true)
        {
            float ReturnHealth = (DMG / 100) * LeechSummonAmount;

            var healMe = TheCaster.GetComponent<SummonCount>();
            for (int y = 0; y < healMe.summonsList.Count; y++)
            {
                if (healMe.summonsList[y].GetComponent<NPC>() != null)
                {
                    healMe.summonsList[y].GetComponent<NPC>().GetHealed(ReturnHealth);
                }
            }
        }

        return DMG;
    }
    
    public override void SkillAbility(GameObject Target)
    {
        if (Target.GetComponent<Entity>() != null)
        {
            if (AOE == true)
            {

                Collider[] EnemiesAround = Physics.OverlapSphere(Target.transform.position, AOE_Radius);
                for (int i = 0; i < EnemiesAround.Length; i++)
                {
                    if (EnemiesAround[i].tag != "Untagged" && EnemiesAround[i].tag != "Projectile" && EnemiesAround[i].tag != TheCaster.tag && EnemiesAround[i].tag != "Shop" && EnemiesAround[i].gameObject !=  null)
                    {
                        float EnemyX = EnemiesAround[i].transform.position.x;
                        float EnemyZ = EnemiesAround[i].transform.position.z;
                        float EnemyY = EnemiesAround[i].transform.position.y;
                        Vector3 IsHit = Target.transform.position;


                        if (EfectOnHit != null)
                            Instantiate(EfectOnHit, IsHit = new Vector3(EnemyX, EnemyY + 1f, EnemyZ), Target.transform.rotation);

                        EnemiesAround[i].GetComponent<Entity>().IWasHitBy = TheCaster.gameObject;
                        TheCaster.GetComponent<SkillUser>().DealDmg(EnemiesAround[i].gameObject, Calc_Dmg());
                        if (Extra_Skill_CharEffect != null)
                            Extra_Skill_CharEffect.GetComponent<CharEffect>().DoSkill(EnemiesAround[i].gameObject);
                    }
                }
            }
            else
            {
                float EnemyX = Target.transform.position.x;
                float EnemyZ = Target.transform.position.z;
                float EnemyY = Target.transform.position.y;
                Vector3 IsHit = Target.transform.position;

                if (EfectOnHit != null)
                    Instantiate(EfectOnHit, IsHit = new Vector3(EnemyX, EnemyY + 1f, EnemyZ), Target.transform.rotation);

                Target.GetComponent<Entity>().IWasHitBy = TheCaster.gameObject;
                TheCaster.GetComponent<SkillUser>().DealDmg(Target, Calc_Dmg());
                if (Extra_Skill_CharEffect != null)
                {
                    Extra_Skill_CharEffect.GetComponent<CharEffect>().DoSkill(Target);
                }
            }
        }
    }
    
    protected override void SpawnObject()
    {
        if (Target.GetComponent<Entity>() != null || npc != null)
        {

            switch (OffacneType)
            {
                case OffacneType.Projectile:
                    {
                        GameObject newProjectile = Instantiate(SkillObject, SourceLocation.position, TheCaster.transform.rotation);
                        newProjectile.GetComponent<Projectile>().father = TheCaster;
                        newProjectile.GetComponent<Projectile>().target = Target;
                        newProjectile.GetComponent<Projectile>().Parent = this;
                        newProjectile.GetComponent<Projectile>().speed = speed;
                        break;
                    }
                case OffacneType.Strom:
                    {

                        Debug.Log("this is the strom");
                        SkillObject.GetComponent<ParticleCollisionInstance>().Parent = this;
                        SkillObject.GetComponent<ParticleCollisionInstance>().Targeter = Target;
                        break;

                    }
                case OffacneType.LavaGround:
                    {
                        SkillObject.GetComponent<OffanceGroundDmg>().DMGskillStat = this;
                        SkillObject.GetComponent<OffanceGroundDmg>().father = TheCaster;
                        SkillObject.GetComponent<OffanceGroundDmg>().Duraion = Duraion;
                        SkillObject.GetComponent<OffanceGroundDmg>().DMGTicks = DMGTick;
                        break;
                    }
                case OffacneType.PricingShot:
                    {
                        SkillObject.GetComponent<PricingProjectile>().speed = speed;
                        SkillObject.GetComponent<PricingProjectile>().hit = EfectOnHit;
                        SkillObject.GetComponent<PricingProjectile>().flash = SpawnParticles;
                        SkillObject.GetComponent<PricingProjectile>().father = TheCaster;
                        SkillObject.GetComponent<PricingProjectile>().OffanceStat = this;
                        break;
                    }
                case OffacneType.BouncingProjectile:
                    {
                        SkillObject.GetComponent<BouncingProjectile>().bounceTime = BouncingTimes;
                        SkillObject.GetComponent<BouncingProjectile>().speed = speed;
                        SkillObject.GetComponent<BouncingProjectile>().father = TheCaster;
                        SkillObject.GetComponent<BouncingProjectile>().offanceStat = this;
                        SkillObject.GetComponent<BouncingProjectile>().target = Target;
                        SkillObject.GetComponent<BouncingProjectile>().AreaOfEffect = BouncingSearchAOE;
                        break;

                    }
                case OffacneType.None:
                    {
                        SkillAbility(Target);
                        break;
                    }
                case OffacneType.Charge:
                    {

                        SkillObject.GetComponent<Charge>().father = TheCaster;
                        SkillObject.GetComponent<Charge>().target = Target;
                        SkillObject.GetComponent<Charge>().Parent = this;
                        break;
                    }
            }
        }
        else
        {
            Destroy(this.gameObject);

        }

    }
}