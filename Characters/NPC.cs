using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using RandomNameGen;

public class NPC : SkillUser
{

    public int MaxMana = 0;
    public float CurrentMana = 0;
    public int ManaMulti = 1;
    [Header("Beef Meeter")]
    public int healthMulti;

    [Header("Self Definition")]
    public string reraty;
    public string CharName;
    public Sprite npcIcon;
    public string className;

    [Header("Main Stats")]
    [Header("Main Stats")]
    public int BaseSTR;
    public int BaseDEX;
    public int BaseINT;
    public int Bravery;
    public int itemsLvlAvg;


    [Header("Role In the World")]
    public SummonType SummonType;
    public bool IsBoss;
    public bool Worker;
    public bool Guard;
    public bool Merchent;

    [Header("Sound")]
    public SummoningSkill SummonStats;
    public AudioSource[] takeingDmgSound;
    public AudioSource dieSound;
    public AudioSource BuildingSound;


    public int Gold;

    [Header("Equipment And Preff")]
    public PlayerRole Rank; //change values in Enums script, at Role to change the types of Roles
    public ClassType classType;//---------------------------------------------------------------------------------------------------
    public StatType Prefference;
    public StatType PrefferenceSecond; // second stat highest for equipping descions or can be none (any second  stat fits)
    public TypeOfWeapons WeaponPref;
    public ArmourType armourPref;

    [Header("Leveling")]
    public int XP;
    public int XpNeededToLvl;
    public int XpOnDeath;
    public int PrevLvlXP;
    private float BaseDmg = 0;

    [Header("Secondery Stats")]
    public int BaseMaxMana = 0;

    public int ManaRegen = 0;
    public int Defence = 0;
    public string Guild;

    public List<GameObject> Friendly_Effects = new List<GameObject>();
    public List<GameObject> Malicious_Effects = new List<GameObject>();

    //Parts for detection sphere for Distirbution of XP and Gold
    public float radius = 7;

    [Header("Party")]
    public int maxPartySize = 10;
    public GameObject myLeader;
    public int charisma;
    public int partyIControl;
    public bool AmIPL;
    //public List<NPC> partyMembers;


    [Header("Resource")]
    public int harvestResource;
    public int MaxCarry;

    public int potionsHave;
    public int targetLvl;
    public Gender gender;
    public PlayerRole playerRole;
    public string MyEnemy;

    [Header("Locations")]
    public GameObject retreat;
    public GameObject conquest;
    public GameObject Quest;


    [Header("Inventory")]
    public Inventory inventory;
    public EquipmentPanel equipmentPanel;
    public StatPanel statPanel;
    public PotionPanel potionPanel;

    [Header("UI Data")]
    public CharData charData;
    public skillsPanel skillsPanel;

    [Header("Bars")]
    public UpperBar upper;
    public Canvas canvas;
    public CanvasGroup cg;
    public Click click;
    public CanvasGroup cgInventory;
    public event System.Action<float, int> OnHealthChanged;
    public event System.Action<float, float> OnManaChanged;
    public event System.Action<int, int> OnExpChanged;


    [Header("State")]
    public bool dualWeapons;
    public bool Looted;
    public bool TwoHanded;// for items equipment test the weapon

    // public event System.Action<GameObject, string> OnDataChanged;
    [Header("Regeneration")]
    public float LastHealTaken;

    public Coroutine Regen;
    public bool RegenOn = false;
    public Coroutine Mana_Regen;
    public bool Mana_RegenOn = false;
    public GameObject lvlup;
    public GameObject lvlupEffect;
    //||============================================

    public int Kills;
    public int TotalGold;
    public int TotalObjectives;

    [Header("Status Effect")]
    public bool Frozen = false;
    public bool Stunned = false;

    [Header("Income Multis")]
    public int XPMultiplier = 1;
    public int GoldMultiplier = 1;


