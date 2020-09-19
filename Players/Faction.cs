using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public abstract class Faction : MonoBehaviour
{
    public int Player_number;
    public string FactionName;
    public int HeroesCount;
    public int agents;

    public FactionsTag FactionTag; //use this 
    public FactionsColor MyTag;

    public List<GameObject> TownHalls;

    public List<string> MyEnemies;
    public List<string> MyAllies;

    public bool ResetRelations = true;
    public bool IsDead = false;

    private void Start()
    {
        if (!ResetRelations && Player_number != 0)
        {
            if (MyEnemies.Count != 0)
            {
                SetPreMadeRelations(MyEnemies, false);
            }
            if (MyAllies.Count != 0)
            {
                Debug.Log("properly setting allies");
                SetPreMadeRelations(MyAllies, true);
            }
        }

        HeroesCount = 0;
    }

    public void SetPreMadeRelations(List<string> FrOrFo, bool friend)
    {
        int amount = 0;
        foreach (GameObject PotentialEnemy in GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().AllKingdoms)
        {
            if (friend) amount = 100;
            else amount = -100;
            if (FrOrFo.Exists(faction => faction == PotentialEnemy.GetComponent<Kingdom>().FactionTag.ToString()) && !PotentialEnemy.GetComponent<Kingdom>().ResetRelations && PotentialEnemy.GetComponent<Kingdom>().Player_number != 0)
            {
                FrOrFo.Remove(PotentialEnemy.GetComponent<Kingdom>().FactionTag.ToString());
                GameMaster.RelationsShift(this, PotentialEnemy.GetComponent<Kingdom>(), amount);
            }
        }
    }
}
