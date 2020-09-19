using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
using System.Linq;
using System.Collections;

public class GameMaster : MonoBehaviour
{
    private List<FactionsTag> AvailableColors = Enum.GetValues(typeof(FactionsTag)).Cast<FactionsTag>().ToList();
    public int TotalPlayersCount;
    public int PlayerCount;
    public int AICount;
    public GameObject KingdomTemplate;
    public GameObject TownHallTemplate;
    public GameObject MonsterKingdom;
    public GameObject OrcKingdom;
    public GameObject BanditKingdom;
    public GameObject HiveKingdom;
    public GameObject UndeadsKingdom;
    public List<GameObject> PreMadeKingdoms;
    public List<GameObject> AllKingdoms;
    public Transform[] allObjects;
    public List<Transform> AllShops;
    public GameObject[] gameObjectss;
    public List<GameObject> conquestPoints;
    public int congOfBlue = 0;
    public int congOfMonster = 0;
    public List<GameObject> TownHall_BuildSpot;
    public static bool Started = false;

    public GameObject RelationsTestKingdom1;
    public GameObject RelationsTestKingdom2;

    private int tempPlayerCount = 0;
    private int tempAICount = 0;
    private int temp = 0;


    private int temp2 = 0;
    //Map Objects To spawn
    public GameObject wildeMagic;
    public GameObject mine;
    public GameObject magicMine;
    public List<GameObject> Lands;


    private static readonly int NumOfFactions = FactionsTag.GetValues(typeof(FactionsTag)).Length;
    public static float[,] Relations;

    private void Awake()
    {
        
    }

    public void Start()
    {
        AtStart();
        //   StartCoroutine(StartGrowth());
        InvokeRepeating("StartGrowth", 1, 3);

    }

    public void StartGrowth()
    {

        if (temp2 < Lands.Count)
        {
            var currentLand = Lands[temp2].GetComponent<TreeLocationSeecker>();

            if ((PlayerCount) > tempPlayerCount)
            {
                currentLand.TownAndChestSapwner(true, true);
                tempPlayerCount++;
            }else if (( AICount) > tempAICount)
            {

                currentLand.TownAndChestSapwner(true, true);
                tempAICount++;
            }else
            {
                currentLand.TownAndChestSapwner(false, false);
            }
            currentLand.ProcaseStart();

        }
        else
        {

            for (int i = 0;  i < PreMadeKingdoms.Count; i++)
            {
               AllKingdoms.Add(PreMadeKingdoms[i]);
            }
            KingdomsRelationsBoot();
            CancelInvoke("StartGrowth");
        }
        temp2++;  
    }

    public void AtStart()
    {
        if (!Started)
        {
            KingdomTemplate.GetComponent<Kingdom>().Gold = SceneBootData.GetGold();
            PlayerCount = SceneBootData.GetPlayerCount();
            AICount = SceneBootData.GetBOTCount();
            TotalPlayersCount = PlayerCount + AICount + PreMadeKingdoms.Count + 1;
            ConnectKingdom(MonsterKingdom, FactionsTag.Monster);
            Started = true;
        }
    }

    public void ConnectKingdom(GameObject KingdomToConnect, bool RanColor)
    {
        int TotalKingdoms = AllKingdoms.Count;
        KingdomToConnect.GetComponent<Faction>().Player_number = TotalKingdoms;
        AllKingdoms.Add(KingdomToConnect);
        if (RanColor == true)
        {
            int r = new System.Random().Next(AvailableColors.Count);
            KingdomToConnect.GetComponent<Faction>().FactionTag = AvailableColors[r];
            KingdomToConnect.name = AvailableColors[r].ToString() + "Faction";
            AvailableColors.Remove(AllKingdoms[TotalKingdoms].GetComponent<Faction>().FactionTag);
        }
        KingdomToConnect.GetComponent<Faction>().gameObject.tag = KingdomToConnect.GetComponent<Faction>().FactionTag.ToString();
    }

    public void ConnectKingdom(GameObject KingdomToConnect, FactionsTag color)
    {
        if (AvailableColors.Contains(color))
        {
            KingdomToConnect.GetComponent<Faction>().FactionTag = color;
            KingdomToConnect.name = color.ToString() + "Faction";
            AvailableColors.Remove(color);
            ConnectKingdom(KingdomToConnect, false);
        }
        else
        {
            ConnectKingdom(KingdomToConnect, true);
        }
    }

