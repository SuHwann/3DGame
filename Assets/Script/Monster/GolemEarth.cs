using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class GolemEarth : MonsterAI
{
    [SerializeField]
    GameObject skillA; //���Ÿ� ���� ������Ʈ
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
        int randomAction = Random.Range(0, 1);
        switch (1)
        {
/*            case 0:
                StartCoroutine(RushAttack());
                break;*/
            case 1:
                StartCoroutine(Skill());
                break;
        }
    }
    //�÷��̾�� ���� �������� ����
    IEnumerator RushAttack()
    {
        print("����");
        tautVec = player.position + lookVec;//���������� �� ��ġ ���� ����
        isLook = false;
        agent.isStopped = false;
        monsterCol.enabled = false;
        anim.SetTrigger("isPunch");
        meleeArea.enabled = true;
        yield return new WaitForSeconds(2f);
        meleeArea.enabled = false;
        isLook= true;
        agent.isStopped=true;
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //���Ÿ� ���� ��ų 
    IEnumerator Skill()
    {
        isLook = false;
        anim.SetTrigger("isProjectile Attack");
        yield return new WaitForSeconds(0.8f);
        GameObject instantSkillA = Instantiate(skillA,skillPointA.position,skillPointA.rotation);
        instantSkillA.transform.localScale = Vector3.one * 3f;
        instantSkillA.transform.LookAt(player);//�÷��̾��� �������� ȸ��
        instantSkillA.GetComponent<Rigidbody>().AddForce(instantSkillA.transform.forward * speed);  //��ü�� ���� ���Ͽ� �߻�ü�� �ӵ��� �����մϴ�.
        yield return new WaitForSeconds(2.5f);
        isLook= true;
        StartCoroutine(Think());
    }
}
