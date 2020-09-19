using System.Collections;
using UnityEngine;
using Enums;

public abstract class Entity : MonoBehaviour
{
    [Header("Global Need")]
    public int Value;
    public int HP = 1;
    public float CurrentHP;
    public string ID;
    public GameObject IWasHitBy;
    public GameObject FloatingtextPrefab;
    public GameObject FloatingHealingTextPrefab;
    public int timeUntilDisappear;
    public bool IsDead;
    public float LastDmgTaken;
    public int MyDmgDealt;



    [Header("ID")]
    FactionsColor factionsColor;
    public GameObject MyKingdom;
    public GameObject FatherBuilding;

    public abstract void TakeDmg(float Dmg);
    public abstract void TrueTakeDmg(float Dmg);


    public void ShowFloatingtext()
    {
        var go = Instantiate(FloatingtextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = LastDmgTaken.ToString();
    }

    public abstract IEnumerator Die();

    public abstract void GetHealed(float HealAmount);






}
