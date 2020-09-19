using UnityEngine;
using Enums;


public class HealingSkill : Skills
{
    private float heal;
    public float speed = 25f;
    public GameObject EfectOnHit;
    public AudioSource HitAudio;
    public HealType HealingType;
    public float StormAoe;
    public float Duraion;
    public float DMGTick;
    
    public float Calc_heal()
    {
        
            heal = TheCaster.GetComponent<SkillUser>().Healing();
            heal += TheCaster.GetComponent<SkillUser>().INT * Main_Stat_Multi;

            return heal;
    }
    
    public override void SkillAbility(GameObject Target)
    {
        if (TheCaster.gameObject.GetComponent<NPC>() != null || Target.GetComponent<NPC>() != null)
        {
            float HealingInTotal = TheCaster.GetComponent<SkillUser>().TotalHealing;
            if (AOE != false)
            {
                Collider[] AlliesAround = Physics.OverlapSphere(Target.transform.position, AOE_Radius);

                for (int i = 0; i < AlliesAround.Length; i++)
                {
                    if (AlliesAround[i].tag != "Untagged" && AlliesAround[i].tag != "Projectile" && AlliesAround[i].tag == TheCaster.tag && AlliesAround[i].tag != "Shop")
                    {
                        float AlliesX = AlliesAround[i].transform.position.x;
                        float AlliesZ = AlliesAround[i].transform.position.z;
                        float AlliesY = AlliesAround[i].transform.position.y;
                        Vector3 IsHit = Target.transform.position;
                        if (EfectOnHit != null)
                            Instantiate(EfectOnHit, IsHit = new Vector3(AlliesX, AlliesY + 1f, AlliesZ), Target.transform.rotation);


                        AlliesAround[i].GetComponent<NPC>().HealTarget(AlliesAround[i].gameObject, Calc_heal());
                        HealingInTotal = HealingInTotal + Calc_heal();


                        if (ToLifeSteel == true)
                        {
                            int ReturnHealth = ((int)heal / 100) * (int)LifeStrealPrecent;

                            TheCaster.GetComponent<SkillUser>().GetHealed(ReturnHealth);


                        }

                    }
                }
            }
            else
            {
                TheCaster.GetComponent<SkillUser>().HealTarget(Target, Calc_heal());
                HealingInTotal = HealingInTotal + Calc_heal();

                if (ToLifeSteel == true)
                {
                    int ReturnHealth = ((int)heal / 100) * (int)LifeStrealPrecent;
                    TheCaster.GetComponent<SkillUser>().GetHealed(ReturnHealth);
                }
            }
        }
    }

    protected override void SpawnObject()
    {
        if (TheCaster.gameObject.GetComponent<NPC>() != null || Target.GetComponent<NPC>() != null)
        {
            switch (HealingType)
            {
                case HealType.Projectile:
                    {
                        GameObject newProjectile = Instantiate(SkillObject, SourceLocation.position, npc.transform.rotation);
                        newProjectile.GetComponent<Projectile>().target = Target;
                        newProjectile.GetComponent<Projectile>().HParent = this;
                        newProjectile.GetComponent<Projectile>().speed = speed;
                        break;
                    }
                case HealType.Strom:
                    {

                        SkillObject.GetComponent<ParticleCollisionInstance>().HParent = this;
                        SkillObject.GetComponent<ParticleCollisionInstance>().Targeter = Target;
                        break;

                    }
                case HealType.LavaGround:
                    {
                        SkillObject.GetComponent<OffanceGroundDmg>().HskillStat = this;
                        SkillObject.GetComponent<OffanceGroundDmg>().father = TheCaster;
                        SkillObject.GetComponent<OffanceGroundDmg>().Duraion = Duraion;
                        SkillObject.GetComponent<OffanceGroundDmg>().DMGTicks = DMGTick;
                        break;
                    }
                case HealType.PricingShot:
                    {
                        SkillObject.GetComponent<PricingProjectile>().speed = speed;
                        SkillObject.GetComponent<PricingProjectile>().hit = EfectOnHit;
                        SkillObject.GetComponent<PricingProjectile>().flash = SpawnParticles;
                        SkillObject.GetComponent<PricingProjectile>().father = TheCaster;
                        SkillObject.GetComponent<PricingProjectile>().healingStat = this;
                        break;
                    }
                case HealType.BouncingProjectile:
                    {
                        SkillObject.GetComponent<BouncingProjectile>().bounceTime = BouncingTimes;
                        SkillObject.GetComponent<BouncingProjectile>().speed = speed;
                        SkillObject.GetComponent<BouncingProjectile>().father = TheCaster;
                        SkillObject.GetComponent<BouncingProjectile>().HealingStat = this;
                        SkillObject.GetComponent<BouncingProjectile>().AreaOfEffect = BouncingSearchAOE;
                        break;

                    }
                case HealType.None:
                    {
                        SkillAbility(Target);
                        break;
                    }
            }
        }
    }
}
