using System.Collections;
using UnityEngine;
using Enums;

public abstract class Skills : MonoBehaviour
{
    public Sprite Icon = null;
    public int Lvl;
    public int Mana_Cost;
    public float Cast_Time;
    public bool AOE;
    public float AOE_Radius;
    public float CoolDown = 1;
    public float Current_CoolDown = 0;
    public float Bullet_CoolDown = 0.1f;
    public int BouncingTimes;
    public float BouncingSearchAOE;
    public bool castOnSelf = false;
    public int ammo = 1;
    public int currentAmmo = 1;
    public bool ToLifeSteel;
    public float LifeStrealPrecent;
    //public float Range;
    public StatType Main_Stat;
    public StatType Second_Stat;
    public float Main_Stat_Multi;
    public float Second_Stat_Multi;
    public GameObject Target;
    public Transform Target_Location;
    public GameObject SkillObject;
    public GameObject SpawnParticles;
    public AudioSource CastAudio;
    protected Transform SourceLocation;
    public GameObject TheCaster;
    public SkillSpawn skillSpawn;
    public string skillDescription;
    
    public SkillUser npc;

    protected abstract void SpawnObject();
    public abstract void SkillAbility(GameObject Target);


    public IEnumerator CountDown(float Timer)
    {
        Current_CoolDown = Timer;
        while (Current_CoolDown > 0)
        {

            yield return new WaitForFixedUpdate();
            Current_CoolDown -= Time.deltaTime;
        }
    }

    public void CastSound()
    {
        if (CastAudio != null)
            Instantiate(CastAudio, TheCaster.transform.position, TheCaster.transform.rotation);
    }


    public void DoSkill(GameObject Target)
    {
        for (int i = currentAmmo; i > 0; i--)
        {



            SkillUser MySpawnPositions = TheCaster.GetComponent<SkillUser>();
            if (castOnSelf == true)
            {
                Target = TheCaster;

            }
            if (Target == null && skillSpawn != SkillSpawn.Summons)
            {
                Destroy(this.gameObject);
            }
            if (Target != null || Target.GetComponent<NPC>() != null)
            {
                float EnemyX = Target.transform.position.x;
                float EnemyZ = Target.transform.position.z;
                float EnemyY = Target.transform.position.y;
                float x = TheCaster.transform.position.x;
                float z = TheCaster.transform.position.z;
                float y = TheCaster.transform.position.y;
                float enemyrotY = Target.transform.rotation.y;
                float enemyrotZ = Target.transform.rotation.z;
                float enemyrotX = Target.transform.rotation.x;
                Vector3 targetPos = Target.transform.position;

                if (SkillObject == null)
                {
                    skillSpawn = SkillSpawn.None;
                }

                switch (skillSpawn) //sets where the skill spawns from
                {
                    case SkillSpawn.FromNPC: //at casting NPC
                        {
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SkillObject.transform.position = new Vector3(x + 1f, y, z), TheCaster.transform.rotation);
                            SourceLocation = TheCaster.transform;
                            MySpawnPositions.skillsSapwn(SkillObject, TheCaster.transform.position, SkillObject.transform);
                            CastSound();
                            break;
                        }
                    case SkillSpawn.TargetPosition: //at trageted NPC
                        {

                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(0);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);
                            SourceLocation = Target.transform;
                            MySpawnPositions.skillsSapwn(SkillObject, targetPos, SkillObject.transform);
                            CastSound();

                            break;
                        }
                    case SkillSpawn.AboveTargetHead: //at trageted NPC
                        {
                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(0);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);
                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(7);
                            MySpawnPositions.skillsSapwn(SkillObject, targetPos = new Vector3(EnemyX, EnemyY + 1f, EnemyZ), Target.transform);
                            CastSound();
                            break;
                        }
                    case SkillSpawn.ShootingPosition: //from casting NPC
                        {

                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(0);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);
                            MySpawnPositions.skillsSapwn(SkillObject, SourceLocation.transform.position, transform.transform);
                            CastSound();
                            break;
                        }
                    case SkillSpawn.RandomSpellSlotSpawn: //from casting NPC
                        {
                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(0);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);
                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(Random.Range(1, 4));
                            MySpawnPositions.skillsSapwn(SkillObject, SkillObject.transform.position = MySpawnPositions.RandomSpellSlotSpawn().transform.position, Target.transform);
                            CastSound();
                            break;
                        }
                    case SkillSpawn.MeteorSpawn: //at trageted NPC
                        {

                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(7);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);
                            MySpawnPositions.skillsSapwn(SkillObject, SourceLocation.transform.position, transform.transform);
                            CastSound();
                            break;

                        }
                    case SkillSpawn.Summons: //from casting NPC
                        {
                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(0);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);

                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(Random.Range(8, 13));
                            MySpawnPositions.skillsSapwn(SkillObject, SourceLocation.transform.position, Target.transform);
                            CastSound();
                            break;

                        }
                    case SkillSpawn.OnMeAura: //from casting NPC
                        {

                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SkillObject.transform.position = new Vector3(x + 1f, y, z), TheCaster.transform.rotation);
                            SourceLocation = npc.transform;
                            GameObject go = Instantiate(SkillObject, TheCaster.transform.position, Quaternion.identity) as GameObject;
                            go.transform.parent = GameObject.Find(TheCaster.name).transform;
                            CastSound();
                            break;

                        }
                    case SkillSpawn.OnTaregetAura: //at trageted NPC
                        {
                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(0);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);
                            SourceLocation = Target.transform;

                            GameObject go = Instantiate(SkillObject, Target.transform.position, Quaternion.identity) as GameObject;
                            go.transform.parent = GameObject.Find(Target.name).transform;
                            CastSound();
                            break;
                        }
                    case SkillSpawn.AuraOnCastingLocation: // at unique location
                        {
                            SourceLocation = MySpawnPositions.SpawnPointSkillLocation(0);
                            if (SpawnParticles != null)
                                Instantiate(SpawnParticles, SourceLocation.transform.position, TheCaster.transform.rotation);

                            SourceLocation = Target.transform;
                            GameObject go = Instantiate(SkillObject, MySpawnPositions.SkillSpawnPosition[0].gameObject.transform.position, Quaternion.identity) as GameObject;
                            go.transform.parent = GameObject.Find(TheCaster.name).transform;
                            CastSound();
                            break;
                        }
                    default: //at trageted NPC
                        {
                            SourceLocation = Target.transform;
                            break;
                        }

                }

                if (SkillObject != null)
                {

                    SpawnObject();

                }
                else
                {   }
                SkillAbility(Target);
                TheCaster.gameObject.GetComponent<NPC>().UseMana(Mana_Cost);
                currentAmmo = currentAmmo - 1;

                if (currentAmmo <= 0)
                {
                    currentAmmo = ammo;
                    TheCaster.GetComponent<SkillUser>().Skill_Reset(this, CoolDown);

                }
                else
                {
                    TheCaster.GetComponent<SkillUser>().Skill_Reset(this, Bullet_CoolDown);
                }
            }
        }
    }
}