    void Awake()// Start is called before the first frame update
    {

        if (!isSummon && !Monster && !Guard && !Worker && !Merchent)
        {

            //   CharName= RandomName.NPCName(gender); //generate npc name
            //  CharName = randomName(gender);
            RandomName r = new RandomName();
            //  RandomName r = GetComponent<RandomName>();
            CharName = r.NPCName(gender);

            //  CharName = "Bugass";
            analytics = FindObjectOfType<Analytics>();
            analytics.addNPC(this.gameObject);
            // Gold = 1300;
        }

    }
    private void Start()
    {

        HP = Lvl * 20;
        CurrentHP = HP;
        Defence = 1;
        //if (Monster == true || isSummon == true)
        //{
        //    Defence = Lvl * 7;
        //}

        if (isSummon)
        {
            STR = 0;
            DEX = 0;
            INT = 0;
        }
        UpdateAllStats(BaseSTR, BaseDEX, BaseINT);

        CurrentCritMulti = BaseCritMulti;
        CurrentCritChance = BaseCritChance;
        CurrentMana = MaxMana;



        if ((isSummon || Monster || Worker || Guard || Merchent) && FatherBuilding == null && MyKingdom == null)
        {
            TakeDmg(CurrentHP * 100);
            Die();

        }
        if (!isSummon)
        {
            UpdateEXP();
        }
        if (Monster)
        {
            AddMonsterToBuilding(this.reraty);


        }


        if (Monster == true && FatherBuilding == null)
        {
            Destroy(this.gameObject);
        }
        if (isSummon && FatherBuilding != null)
        {
            FatherBuilding.GetComponent<SummonCount>().summonsList.Add(this.gameObject);

        }
;

        if (!isSummon && !Monster && !Worker && !Guard && !Merchent)
        {

            AmIPL = false;
            Guild = "Not a Guild Memeber";
            CloseDisplay();
            PrevLvlXP = 0;
            XpNeededToLvl = 10;
            Looted = false;
            if (Lvl != 1)
            {
                UpdateEXP();
            }
            //analytics total
            MyDmgDealt = 0;
            Kills = 0;
            TotalGold = 0;
            TotalObjectives = 0;
            TotalHealing = 0;
        }
        if (isSummon)
        {
            FatherBuilding = SummonStats.TheCaster.gameObject;
            SummonStats.SkillAbility(this.gameObject);
            CharName = SummonType.ToString();

            if (FatherBuilding.tag != this.gameObject.tag)
            {

                SummonCounReduction();
                Destroy(this.gameObject);

            }

            if (FatherBuilding == null)
            {

                SummonCounReduction();
                Destroy(this.gameObject);
            }





            foreach (GameObject skill in skills)
            {
                skill.GetComponent<Skills>().npc = this;
            }

            //auto skill reset incase skills saves theyre cds
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i].GetComponent<Skills>().Current_CoolDown > 0)
                {
                    skills[i].GetComponent<Skills>().Current_CoolDown = 0;

                }
                if (skills[i].GetComponent<Skills>().currentAmmo <= 0)
                {
                    skills[i].GetComponent<Skills>().currentAmmo = skills[i].GetComponent<Skills>().ammo;
                }

            }
        }
    }

    public void AddMonsterToBuilding(string Reraty)
    {
        if (FatherBuilding != null)
        {
            Gold = 0;
            TotalGold = 0;
            int GoldWorth = 0;
            switch (Reraty)
            {
                case "common":
                    {
                        GoldWorth = Lvl * (Random.Range(1, 5) * 2);
                        FatherBuilding.GetComponent<MonstersDen>().CurrentCommons++;
                        Gold = GoldWorth;
                        RecieveGold(GoldWorth);
                        break;
                    }
                case "rare":
                    {
                        GoldWorth = Lvl * (Random.Range(3, 7) * 2);
                        FatherBuilding.GetComponent<MonstersDen>().CurrentRares++;
                        Gold = GoldWorth;
                        RecieveGold(GoldWorth);
                        break;
                    }
                case "elite":
                    {
                        GoldWorth = Lvl * (Random.Range(4, 10) * 2);
                        FatherBuilding.GetComponent<MonstersDen>().CurrentElits++;
                        Gold = GoldWorth;
                        RecieveGold(GoldWorth);
                        break;
                    }
                case "boss":
                    {
                        GoldWorth = Lvl * (Random.Range(10, 15) * 2);
                        FatherBuilding.GetComponent<MonstersDen>().CurrentBosses++;
                        Gold = GoldWorth;
                        RecieveGold(GoldWorth);
                        break;
                    }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void removeMonsterToBuilding()
    {
        if (FatherBuilding != null)
        {


            switch (reraty)
            {
                case "common":
                    {
                        FatherBuilding.GetComponent<MonstersDen>().CurrentCommons--;
                        break;
                    }
                case "rare":
                    {
                        FatherBuilding.GetComponent<MonstersDen>().CurrentRares--;
                        break;
                    }
                case "elite":
                    {
                        FatherBuilding.GetComponent<MonstersDen>().CurrentElits--;
                        break;
                    }
                case "boss":
                    {
                        FatherBuilding.GetComponent<MonstersDen>().CurrentBosses--;
                        break;
                    }
            }
        }
    }

    public MerchantType ReturnMerchentAnItemType()
    {

        return equipmentPanel.getLowestLvl();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RecieveXP(100);
        }
    }

    public int getItemsLvlAvg()
    {
        itemsLvlAvg = equipmentPanel.Avg();
        return itemsLvlAvg;
    }

    public void UpdateEXP()//invoked when npc is lvled up, update once per lvl
    {
        XpNeededToLvl = (10 * Lvl) * (5 * Lvl);
        XP = XpNeededToLvl;
    }

    public void LvlUp(int LeftOversXP)
    {
        if (Lvl < 51)
        {
            Lvl = Lvl + 1;
            GameObject go = Instantiate(lvlupEffect, this.transform.position, this.transform.rotation);
            go.transform.parent = GameObject.Find(this.name).transform;
            GameObject lvlupt = Instantiate(lvlupEffect) as GameObject;
            lvlupt.transform.SetParent(lvlup.transform, false);
            OnExpChanged?.Invoke(XP, 0);
            //CurrentHP = HP;
            CurrentMana = MaxMana;
            OnHealthChanged?.Invoke(HP, HP);
            OnManaChanged?.Invoke(MaxMana, MaxMana);
            if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
            {
                if (isMyPanelOpen())
                {
                    charData.updateText();
                    upper.OnXPChanged(XP, 0);
                }
            }

            if (LeftOversXP < 0)
            {
                LeftOversXP = LeftOversXP * (-1);
            }
            XPMultiplier = 0;
            UpdateAllStats(Lvl, Lvl, Lvl);
            UpdateEXP();
            //update stats here too 
            if (LeftOversXP > 0)
                RecieveXP(LeftOversXP);
        }
    }

    public void RecieveXP(int GetXP)
    {
        XPMultiplier += GetXP;
        XpNeededToLvl = XpNeededToLvl - GetXP;
        //OnExpChanged?.Invoke(XP, XPMultiplier);//xp, getxp
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
                upper.OnXPChanged(XP, XPMultiplier);
            }
        }

        if (XpNeededToLvl <= 0)
        {
            LvlUp(XpNeededToLvl);
        }
    }


    public int XPWorth()
    {

        int XpOnDeath = (int)(((10 * Lvl * 2) * 10) + 1);

        if (Monster == true)
        {
            XpOnDeath = (int)(((10 * Lvl * 2) * 5) / 3) + 1;

        }
        if (IsBoss == true)
        {
            XpOnDeath = (int)(((10 * Lvl * 2) * 25) * 3) + 1;
        }



        //  Debug.Log("my xp befor deviding "+ XpOnDeath);
        return XpOnDeath;
    }

    public void RecieveGold(float ReceivedGold)
    {
        Gold *= GoldMultiplier;
        Gold = Gold + (int)ReceivedGold;
        TotalGold = TotalGold + Gold;
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
            analytics.OnDataChanged(this.gameObject, "Gold");//analytics
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
            }
        }
    }

    public bool SpendGold(int GoldToSpend)
    {
        if (Gold - GoldToSpend <= 0)
        {
            return false;
        }
        else
        {
            Gold = Gold - GoldToSpend;
            if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
            {
                if (isMyPanelOpen())
                {
                    charData.updateText();
                }
            }
            return true;
        }
    }

    public void UpdateStats(int strStat, int dexStat, int intStat, int def, int minD, int maxD)
    {
        STR += strStat;
        DEX += dexStat;
        INT += intStat;
        Defence += def;
        CurrentMinDmg += minD;
        CurrentMaxDmg += maxD;
        if (strStat != 0)
        {
            UpdateStatSTR();
        }
        if (dexStat != 0)
        {
            UpdateStatDEX();
        }
        if (intStat != 0)
        {
            UpdateStatINT();
        }
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                statPanel.UpdateStatValues();
            }
        }
    }


    public void UpdateAllStats(int strStat, int dexStat, int intStat)
    {
        STR += strStat;
        DEX += dexStat;
        INT += intStat;
        UpdateStatSTR();
        UpdateStatDEX();
        UpdateStatINT();

        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                statPanel.UpdateStatValues();
            }
        }

    }

    public bool TestHP()
    {
        return true;
    }

    public void healPotion()
    {
        if (TestHP())//if npc has less then 40 precent hp && atleast 1 potion
        {
            if (potionPanel.RemovePotion())
            {
                GetHealed((HP / 100) * 40);
            }
        }
    }

    public void SummonCounReduction()
    {
        Destroy(this.gameObject);
    }

    private void UpdateStatSTR()
    {
        int NewHP = HP + STR * healthMulti;
        if (NewHP > HP)
            CurrentHP += NewHP - HP;
        //Debug.Log();
        HP = NewHP;
        if (CurrentHP > HP)
            CurrentHP = HP;

        CurrentMinDmg = BaseDmg + STR;
        CurrentMaxDmg = BaseDmg + STR * 2;
    }
    private void UpdateStatDEX()
    {
        float BaseCritMultiTemp = DEX * 0.4f;
        float BaseCritChanceTemp = DEX * 0.3f;
        BaseCritMulti = BaseCritMulti + BaseCritMultiTemp;
        BaseCritChance = BaseCritChance + BaseCritChanceTemp;
    }
    private void UpdateStatINT()
    {
        int NewMana = (BaseMaxMana + INT * 3) * ManaMulti;
        if (NewMana > MaxMana)
            CurrentMana += NewMana - MaxMana;
        MaxMana = NewMana;
        if (CurrentMana > MaxMana)
            CurrentMana = MaxMana;

        ManaRegen = INT;
        Call_Mana_regen();
    }
    //=========================================================================================


    public override float Healing()
    {
        float heal;
        bool criticalHeal = false;
        float healing = Random.Range(0.0F, (float)(CurrentMaxDmg - CurrentMinDmg));
        if (Random.Range(0.0F, 100.0F) < BaseCritChance) { criticalHeal = true; };

        if (!criticalHeal)
        {
            heal = healing + (float)(INT);
        }
        else
        {
            heal = (healing / 100.0F) * (float)(BaseCritMulti);
        }

        return (float)(heal);
    }
    //=========================================================================================

    public override void TakeDmg(float Dmg)
    {
        int num = Random.Range(0, 3);
        if (num >= 2 && takeingDmgSound.Length < 0)
        {
            int ran = Random.Range(0, takeingDmgSound.Length);
            Instantiate(takeingDmgSound[ran], this.gameObject.transform.position, transform.rotation);
        }

        float BlockValue = ((float)Defence / ((float)Lvl) / 2.0F);
        float DmgDealt = Dmg - (BlockValue / 3);

        if (FloatingtextPrefab)
        {
            ShowFloatingtext();
        }
        if (DmgDealt <= 0)
        {
            DmgDealt = 0;

        }

        CurrentHP -= DmgDealt;
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
            }
        }
        LastDmgTaken = (int)DmgDealt;
        MyDmgDealt += (int)DmgDealt;


        if (!isSummon && !Monster && !Worker && !Guard && !Merchent)
        {
            analytics.OnDataChanged(this.gameObject, "Damage Dealt");//analytics
        }

        if (IWasHitBy != null)
        {
            float MyDmgDealt = IWasHitBy.GetComponent<NPC>().MyDmgDealt + DmgDealt;
        }

        if (CurrentHP <= 0)
        {

            IsDead = true;
            if (Worker || Guard || Merchent)
            {
                FatherBuilding.GetComponent<TownHall>().RemoveFromWork(this.gameObject);
            }

            if (Monster == true)
            {
                StartCoroutine(Die());
                removeMonsterToBuilding();
            }
         //   MyKingdom.GetComponent<Kingdom>().agents--;
            if (dieSound != null)
            {
                Instantiate(dieSound, this.gameObject.transform.position, transform.rotation);
            }

        }
        else if ((CurrentHP < HP) && (IsDead == false) && (RegenOn == false))
        {
            RegenOn = true;
            Regen = StartCoroutine(Regeneration());
        }

    }
    public override void TrueTakeDmg(float Dmg)
    {
        int num = Random.Range(0, 3);
        if (num >= 2 && takeingDmgSound.Length < 0)
        {
            int ran = Random.Range(0, takeingDmgSound.Length);
            Instantiate(takeingDmgSound[ran], this.gameObject.transform.position, transform.rotation);
        }

        float DmgDealt = Dmg ;

        if (FloatingtextPrefab)
        {
            ShowFloatingtext();
        }
        if (DmgDealt <= 0)
        {
            DmgDealt = 0;

        }

        CurrentHP -= DmgDealt;
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
            }
        }
        LastDmgTaken = (int)DmgDealt;
        MyDmgDealt += (int)DmgDealt;


        if (isSummon == false && !Monster)
        {
            analytics.OnDataChanged(this.gameObject, "Damage Dealt");//analytics
        }

        if (IWasHitBy != null)
        {
            float MyDmgDealt = IWasHitBy.GetComponent<NPC>().MyDmgDealt + DmgDealt;
        }

        if (CurrentHP <= 0)
        {

            IsDead = true;
            if (Worker || Guard || Merchent)
            {
                FatherBuilding.GetComponent<TownHall>().RemoveFromWork(this.gameObject);
            }

            if (Monster == true)
                StartCoroutine(Die());
            removeMonsterToBuilding();
            if (dieSound != null)
            {
                Instantiate(dieSound, this.gameObject.transform.position, transform.rotation);
            }

        }
        else if ((CurrentHP < HP) && (IsDead == false) && (RegenOn == false))
        {
            RegenOn = true;
            Regen = StartCoroutine(Regeneration());
        }

    }

    public void Revived()
    {
        OnHealthChanged?.Invoke(HP, HP);
        OnManaChanged?.Invoke(MaxMana, MaxMana);
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
            }
        }
    }


    //instantiate for floating damage text
    public void ShowFloatingtext()
    {
        var go = Instantiate(FloatingtextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = LastDmgTaken.ToString();
    }

    void ShowFloatingHealingtext()

    {
        var go = Instantiate(FloatingHealingTextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = LastHealTaken.ToString();
    }





    public virtual void BuildUpBuilding(GameObject Target)// + weapon damge 
    {
        float Dmg = RollDmg();
        if ((CurrentCritChance + (DEX / 2)) >= Random.Range(0, 100))
        {
            float crit = (float)(BaseCritMulti + (DEX * 0.4));
            float reallCrit = crit / 100;

            Dmg = (int)(Dmg * reallCrit);
            LastCrit = Dmg;
        }

        if (BuildingSound != null)
        {
            if (MyDmgSound == true)
            {
                Instantiate(BuildingSound, Target.transform.position, transform.rotation);
            }
            else
            {
                Instantiate(BuildingSound, this.transform.position, transform.rotation);
            }
        }

        if (Target.GetComponent<Building>() == true)
        {
            Target.GetComponent<Building>().BuildUp(Dmg);
        }
    }

    //=========================================================================================

    public void ManaTheTarget(GameObject Target, float ManaAmount)
    {
        Target.GetComponent<NPC>().GetMana(ManaAmount);
    }

    public void HealTargetPotion()
    {
        GetHealed((75.0F * (float)HP) / 100.0F);
    }

    public void ManaTheTargetPotion()
    {
        GetMana((75.0F * (float)MaxMana) / 100.0F);
    }

    public override void GetHealed(float HealAmount)
    {

        if (FloatingHealingTextPrefab)
        {
            ShowFloatingHealingtext();
        }
        float NewHP = CurrentHP + HealAmount;

        if (NewHP > HP)
            CurrentHP = HP;
        else
            CurrentHP = NewHP;

        //OnHealthChanged?.Invoke(CurrentHP, HP);
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
            }
        }
        LastHealTaken = (int)HealAmount;

        TotalHealing += (int)HealAmount;

    }
    //======================================================use for skills=======================================
    public void GetMana(float ManaAmount)
    {
        float NewMana = CurrentMana + ManaAmount;
        if (NewMana > MaxMana)
            CurrentMana = MaxMana;
        else
            CurrentMana = NewMana;
        //OnManaChanged?.Invoke(MaxMana, CurrentMana);
        if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
            }
        }
    }

    public bool UseMana(float ManaAmount)
    {
        if (CurrentMana - ManaAmount >= 0)
        {
            CurrentMana = CurrentMana - ManaAmount;
            //OnManaChanged?.Invoke(MaxMana, CurrentMana);
            if ((Monster == false && isSummon == false && Guard == false && Worker == false && Merchent == false))
            {
                if (isMyPanelOpen())
                {
                    charData.updateText();
                }
            }
            return true;
        }
        return false;
    }

    //=========================================================================================

    public IEnumerator Regeneration()
    {
        yield return new WaitForSeconds(5);
        while ((IsDead == false) && (CurrentHP < HP))
        {
            HealTarget(this.gameObject, HP * 0.03f);
            ManaTheTarget(this.gameObject, (MaxMana * 0.03f) + (INT / 2));
            //OnManaChanged?.Invoke(MaxMana, CurrentMana);
            if ((Monster == false && isSummon == false && Guard == false && Worker ==  false && Merchent ==  false))
            {
                if (isMyPanelOpen())
                {
                    charData.updateText();
                }
            }
            yield return new WaitForSeconds(5);
        }
        RegenOn = false;
    }

    public void Call_Mana_regen()
    {
        if (!Mana_RegenOn)
        {
            Mana_Regen = StartCoroutine(Mana_Regeneration());
            Mana_RegenOn = true;
        }
    }

    private IEnumerator Lvl_up()
    {
        lvlup.SetActive(true);
        yield return new WaitForSeconds(2);
        lvlup.SetActive(false);
    }

    private IEnumerator Mana_Regeneration()
    {
        yield return new WaitForSeconds(5);
        while ((IsDead == false) && (CurrentMana < MaxMana))
        {
            CurrentMana += ManaRegen * Time.deltaTime;
            yield return new WaitForSeconds(5);
        }

        if (CurrentMana > MaxMana)
            CurrentMana = MaxMana;
        //OnManaChanged?.Invoke(MaxMana, CurrentMana);
        if ((Monster == false && isSummon == false && Guard == false && Worker ==  false && Merchent ==  false))
        {
            if (isMyPanelOpen())
            {
                charData.updateText();
            }
        }
        Mana_RegenOn = false;
    }
    //=========================================================================================
    private void DropBag()
    {
        //When NPC dies, all items that have not been looted will move to a bag which is dropped in place of the NPC
    }

    public void CashOut()
    {
        //Add function to spread all the bounty that was on the NPC to other characters
    }

    public override IEnumerator Die()
    {
        if (IWasHitBy != null)
        {
            IWasHitBy.GetComponent<NPC>().Kills++;
        }

        // to do: fade out
        yield return new WaitForSeconds(timeUntilDisappear);
        if (FatherBuilding != null)
        {
            this.gameObject.transform.position = new Vector3(FatherBuilding.transform.position.x, 0, FatherBuilding.transform.position.z);
        }

        if (!Monster && !isSummon)
            analytics.removeNPC(this.gameObject);

        Destroy(this.gameObject);


    }

    //=========================================================================================
    
    public void ChangeDisplayStatus()
    {
        canvas.gameObject.SetActive(true);
        // upper.GetComponent<UpperBar>();
        statPanel.UpdateStatValues();
        equipmentPanel.updateList();
        inventory.RefreshUI2();
        skillsPanel.RefreshUI();
        upper.OnXPChanged(XpNeededToLvl, 0);
        charData.updateText();
        canvas.targetDisplay = 0;
        canvas.sortingOrder = 1;
        //  cg.alpha = 1;
        // cgInventory.alpha = 1;
    }

    public void CloseDisplay()
    {
        canvas.targetDisplay = 1;
        canvas.sortingOrder = 0;
        cg.alpha = 0;
        cgInventory.alpha = 0;
        canvas.gameObject.SetActive(false);
    }

    public bool isMyPanelOpen()
    {
        if (canvas.isActiveAndEnabled)
        {
            return true;
        }
        return false;
    }

    public void OpenEquip()
    {
        cg.alpha = 1;
    }

    public void OpenInventory()
    {
        cgInventory.alpha = 1;
    }

    public void CloseEquip()
    {
        cg.alpha = 0;
    }

    public void CloseInventory()
    {
        cgInventory.alpha = 0;
    }

}

