using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GolemEarth : MonsterAI
{
    [SerializeField]
    GameObject skillA; //���Ÿ� ���� ������Ʈ
    [SerializeField]
    GameObject impactWave; //���� ��ų ������Ʈ
    [SerializeField]
    Transform impactWavePosition; //������ų ���� ��ġ
    [SerializeField]
    Transform skillPointA; //���Ÿ� ���ݽ�ų ���� ��ġ
    public RectTransform bossHealthGroup, bossHealthBar;
    bool isLook; //�÷��̾ �ٶ� ���� üũ
    Vector3 lookVec; //�÷��̾� ���� �̸� ���� 
    Vector3 tautVec;
    public float speed = 1000f;
    [SerializeField]
    GameObject cinema; //�ó׸���
    GameManager manager; //���Ӹ޴���
    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        agent.isStopped = true;
        isLook = true;
        StartCoroutine(Look());
        StartCoroutine(BossHealth());
        StartCoroutine(Think());
    }
    //�÷��̾ �ٶ�, �÷��̾� ������ �̸� ����
    IEnumerator Look()
    {
        while (true)
        {
            if (isDead)
            {
                manager.bossText.text = "x 1";
                StopAllCoroutines();
            }
            if (isLook)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 1f; //�÷��̾� �Է°����� ���� ���Ͱ� ����
                transform.LookAt(player.position + lookVec); //�÷��̾ �ٶ�
            }
            if(!isLook || !isDead) { agent.SetDestination(tautVec); }
            yield return null;
        }
    }
    //������ ����
    IEnumerator Think()
    {
        yield return new WaitForSeconds(1f);
        int randomAction = Random.Range(0, 3);
        switch (randomAction)
        {
             case 0:
                 StartCoroutine(RushAttack());
                 break;
            case 1:
                 StartCoroutine(Skill());
                 break;
            case 2:
                 StartCoroutine(WideSkill());
                 break;
        }
    }
    //�÷��̾�� ���� �������� ����
    IEnumerator RushAttack()
    {
            float targetRadius = 3f;
            float targetRange = 5f;
            isLook = false;
            agent.isStopped = false;
            while (true)
            {
                 tautVec = player.position + lookVec;//���������� �� ��ġ ���� ����*/
                 RaycastHit[] rayHits =
                 Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
                 LayerMask.GetMask("Player"));
                if (rayHits.Length > 0)
                {
                    agent.isStopped = true;
                    anim.SetTrigger("isPunch");
                    monsterCol.enabled = false;
                    yield return new WaitForSeconds(5f);
                    monsterCol.enabled = true;
                    StartCoroutine(Think());
                    yield break;
                }
                yield return null;
            }
    }
    //���Ÿ� ���� ��ų 
    IEnumerator Skill()
    {
        isLook= true;
        anim.SetTrigger("isProjectile Attack");
        monsterCol.enabled = false;
        yield return new WaitForSeconds(0.8f);
        for(int i =0; i < 3; i++)
        {
            GameObject instantSkillA = Instantiate(skillA, skillPointA.position, skillPointA.rotation);
            instantSkillA.transform.LookAt(player);//�÷��̾��� �������� ȸ��
            instantSkillA.GetComponent<Rigidbody>().AddForce(instantSkillA.transform.forward * speed);  //��ü�� ���� ���Ͽ� �߻�ü�� �ӵ��� �����մϴ�
            yield return new WaitForSeconds(0.2f);
        }
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //������ų
    IEnumerator WideSkill()
    {
        anim.SetTrigger("isCastSpell");
        monsterCol.enabled = false;
        yield return new WaitForSeconds(0.8f);
        GameObject instantSkillB = Instantiate(impactWave,impactWavePosition.position,impactWavePosition.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(instantSkillB);
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //���� ü�� UI 
    IEnumerator BossHealth()
    {
        while(true)
        {
            bossHealthBar.localScale = new Vector3((float)curHealth / maxHealth, 1, 1);
            if (curHealth < 0) { bossHealthBar.localScale = Vector3.zero; };
            yield return null;
        }
    }
    //�ó׸�ƽ�� ��������
    public void SignalBattleOn()
    {
        GetComponent<GolemEarth>().enabled = true;
        cinema.SetActive(false);
    }
}
