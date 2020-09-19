public class StatChanger : CharEffect
{
    public enum EffectedStats
    {
        STR,
        DEX,
        INT,
        XPMultiplier,
        GoldMultiplier,
        SizeChange,
    //HPRegainRate,
    //MaxMana,
    //MPRegainRate,
    }

    public enum POT
    {
        Percent,
        Constant
    }

    [System.Serializable]
    public struct Effect
    {
        public EffectedStats Stat;
        public POT PercentOrConst;
        public double amount;
        public double AmountInConst;
    }
    
    public Effect[] effects; // an array of all stat changes applied by the skill

    public override void ExecuteConEffect() // changes the appropriate stats
    {
        for (int i = 0; i < effects.Length; i++)
        {
            switch (effects[i].Stat)
            {
                case EffectedStats.STR: // adds to STR
                    {
                        if (effects[i].PercentOrConst == POT.Percent)
                            effects[i].AmountInConst = Target.GetComponent<NPC>().STR * (effects[i].amount * 0.01);
                        else
                            effects[i].AmountInConst = effects[i].amount;
                        Target.GetComponent<NPC>().STR += (int)effects[i].AmountInConst;
                        break;
                    }
                case EffectedStats.DEX: // adds to DEX
                    {
                        if (effects[i].PercentOrConst == POT.Percent)
                            effects[i].AmountInConst = Target.GetComponent<NPC>().DEX * (effects[i].amount * 0.01);
                        else
                            effects[i].AmountInConst = effects[i].amount;
                        Target.GetComponent<NPC>().DEX += (int)effects[i].AmountInConst;
                        break;
                    }
                case EffectedStats.INT: // adds to INT
                    {
                        if (effects[i].PercentOrConst == POT.Percent)
                            effects[i].AmountInConst = Target.GetComponent<NPC>().INT * (effects[i].amount * 0.01);
                        else
                            effects[i].AmountInConst = effects[i].amount;
                        Target.GetComponent<NPC>().INT += (int)effects[i].AmountInConst;
                        break;
                    }
                case EffectedStats.XPMultiplier: // sets the multiplier for XP; amount = 100% => no change, amount < 100% => decrease; amount < 100%  => increase
                    {
                        if (effects[i].PercentOrConst == POT.Percent)
                            effects[i].AmountInConst = effects[i].amount * 0.01;
                        else
                            effects[i].AmountInConst = effects[i].amount;
                        Target.GetComponent<NPC>().XPMultiplier *= (int)effects[i].AmountInConst;
                        break;
                    }
                case EffectedStats.GoldMultiplier: // sets the multiplier for Gold; amount = 100% => no change, amount < 100% => decrease; amount < 100%  => increase
                    {
                        if (effects[i].PercentOrConst == POT.Percent)
                            effects[i].AmountInConst = effects[i].amount * 0.01;
                        else
                            effects[i].AmountInConst = effects[i].amount;
                        Target.GetComponent<NPC>().GoldMultiplier *= (int)effects[i].AmountInConst;
                        break;
                    }
                case EffectedStats.SizeChange: // sets the multiplier for Size; amount = 100% => no change, amount < 100% => decrease; amount < 100%  => increase
                    {
                        if (effects[i].PercentOrConst == POT.Percent)
                            effects[i].AmountInConst *= effects[i].amount * 0.01;
                        else
                            effects[i].AmountInConst = effects[i].amount;
                        Target.transform.localScale *= (float)effects[i].AmountInConst;
                        break;
                    }
                //case EffectedStats.Speed:
                //    {
                //        break;
                //    }
                //case EffectedStats.MaxHP:
                //    {
                //        break;
                //    }
                //case EffectedStats.HPRegainRate:
                //    {
                //        break;
                //    }
                //case EffectedStats.MaxMana:
                //    {
                //        break;
                //    }
                //case EffectedStats.MPRegainRate:
                //    {
                //        break;
                //    }
                default:
                    {
                        break;
                    }
            }
        }

    }

    public override void StopConEffect() // resets the stats once the effects duration is over
    {
        foreach (Effect effect in effects)
        {
            switch (effect.Stat)
            {
                case EffectedStats.STR:
                    {
                        Target.GetComponent<NPC>().STR -= (int)effect.AmountInConst;
                        break;
                    }
                case EffectedStats.DEX:
                    {
                        Target.GetComponent<NPC>().DEX -= (int)effect.AmountInConst;
                        break;
                    }
                case EffectedStats.INT:
                    {
                        Target.GetComponent<NPC>().INT -= (int)effect.AmountInConst;
                        break;
                    }
                case EffectedStats.XPMultiplier:
                    {
                        Target.GetComponent<NPC>().XPMultiplier -= (int)effect.AmountInConst;
                        break;
                    }
                case EffectedStats.GoldMultiplier:
                    {
                        Target.GetComponent<NPC>().GoldMultiplier -= (int)effect.AmountInConst;
                        break;
                    }
                case EffectedStats.SizeChange:
                    {
                        Target.transform.localScale /= (float)effect.AmountInConst;
                        break;
                    }
                //case EffectedStats.Speed:
                //    {
                //        break;
                //    }
                //case EffectedStats.MaxHP:
                //    {
                //        break;
                //    }
                //case EffectedStats.HPRegainRate:
                //    {
                //        break;
                //    }
                //case EffectedStats.MaxMana:
                //    {
                //        break;
                //    }
                //case EffectedStats.MPRegainRate:
                //    {
                //        break;
                //    }
                default:
                    {
                        break;
                    }
            }

        }
    }
}
