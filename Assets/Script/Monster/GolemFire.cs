using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class GolemFire : MonsterAI
{
    [SerializeField]
    GameObject[] skill; //���Ÿ� ���� ������Ʈ
    [SerializeField]
    Transform[] skillPoint; //���Ÿ� ���ݽ�ų ���� ��ġ
    [SerializeField]
    GameObject impactWave; //���� ��ų ������Ʈ
    [SerializeField]
    Transform impactWavePosition; //������ų ���� ��ġ
    public RectTransform bossHealthGroup, bossHealthBar;
    bool isLook; //�÷��̾ �ٶ� ���� üũ
    Vector3 lookVec; //�÷��̾� ���� �̸� ���� 
    Vector3 tautVec;
    int count = 0; //���Ÿ� ���� ī��Ʈ ����
    [SerializeField]
    GameObject cinema; //�ó׸���
    private void Start()
    {
        //anim.SetTrigger("isSpawn");
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
            else
                agent.SetDestination(tautVec);
            yield return null;
        }
    }
    //������ ����
    IEnumerator Think()
    {
        yield return new WaitForSeconds(1f);
        int randomAction = Random.Range(0, 3);
        if(!isDead)
        {
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

    }
    //�÷��̾�� ���� �������� ����
    IEnumerator RushAttack()
    {
            tautVec = player.position + lookVec;//���������� �� ��ġ ���� ����*/
            isLook = false;
            agent.isStopped = false;
            anim.SetTrigger("isPunch");
            yield return new WaitForSeconds(3f);
            agent.isStopped = true;
            isLook = true;
            StartCoroutine(Think());
    }
    //���Ÿ� ���� ������Ʈ �����Լ�
    void DistanceAttackOb(int speed)
    {
        GameObject instantSkill = Instantiate(skill[count], skillPoint[count].position, skillPoint[count].rotation);
        instantSkill.transform.LookAt(player);//�÷��̾��� �������� ȸ�� 
        instantSkill.GetComponent<Rigidbody>().AddForce(instantSkill.transform.forward * speed); //���ϰ� ���� ���Ͽ� �߻�ü �ӵ� ����
    }
    //���Ÿ� ���� ��ų 
    IEnumerator Skill()
    {
        count = 0;
        anim.SetTrigger("isShot");
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {
            DistanceAttackOb(1000);
            yield return new WaitForSeconds(0.2f);
        }
        count++;
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 3; i++)
        {
            DistanceAttackOb(2000);
            yield return new WaitForSeconds(0.1f);
        }
        count++;
        yield return new WaitForSeconds(1.5f);
        DistanceAttackOb(1000);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Think());
    }
    //������ų
    IEnumerator WideSkill()
    {
        isLook = false;
        anim.SetTrigger("isCastSpell");
        GameObject instantSkillB = Instantiate(impactWave, impactWavePosition.position, impactWavePosition.rotation);
        yield return new WaitForSeconds(1.8f);
        instantSkillB.transform.GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForSeconds(1f);
        instantSkillB.transform.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(instantSkillB);
        isLook = true;
        StartCoroutine(Think());
    }
    IEnumerator BossHealth()
    {
        while (true)
        {
            bossHealthBar.localScale = new Vector3((float)curHealth / maxHealth, 1, 1);
            if (curHealth < 0) { bossHealthBar.localScale = Vector3.zero; };
            yield return null;
        }
    }
    public void SignalBattleOn()
    {
        GetComponent<GolemFire>().enabled = true;
        cinema.SetActive(false);
    }
}
