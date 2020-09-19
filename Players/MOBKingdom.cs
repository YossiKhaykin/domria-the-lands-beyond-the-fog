using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOBKingdom : Faction
{
    [Header("Resources")]
    public int MyResource;

    public void IdGiverForDens(GameObject Building)
    {
        if (Building.GetComponent<MonstersDen>() == true)
        {
            Building.GetComponent<MonstersDen>().MyKingdom = this.gameObject;
            Building.GetComponent<MonstersDen>().tag = FactionTag.ToString();

        }

        if (Building.GetComponent<ReatretPoint>() == true)
        {
            Building.GetComponent<ReatretPoint>().MyKingdom = this.gameObject;
            Building.GetComponent<ReatretPoint>().myFactionsIs = FactionTag;
        }
    }

    public void addMyResource(int g)
    {
        MyResource += g;
    }

    public bool subMyResource(int g)
    {
        if (MyResource - g >= 0)
        {
            MyResource -= g;
            return true;
        }
        return false;
    }
}
