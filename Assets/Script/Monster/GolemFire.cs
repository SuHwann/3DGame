using System.Collections;
using System.Collections.Generic;
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
    bool isLook; //�÷��̾ �ٶ� ���� üũ
    Vector3 lookVec; //�÷��̾� ���� �̸� ���� 
    Vector3 tautVec;
    public float speed = 1000f;
    int count = 0;
    private void Start()
    {
        //anim.SetTrigger("isSpawn");
        agent.isStopped = true;
        isLook = true;
        StartCoroutine(Look());
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
        int randomAction = Random.Range(0, 2);
        switch (1)
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
        tautVec = player.position + lookVec;//���������� �� ��ġ ���� ����
        isLook = false;
        agent.isStopped = false;
        monsterCol.enabled = false;
        anim.SetTrigger("isPunch");
        meleeArea.enabled = true;
        yield return new WaitForSeconds(2f);
        meleeArea.enabled = false;
        isLook = true;
        agent.isStopped = true;
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //���Ÿ� ���� ������Ʈ �����Լ�
    void DistanceAttackOb()
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
            DistanceAttackOb();
            yield return new WaitForSeconds(0.2f);
        }
        count++;
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 3; i++)
        {
            DistanceAttackOb();
            yield return new WaitForSeconds(0.1f);
        }
        count++;
        yield return new WaitForSeconds(1f);
        DistanceAttackOb();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Think());
    }
    //������ų
    IEnumerator WideSkill()
    {
        anim.SetTrigger("isCastSpell");
        yield return new WaitForSeconds(0.8f);
        GameObject instantSkillB = Instantiate(impactWave, impactWavePosition.position, impactWavePosition.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(instantSkillB);
        StartCoroutine(Think());
    }
}
