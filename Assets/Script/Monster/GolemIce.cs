using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIce : MonsterAI
{
    [SerializeField]
    GameObject skill; //원거리 공격 오브젝트
    [SerializeField]
    Transform skillPoint; //원거리 공격스킬 생성 위치
    [SerializeField]
    GameObject impactWave; //광역 스킬 오브젝트
    [SerializeField]
    Transform impactWavePosition; //광역스킬 생성 위치
    public RectTransform bossHealthGroup, bossHealthBar;
    bool isLook; //플레이어를 바라볼 방향 체크
    Vector3 lookVec; //플레이어 방향 미리 예상 
    Vector3 tautVec;
    [SerializeField]
    int speed; //원거리 공격 투사체 속도
    [SerializeField]
    GameObject cinema; //시네마씬
    GameManager manager; //게임메니저

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        agent.isStopped = true;
        isLook = true;
        StartCoroutine(Look());
        StartCoroutine(BossHealth());
        StartCoroutine(Think());
    }
    //플레이어를 바라봄, 플레이어 움직임 미리 예상
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
                lookVec = new Vector3(h, 0, v) * 1f; //플레이어 입력값으로 예측 벡터값 생성
                transform.LookAt(player.position + lookVec); //플레이어를 바라봄
            }
            if (Vector3.Distance(transform.position, player.position) < 25f)
            {
                agent.isStopped = true;
            }
            else { agent.SetDestination(tautVec); }
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
        tautVec = player.position + lookVec;//돌진공격을 할 위치 변수 저장*/
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
    //원거리 공격 스킬 
    IEnumerator Skill()
    {
        anim.SetTrigger("isShot");
        monsterCol.enabled = false;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {
            GameObject instantSkill = Instantiate(skill, skillPoint.position, skillPoint.rotation);
            instantSkill.transform.LookAt(player);//플레이어의 방향으로 회전 
            instantSkill.GetComponent<Rigidbody>().AddForce(instantSkill.transform.forward * speed); //강하게 힘을 가하여 발사체 속도 설정
            yield return new WaitForSeconds(0.2f);
        }
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //광역스킬
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
    //보스몬스터 UI
    IEnumerator BossHealth()
    {
        while (true)
        {
            bossHealthBar.localScale = new Vector3((float)curHealth / maxHealth, 1, 1);
            if (curHealth < 0) { bossHealthBar.localScale = Vector3.zero; };
            yield return null;
        }
    }
    //시네마틱이 끝나면
    public void SignalBattleOn()
    {
        GetComponent<GolemIce>().enabled = true;
        cinema.SetActive(false);
    }
}