    public void CreateKingdom() //in a case of non player faction - disable ui here 
    {
        GameObject NewKingdom = Instantiate(KingdomTemplate);ConnectKingdom(NewKingdom, true);
    }

    public void CreateKingdom(GameObject FirstTownHall) //in a case of non player faction - disable ui here 
    {
        GameObject NewKingdom = KingdomTemplate;
        GameObject king = Instantiate(NewKingdom);
        king.GetComponent<Kingdom>().TownHalls.Clear();
        king.GetComponent<Kingdom>().TownHalls.Add(FirstTownHall);
        if (AllKingdoms.Count()<= PlayerCount) 
            king.GetComponent<Kingdom>().amIRealPlayer = true;


        ConnectKingdom(king, true);
    }

    private void KingdomsRelationsBoot()
    {
        if (Relations == null)
        {
            Relations = new float[TotalPlayersCount, TotalPlayersCount];
        }
        for (int i = 0; i < TotalPlayersCount; i++)
        {
            for (int j = 0; j < TotalPlayersCount; j++)
            {
                if (i == j)
                {

                    Relations[i, j] = 100;
                }
                else
                {
                    if (i == 0 || j == 0)
                    {
                        if (Relations[i, j] != -100)
                        {
                            RelationsShift(AllKingdoms[i].GetComponent<Faction>(), AllKingdoms[j].GetComponent<Faction>(), -100);
                        }
                    }
                    else if (AllKingdoms[i].GetComponent<Faction>().ResetRelations || AllKingdoms[j].GetComponent<Faction>().ResetRelations)
                    {
                        RelationsShift(AllKingdoms[i].GetComponent<Faction>(), AllKingdoms[j].GetComponent<Faction>(), -1 * Relations[i, j]);
                    }
                }
            }
        }
    }

    public static void RelationsShift(Faction PlayerA, Faction PlayerB, float amount)
    {
        float before_change = Relations[PlayerA.Player_number, PlayerB.Player_number];
        Relations[PlayerA.Player_number, PlayerB.Player_number] += amount;
        Relations[PlayerB.Player_number, PlayerA.Player_number] += amount;
        float after_change = Relations[PlayerA.Player_number, PlayerB.Player_number];
        if (before_change > -50 && after_change <= -50)
        {
            PlayerA.MyEnemies.Add(PlayerB.FactionTag.ToString());
            PlayerB.MyEnemies.Add(PlayerA.FactionTag.ToString());
        }
        else if (before_change <= -50 && after_change > -50)
        {
            PlayerA.MyEnemies.Remove(PlayerB.FactionTag.ToString());
            PlayerB.MyEnemies.Remove(PlayerA.FactionTag.ToString());
        }

        if (before_change < 50 && after_change >= 50)
        {
            PlayerA.MyAllies.Add(PlayerB.FactionTag.ToString());
            PlayerB.MyAllies.Add(PlayerA.FactionTag.ToString());
        }
        else if (before_change >= 50 && after_change < 50)
        {
            PlayerA.MyAllies.Remove(PlayerB.FactionTag.ToString());
            PlayerB.MyAllies.Remove(PlayerA.FactionTag.ToString());
        }

        if (after_change < -100)
        {
            Relations[PlayerA.Player_number, PlayerB.Player_number] = -100;
            Relations[PlayerB.Player_number, PlayerA.Player_number] = -100;
            after_change = -100;
        }
        else if (after_change > 100)
        {
            Relations[PlayerA.Player_number, PlayerB.Player_number] = 100;
            Relations[PlayerB.Player_number, PlayerA.Player_number] = 100;
            after_change = 100;
        }
    }

    public float RelationsValueCheck(Kingdom PlayerA, Kingdom PlayerB)
    {
        return Relations[PlayerA.Player_number, PlayerB.Player_number];
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) && RelationsTestKingdom1 && RelationsTestKingdom2)
            RelationsShift(RelationsTestKingdom1.GetComponent<Faction>(), RelationsTestKingdom2.GetComponent<Faction>(), 30);
        if ((Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) && RelationsTestKingdom1 && RelationsTestKingdom2)
            RelationsShift(RelationsTestKingdom1.GetComponent<Faction>(), RelationsTestKingdom2.GetComponent<Faction>(), -30);
    }

    public void addToShopping()
    {
        gameObjectss = GameObject.FindGameObjectsWithTag("Shop");
        //here add the transform from merchant onStart class 
    }

    public void printt()
    {
        Debug.Log(gameObjectss[0].tag.ToString());
    }
}
