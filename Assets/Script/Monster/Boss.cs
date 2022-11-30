using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonsterAI
{
    [SerializeField]
    GameObject skillA , skillB;
    [SerializeField]
    Transform skillpointA,skillpointB;
    [SerializeField]
    bool isLook; //�÷��̾ �ٶ� ���� üũ
    Vector3 lookVec; //�÷��̾� ���� �̸� ����
    Vector3 tautVec;


    private void Start()
    {
        isLook = true;
        StartCoroutine(Look());
        StartCoroutine(Think());
    }
    IEnumerator Look()
    {
        while(true)
        {
            if(isLook)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 1f; //�÷��̾� �Է°����� ���� ���Ͱ� ����
                transform.LookAt(player.position + lookVec); //�÷��̾ �ٶ�
            }
            yield return null;
        }
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int randomAction = Random.Range(0, 5);
        switch(randomAction)
        {
            case 0:
            case 1:
                StartCoroutine(SkillA());
                //SkillA �߻� ����
                break;
            case 2:
            case 3:
                StartCoroutine(SkillB());
                //SkillB �߻� ����
                break;
            case 4:
                StartCoroutine(SkillJump());
                //Jump Attack
                break;
        }
    }
    IEnumerator SkillA()
    {
        anim.SetTrigger("doAttack1");
        yield return new WaitForSeconds(0.2f);
        GameObject instantSkillA = Instantiate(skillA);
        instantSkillA.transform.position = skillpointA.transform.position;  
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }
    IEnumerator SkillB()
    {
        anim.SetTrigger("doAttack2");
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }
    IEnumerator SkillJump()
    {
        anim.SetTrigger("doAttack3");
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }
}
