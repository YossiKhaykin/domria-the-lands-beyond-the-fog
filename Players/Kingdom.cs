using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;


public class Kingdom : Faction
{
    public int Gold;
    public int Provences;
    public bool amIRealPlayer;

    [Header("UI")]
    public Camera cam;

    public Transform buttons;
    public Transform mainCanvas;
    public Transform miniMap;
    public Transform buildingPanel;
    public Text goldT;
    public Text woodT;
    public Text metalT;
    public Text foodT;
    public Text runesT;
    public Text StoneT;
    public Text GemsT;

    [Header("Resources")]
    public int golds;
    public int wood;
    public int metal;
    public int food;
    public int runes;
    public int Stone;
    public int Gems;

    public bool check;

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

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
       
        if (amIRealPlayer == true)
        {
            buttons.gameObject.SetActive(true);
            mainCanvas.gameObject.SetActive(true);
            miniMap.gameObject.SetActive(true);
         
            mainCanvas.tag = this.tag;
            buildingPanel.tag= this.tag;
        }
        else
        {
            buttons.gameObject.SetActive(false);
            mainCanvas.gameObject.SetActive(false);
            miniMap.gameObject.SetActive(false);
        }

        for (int i = 0; i < TownHalls.Count; i++)
        {
            if (TownHalls[i].gameObject != null)
            {
                if (TownHalls[i].GetComponent<ReatretPoint>() == true)
                {
                    TownHalls[i].GetComponent<ReatretPoint>().MyKingdom = this.gameObject;
                    TownHalls[i].GetComponent<ReatretPoint>().myFactionsIs = FactionTag;
                }

                if (TownHalls[i].GetComponent<MonstersDen>() == true)
                {
                    TownHalls[i].GetComponent<MonstersDen>().MyKingdom = this.gameObject;
                    TownHalls[i].GetComponent<MonstersDen>().tag = FactionTag.ToString();

                }
                if (TownHalls[i].GetComponent<Building>() == true)
                {
                    TownHalls[i].GetComponent<Building>().MyKingdom = this.gameObject;
                    TownHalls[i].GetComponent<Building>().tag = FactionTag.ToString();
                }
            }
        }
        updateRes(1500, 1500, 1500, 1500, 1500, 1500, 1500);

        Provences = 1;
    }
    //for buildings
    public void updateRes(int gold,int wood, int metal, int food, int runes,int stones,int gems)
    {
        addGold(gold);
        addWood(wood);
        addMetal(metal);
        addFood(food);
        addRunes(runes);
        addStone(stones);
        addGems(gems);
    }
    //for buildings
    public bool subtractRes(int gold, int wood, int metal, int food, int runes, int stones, int gems)
    {
        check = true;
        if (golds - gold < 0) check = false;
        if (this.wood - wood < 0) check = false;
        if (this.metal - metal < 0) check = false;
        if (this.food - food < 0) check = false;
        if (this.runes - runes < 0) check = false;
        if (this.Stone - stones < 0) check = false;
        if (this.Gems - gems < 0) check = false;

        if (check) {
                return true;
            }
        else
        {
            return false;
        }
    }

    public void removeRes(int gold, int wood, int metal, int food, int runes, int stones, int gems)
    {
        subGold(gold);
        subWood(wood);
        subMetal(metal);
        subFood(food);
        subRunes(runes);
        subStone(stones);
        subGems(gems);
    }
    public void addGold(int g)
    {
        golds += g;
        goldT.text = this.golds.ToString();
    }

    public void addWood(int g)
    {
        wood += g;
        woodT.text = this.wood.ToString();
    }

    public void addMetal(int g)
    {
        metal += g;
        metalT.text = this.metal.ToString();
    }

    public void addFood(int g)
    {
        food += g;
        foodT.text = this.food.ToString();
    }

    public void addRunes(int g)
    {
        runes += g;
        runesT.text = this.runes.ToString();
    }

    public void addStone(int g)
    {
        Stone += g;
        StoneT.text = this.Stone.ToString();
    }

    public void addGems(int g)
    {
        Gems += g;
        GemsT.text = this.Gems.ToString();
    }

    //sub resources - call per needed resource method 
    public bool subGold(int g)
    {
        if (golds -g >=0)
        {
            golds -= g;
            goldT.text = this.golds.ToString();
            return true;
        }
        return false;
    }

    public bool subWood(int g)
    {
        if (wood - g >= 0)
        {
            wood -= g;
            woodT.text = this.wood.ToString();
            return true;
        }
        return false;
    }

    public bool subMetal(int g)
    {
        if (metal - g >= 0)
        {
            metal -= g;
            metalT.text = this.metal.ToString();
            return true;
        }
        return false;
    }

    public bool subFood(int g)
    {
        if (food - g >= 0)
        {
            food -= g;
            foodT.text = this.food.ToString();
            return true;
        }
        return false;
    }

    public bool subRunes(int g)
    {
        if (runes - g >= 0)
        {
            runes -= g;
            runesT.text = this.runes.ToString();
            return true;
        }
        return false;
    }

    public bool subStone(int g)
    {
        if (Stone - g >= 0)
        {
            Stone -= g;
            StoneT.text = this.Stone.ToString();
            return true;
        }
        return false;
    }

    public bool subGems(int g)
    {
        if (Gems - g >= 0)
        {
            Gems -= g;
            GemsT.text = this.Gems.ToString();
            return true;
        }
        return false;
    }
}
