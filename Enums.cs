namespace Enums
{
    public enum RelationsTypes
    {
        Friendly,
        Neutral,
        Enemy
    }

    public enum Role
    {
        Common,
        Rare,
        Elite,
        Boss,
        WorldBoss
    }

    public enum PlayerRole
    {
        //dps,
        //tank,
        //healer
        Soldier,
        Healer,
        Leader
    }

    public enum ClassType
    {
        Mage,
        Warrior,
        Necromancer,
        Rouge,
        Priest,
        Archer,
        Bard
    }

   

    public enum MerchantType
    {
        Weapons,
        Wearables,
        Potions,
        Goodies,
        CraftingItems,
        defult,
        Jewlery,
        SpecialRings,
        NoShop
    }

    public enum Gender
    {
        female,
        male,
        none
    }

    public enum StatType
    {
        STR,
        DEX,
        INT,
        None
    }

    public enum RingAmuletBuff
    {
        hp,
        mana
    }
    public enum SummonType
    {
        SkeletonWarrior,
        SkeletonArcher,
        SkeletonMage,
        Shade,
        Wolf,
        Bee
    }

    public enum WearTypes
    {
        Helmet,
        Armour,
        Shield,
        Lagings,
        Ring,
        Ring2,
        Amulate,
        Weapon,
        Other
    }

    public enum ArmourType
    {
        Light,
        Medium,
        Heavy,
        Shield,
        None
    }
    public enum TypeOfWeapons
    {
        Dagger,
        Spear,
        Staff,
        OneHeandSword,
        TwoHandedSword,
        Bow,
        CrowssBow,
        PoleArme,
        ThrowWeapon,
        Exstras,
        FistWeapons,
        Axe,
        Mace,
        TwoHandedAxe,
        None
    }

    public enum LootType
    {
        Wearable,
        Weapon,
        Potion,
        Jewlery,
        Other
    }

    public enum PotionType
    {
        Health,
        Mana,
        Magic
    }

    public enum RaretyType
    {
        Normal,
        Rare,
        Epic,
        Legendary
    }

    public enum GraspType
    {
        TwoHanded,
        OneHanded,
        None
    }

    public enum ChestType
    {
        Good,
        Steel,
        Gold,
        Boss,
        DeadCharcter
    }

    public enum Ailment
    {
        Stun,
        Poison,
        burn,
        freeze
    }

    public enum HeroType
    {
        Monster,
        Boss,
        Player
    }
    public enum SkillSpawn
    {
        FromNPC,
        TargetPosition,
        AboveTargetHead,
        ShootingPosition,
        RandomSpellSlotSpawn,
        MeteorSpawn,
        Summons,
        OnMeAura,
        OnTaregetAura,
        AuraOnCastingLocation,
        None
    }
    public enum TypeOfSkill
    {
        Heal,
        Melee,
        Buff,
        Debuff,
        Strom,
        Movment,
        Projectile,
        summon

    }
    public enum OffacneType
    {
        Projectile,
        Strom,
        LavaGround,
        PricingShot,
        BouncingProjectile,
        Charge,
        None
    }
    public enum HealType
    {
        Projectile,
        Strom,
        LavaGround,
        PricingShot,
        BouncingProjectile,
        None

    }
    public enum Projectile_Type
    {
        arrow,
        spell
    }
    public enum MonmstersFactions
    {
        Monster = 0,
        Orc,
        Undaed,
        Hive,
        Bandits
    }
    public enum FactionsColor
    {
        Monster = 0,
        Red,
        Blue,
        Green,
        Purple,
        Orange,
        Brown,
        Black,
        White,
        Orc,
        Basndits,
        Hive,
        Undead
    }
    public enum FactionsTag
    {
        Monster,
        Red,
        Blue,
        Green,
        Purple,
        Orange,
        Brown,
        Black,
        White,
        Orc,
        Basndits,
        Hive,
        Undead
    }
    public enum ToAoe
    {
        Heal,
        Dmg
    }

    public enum BuildingType
    {
        Shop,
        Guild,
        Research,
        Defence,
        Shrine,
        TownHall
    }

    public enum BuildingTypePanel
    {
        Research,
        Military,
        Economy
    }
    //public enum NpcStatus
    //{
    //    Suned,
    //    Revived,
    //    Dodge,
    //    UseingSkill,
    //    Wamdering,
    //    LookingForQuest,
    //    Noramal
    //}
}