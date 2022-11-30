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
    bool isLook; //플레이어를 바라볼 뱡향 체크
    Vector3 lookVec; //플레이어 방향 미리 예상
    Vector3 tautVec;

    private void Start()
    {
        agent.isStopped = true;
        isLook = true;
        StartCoroutine(Look());
        StartCoroutine(Think());
    }
    IEnumerator Look()
    {
        while(true)
        {
            if(isDead)
            {
                StopAllCoroutines();
                yield break;
            }
            if(isLook)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 1f; //플레이어 입력값으로 예측 벡터값 생성
                transform.LookAt(player.position + lookVec); //플레이어를 바라봄
            }
            else
                agent.SetDestination(tautVec);
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
                //SkillA 발사 패턴
                break;
            case 2:
            case 3:
                StartCoroutine(SkillB());
                //SkillB 발사 패턴
                break;
            case 4:
                StartCoroutine(SkillJump());
                //Jump Attack
                break;
        }
    }
    //검은 불꽃 발사 
    IEnumerator SkillA()
    {
        anim.SetTrigger("doAttack1");
        yield return new WaitForSeconds(0.2f);
        GameObject instantSkillA = Instantiate(skillA);
        instantSkillA.transform.position = skillpointA.transform.position;  
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }
    //큰 검은 불꽃 발사
    IEnumerator SkillB()
    {
        isLook = false;
        anim.SetTrigger("doAttack2");
        Instantiate(skillB,skillpointB.position,skillpointB.rotation);

        yield return new WaitForSeconds(2.5f);
        isLook = true;
        StartCoroutine(Think());
    }
    //내려 찍는 스킬 
    IEnumerator SkillJump()
    {
        tautVec = player.position + lookVec;

        isLook = false;
        agent.isStopped = false;
        monsterCol.enabled = false;
        anim.SetTrigger("doAttack3");
        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(1f);
        isLook = true;
        agent.isStopped = true;
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
}
