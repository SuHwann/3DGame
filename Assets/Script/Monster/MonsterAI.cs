using System.Collections;
using System.Collections.Generic;
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
    public Transform player;         //���� ���� Target����
    public NavMeshAgent agent;       //NMA ����
    int randomInt;            //movePoint ������ �����ϰ� ���� �� ����
    private bool isAttack;    //���� ���������� Ȯ���ϴ� ���� 
    public Rigidbody rigid;          //�浹�� �Ͼ�� ���� ��Ȳ ���� ����
    public Collider monsterCol;      //���� �ݶ��̴� 
    public SkinnedMeshRenderer[] meshs;             //�ǰݽ� ��� ���׸����� ����
    public Animator anim;            //���� �ൿ �ִϸ��̼�
    public bool isDead;               //������ ���� üũ ����
    [SerializeField]
    GameObject[] coins;               //���� ����� ��� ����
    private void Awake()
    {
        VariableRest();
        StartCoroutine(AiMonster());
        GetPoint();
        randomInt = Random.Range(0, movePoint.Length); //���� ���� ����
        StartCoroutine(Attack());
        anim.SetBool("isWalk", true);   //ù���۽� �ȴ� �ൿ ���� 
    }
    void GetPoint()  //Point���� ��ġ�� �ν����Ϳ� �����Ѵ�.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
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
        if (Vector3.Distance(transform.position, movePoint[randomInt].transform.position) < 1f)
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
            print(weapon.damage);
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
            Destroy(other.gameObject);
            //�˹� ��ġ or ����
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
    }
    //������ ���� 
    IEnumerator OnDamage(Vector3 reactVec)
    {
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

            foreach (SkinnedMeshRenderer mesh in meshs)
            { mesh.material.color = Color.gray; }
            StopAllCoroutines();
            gameObject.layer = 7;
            isDead = true;
            monsterCol.enabled = false;
            anim.SetTrigger("doDie");
            //���� �״� ������ ���� ��� ����
            int ranCoin = Random.Range(0, 3);
            Vector3 itemVec = new Vector3(transform.position.x, transform.position.y * 1.5f, transform.position.z);
            coins[ranCoin].transform.localScale = Vector3.one * 3f;
            Instantiate(coins[ranCoin], itemVec, Quaternion.identity);
            //���Ͱ� �����鼭 �ڷ� �и�
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            if(enemyType != Type.D) {Destroy(gameObject, 3f);} //������ �ƴҶ��� ����
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
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 3f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
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
        if (!isDead) //���Ͱ� ���� ������ ���� ����
        {
            agent.isStopped = true;
            isAttack = true;
            anim.SetBool("isAttack", true);

            switch (enemyType)
            {
                case Type.A: //�Ϲ� ���� �ൿ 
                    yield return new WaitForSeconds(0.2f);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f);
                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(1f);
                    break;
                case Type.B: //������ ���� �ൿ
                    yield return new WaitForSeconds(0.1f);
                    rigid.AddForce(transform.forward * 20, ForceMode.Impulse); //���� ���� 
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.5f);
                    rigid.velocity = Vector3.zero; //�ӵ� ���� 
                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(2f);
                    break;
                case Type.C:
                    yield return new WaitForSeconds(0.5f);

                    Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);
                    GameObject instantSlash = Instantiate(slashOb, pos, transform.rotation);
                    Rigidbody rigidSlash = instantSlash.GetComponent<Rigidbody>();
                    rigidSlash.velocity = transform.forward * 20f;

                    yield return new WaitForSeconds(2f);
                    Destroy(instantSlash, 1f);
                    break;
            }
            agent.isStopped = false;
            isAttack = false;
            anim.SetBool("isAttack", false);
        }
    }
    private void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }
}
