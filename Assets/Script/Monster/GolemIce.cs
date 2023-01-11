using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIce : MonsterAI
{
    [SerializeField]
    GameObject skill; //���Ÿ� ���� ������Ʈ
    [SerializeField]
    Transform skillPoint; //���Ÿ� ���ݽ�ų ���� ��ġ
    [SerializeField]
    GameObject impactWave; //���� ��ų ������Ʈ
    [SerializeField]
    Transform impactWavePosition; //������ų ���� ��ġ
    public RectTransform bossHealthGroup, bossHealthBar;
    bool isLook; //�÷��̾ �ٶ� ���� üũ
    Vector3 lookVec; //�÷��̾� ���� �̸� ���� 
    Vector3 tautVec;
    [SerializeField]
    int speed; //���Ÿ� ���� ����ü �ӵ�
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
                anim.SetTrigger("doDie");
                StopAllCoroutines();
                yield break;
            }
            if (isLook)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 1f; //�÷��̾� �Է°����� ���� ���Ͱ� ����
                transform.LookAt(player.position + lookVec); //�÷��̾ �ٶ�
            }
            if (Vector3.Distance(transform.position, player.position) < 25f)
            {
                agent.isStopped = true;
            }
            else { agent.SetDestination(tautVec); }
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
        tautVec = player.position + lookVec;//���������� �� ��ġ ���� ����*/
        isLook = false;
        agent.isStopped = false;
        anim.SetTrigger("isPunch");
        monsterCol.enabled = false;
        yield return new WaitForSeconds(3f);
        agent.isStopped = true;
        isLook = true;
        monsterCol.enabled = true;
        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }
    //���Ÿ� ���� ��ų 
    IEnumerator Skill()
    {
        anim.SetTrigger("isShot");
        monsterCol.enabled = false;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {
            GameObject instantSkill = Instantiate(skill, skillPoint.position, skillPoint.rotation);
            instantSkill.transform.LookAt(player);//�÷��̾��� �������� ȸ�� 
            instantSkill.GetComponent<Rigidbody>().AddForce(instantSkill.transform.forward * speed); //���ϰ� ���� ���Ͽ� �߻�ü �ӵ� ����
            yield return new WaitForSeconds(0.2f);
        }
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //������ų
    IEnumerator WideSkill()
    {
        isLook = false;
        anim.SetTrigger("isCastSpell");
        monsterCol.enabled = false;
        GameObject instantSkillB = Instantiate(impactWave, impactWavePosition.position, impactWavePosition.rotation);
        yield return new WaitForSeconds(1.8f);
        instantSkillB.transform.GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForSeconds(1f);
        instantSkillB.transform.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(instantSkillB);
        isLook = true;
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //�������� UI
    IEnumerator BossHealth()
    {
        while (true)
        {
            bossHealthBar.localScale = new Vector3((float)curHealth / maxHealth, 1, 1);
            if (curHealth < 0) { bossHealthBar.localScale = Vector3.zero; };
            yield return null;
        }
    }
    //�ó׸�ƽ�� ������
    public void SignalBattleOn()
    {
        GetComponent<GolemIce>().enabled = true;
        cinema.SetActive(false);
    }
}
