using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SocialPlatforms.Impl;
//EnemyScript�� ��ģ ai��ũ��Ʈ 
public class MonsterAI : MonoBehaviour
{
    public enum Type { A , B, C , D} //���� Ÿ��
    [SerializeField]
    Type enemyType; //���� Ÿ�� ���� 
    public int maxHealth; //�ִ� ü��
    public int curHealth; //���� ü�� 
    [SerializeField]
    Transform[] movePoint;    //���� ��ǥ ��ġ ����
    public BoxCollider meleeArea;      //���� ���� �ڽ��ݶ��̴�
    [SerializeField]
    private GameObject slashOb;         //���Ÿ� ���� ������Ʈ
    [SerializeField]
    Transform farSkillPointA;              //���Ÿ� ���� ���� ��ġ 
    public  Transform player;         //���� ���� Target����
    public NavMeshAgent agent;       //NMA ����
    int randomInt;            //movePoint ������ �����ϰ� ���� �� ����
    private bool isAttack;    //���� ���������� Ȯ���ϴ� ���� 
    public Rigidbody rigid;          //�浹�� �Ͼ�� ���� ��Ȳ ���� ����
    public Collider monsterCol;      //���� �ݶ��̴� 
    public SkinnedMeshRenderer[] meshs;             //�ǰݽ� ��� ���׸����� ����
    public Animator anim;            //���� �ൿ �ִϸ��̼�
    public bool isDead;               //������ ���� üũ ����
    [SerializeField]
    GameObject[] dropItem;               //���� ����� ��� ����
    [SerializeField]
    float attackRidius;             //���� ���ݰŸ�
    Sound speaker;                  //���� ����
    private void Awake()
    {
        VariableRest();
        StartCoroutine(AiMonster());
        randomInt = Random.Range(0, movePoint.Length); //���� ���� ����
        anim.SetBool("isWalk", true);   //ù���۽� �ȴ� �ൿ ����
        speaker = FindObjectOfType<Sound>();
    }
    IEnumerator AiMonster()    //���� ���� ��� 
    { 
        if(enemyType != Type.D) //�������Ͱ� �ƴϸ� ����
        {
            while (agent.SetDestination(movePoint[randomInt].transform.position))//�����ϰ� �������� �̵� ���� 
            {
                if (Vector3.Distance(transform.position, player.position) <= 20f) //Player�� ��������� �÷��̾ ���󰣴�.
                {
                    agent.SetDestination(player.position);
                }
                Destination();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
    void VariableRest() //���� �ʱ�ȭ.
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();    //Material�� Mesh Renderer ������Ʈ���� ���ٰ���!
        anim = GetComponent<Animator>();
        monsterCol = GetComponent<Collider>();

    }
    void FreezeVelocity()   //ai ���� �浹�� �ڷ� �и��� �浹 ���� 
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    void Destination() //�������� �����ϸ� ������ ����
    {
        if (Vector3.Distance(transform.position, movePoint[randomInt].transform.position) < 5f)
        {
            randomInt = Random.Range(0, movePoint.Length);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //�ٰŸ� �����϶� 
        if (other.tag == "Melee"&&!isDead)
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            //�˹� ��ġ or ����
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
        //���Ÿ� �����϶� 
        else if (other.tag == "Slash" && !isDead)
        {
            Slash slash = other.GetComponent<Slash>();
            curHealth -= slash.damage;
            //������ ���� ������� ���� �ǵ��� 
            Destroy(other.gameObject,1f);
            //�˹� ��ġ or ����
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
    }
    //������ ���� 
    IEnumerator OnDamage(Vector3 reactVec)
    {
        anim.SetTrigger("doDamage");
        speaker.SoundByNum2(11);
        foreach (SkinnedMeshRenderer mesh in meshs)
        { mesh.material.color = Color.red;}
        yield return new WaitForSeconds(0.1f);
        //���� ������� 
        if (curHealth > 0)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
            { mesh.material.color = Color.white; }
        }
        //���� ���� 
        else
        {
            anim.SetBool("isAttack", false);
            isDead = true;
            if (enemyType != Type.D) { StopAllCoroutines(); }
             speaker.SoundByNum2(4);
            monsterCol.enabled = false;
            foreach (SkinnedMeshRenderer mesh in meshs)
            { mesh.material.color = Color.gray; }
            gameObject.layer = 7;
            rigid.velocity= Vector3.zero;
            anim.SetTrigger("doDie");
            //���� �״� ������ ���� ��� ���� , ������ �ƴҶ���
            if (enemyType != Type.D) {
                int ranItem = Random.Range(0, dropItem.Length);
                Vector3 itemVec = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                dropItem[ranItem].transform.localScale = Vector3.one * 3f;
                Instantiate(dropItem[ranItem], itemVec, Quaternion.identity);
            }
            //������ �ƴҶ� 3�ʵڿ� ������Ʈ ����
            if(enemyType != Type.D){ Destroy(gameObject, 3f); };
            GameManager.DieCount();

        }
    }
    //Player���� Ray�� ���
    void Targerting()
    {
        if(!isDead && enemyType != Type.D)
        {
            float targetRadius = 0f;
            float targetRange = 0f;
            //���� Ÿ�� �б⹮
            switch (enemyType)
            {
                case Type.A:
                    targetRadius = attackRidius;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 3f;
                    targetRange = 4f;
                    break;
                case Type.C:
                    targetRadius = 1.5f;
                    targetRange = 30f;
                    break;
            }

            RaycastHit[] rayHits =
                 Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
                 LayerMask.GetMask("Player"));
            //rayHit ������ �����Ͱ� ������ ���� �ڷ�ƾ ���� , �̹� �������϶� ���� �ȵ�
            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
       
    }
    //���� ���ݱ�� BoxCollider false true�� ���� ����
    IEnumerator Attack()
    {
            agent.isStopped = true;
            isAttack = true;
            anim.SetBool("isAttack", true);
            transform.LookAt(player);
            switch (enemyType)
            {
                case Type.A: //�Ϲ� ���� �ൿ 
                    yield return new WaitForSeconds(0.2f);
                    meleeArea.enabled = true;
                    yield return new WaitForSeconds(1f);
                    meleeArea.enabled = false;
                    break;
                case Type.B: //������ ���� �ൿ
                    rigid.AddForce(transform.forward * 100f, ForceMode.Impulse); //���� ���� 
                    meleeArea.enabled = true;
                    yield return new WaitForSeconds(1f);
                    rigid.velocity = Vector3.zero; //�ӵ� ���� 
                    meleeArea.enabled = false;
                    yield return new WaitForSeconds(2f);
                    break;
                case Type.C:
                    yield return new WaitForSeconds(0.5f);
                    GameObject instantSlash = Instantiate(slashOb, farSkillPointA.position, farSkillPointA.rotation);
                    instantSlash.transform.LookAt(player);
                    instantSlash.GetComponent<Rigidbody>().AddForce(instantSlash.transform.forward * 1000f);
                    yield return new WaitForSeconds(2f);
                    Destroy(instantSlash, 1f);
                    break;
            }
            agent.isStopped = false;
            isAttack = false;
            anim.SetBool("isAttack", false);
    }
    private void FixedUpdate()
    {

        Targerting();
        FreezeVelocity();
    }
}
