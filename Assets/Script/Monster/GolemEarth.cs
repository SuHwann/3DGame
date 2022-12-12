using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class GolemEarth : MonsterAI
{
    [SerializeField]
    GameObject skillA; //원거리 공격 오브젝트
    [SerializeField]
    GameObject impactWave; //광역 스킬 오브젝트
    [SerializeField]
    Transform impactWavePosition; //광역스킬 생성 위치
    [SerializeField]
    Transform skillPointA; //원거리 공격스킬 생성 위치
    bool isLook; //플레이어를 바라볼 방향 체크
    Vector3 lookVec; //플레이어 방향 미리 예상 
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
    //플레이어를 바라봄, 플레이어 움직임 미리 예상
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
                lookVec = new Vector3(h, 0, v) * 1f; //플레이어 입력값으로 예측 벡터값 생성
                transform.LookAt(player.position + lookVec); //플레이어를 바라봄
            }
            else
                agent.SetDestination(tautVec);
            yield return null;
        }
    }
    //랜덤한 공격
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
    //플레이어에게 돌진 근접공격 시작
    IEnumerator RushAttack()
    {
        tautVec = player.position + lookVec;//돌진공격을 할 위치 변수 저장
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
    //원거리 공격 스킬 
    IEnumerator Skill()
    {
        isLook = false;
        anim.SetTrigger("isProjectile Attack");
        yield return new WaitForSeconds(0.8f);
        for(int i =0; i < 3; i++)
        {
            GameObject instantSkillA = Instantiate(skillA, skillPointA.position, skillPointA.rotation);
            instantSkillA.transform.LookAt(player);//플레이어의 방향으로 회전
            instantSkillA.GetComponent<Rigidbody>().AddForce(instantSkillA.transform.forward * speed);  //강체에 힘을 가하여 발사체의 속도를 설정합니다
            yield return new WaitForSeconds(0.2f);
        }
        isLook= true;
        StartCoroutine(Think());
    }
    //광역스킬
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
