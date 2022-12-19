using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
    bool isLook; //�÷��̾ �ٶ� ���� üũ
    Vector3 lookVec; //�÷��̾� ���� �̸� ���� 
    Vector3 tautVec;
    public float speed = 1000f;
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
        int randomAction = Random.Range(0, 3);
        switch (0)
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
        while(true)
        {
            tautVec = player.position;//���������� �� ��ġ ���� ����
            RaycastHit[] rayHits =
             Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
             LayerMask.GetMask("Player"));
            if(rayHits.Length > 0 )
            {
                agent.isStopped = true;
                print("�÷��̾� ã��");
                anim.SetTrigger("isPunch");
                yield return new WaitForSeconds(5f);
                StartCoroutine(Think());
                yield break;
            }
            yield return null;
        }

    }
    //���Ÿ� ���� ��ų 
    IEnumerator Skill()
    {
        anim.SetTrigger("isProjectile Attack");
        yield return new WaitForSeconds(0.8f);
        for(int i =0; i < 3; i++)
        {
            GameObject instantSkillA = Instantiate(skillA, skillPointA.position, skillPointA.rotation);
            instantSkillA.transform.LookAt(player);//�÷��̾��� �������� ȸ��
            instantSkillA.GetComponent<Rigidbody>().AddForce(instantSkillA.transform.forward * speed);  //��ü�� ���� ���Ͽ� �߻�ü�� �ӵ��� �����մϴ�
            yield return new WaitForSeconds(0.2f);
        }
        StartCoroutine(Think());
    }
    //������ų
    IEnumerator WideSkill()
    {
        anim.SetTrigger("isCastSpell");
        yield return new WaitForSeconds(0.8f);
        GameObject instantSkillB = Instantiate(impactWave,impactWavePosition.position,impactWavePosition.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(instantSkillB);
        StartCoroutine(Think());
    }
}
