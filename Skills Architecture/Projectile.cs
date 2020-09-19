using UnityEngine;
using Enums;
public class Projectile : MonoBehaviour
{
    [SerializeField] public Projectile_Type projectileType;
    public GameObject target;
    public OffenceSkill Parent;
    public HealingSkill HParent;
    public GameObject father;
    [SerializeField]public float speed = 25;
    private float ForwordPosition;

    private void Start()
    {
        if (projectileType == Projectile_Type.arrow)
        {
            ForwordPosition = 90f;
        }
        else
        {
            ForwordPosition = 0f;
        }
    }

    void Update()
    {
        if (target == null || father == null)
        {
            Destroy(this.gameObject);
        }
        else
        {

            float EnemyX = target.transform.position.x;
            float EnemyZ = target.transform.position.z;
            float EnemyY = target.transform.position.y;
            Vector3 travelToMe = target.transform.position;
            Vector3 targetDir = target.transform.position - transform.position;
            float step = speed * Time.deltaTime;
            var rotation = Quaternion.LookRotation(targetDir).eulerAngles;
            transform.rotation = Quaternion.Euler(ForwordPosition, rotation.y, rotation.z);
            transform.position = Vector3.MoveTowards(transform.position, travelToMe = new Vector3(EnemyX, EnemyY + 1f, EnemyZ), step);

            if (Vector3.Distance(transform.position, travelToMe = new Vector3(EnemyX, EnemyY + 1f, EnemyZ)) < 0.3f)
            {
                if (Parent != null)
                {
                    Parent.GetComponent<OffenceSkill>().TheCaster = father;
                    Parent.GetComponent<OffenceSkill>().SkillAbility(target);
                   
                }
                else
                {
                    Parent.GetComponent<HealingSkill>().TheCaster = father;
                    HParent.GetComponent<HealingSkill>().SkillAbility(target);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
