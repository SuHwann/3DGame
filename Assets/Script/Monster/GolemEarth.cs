using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public RectTransform bossHealthGroup, bossHealthBar;
    bool isLook; //플레이어를 바라볼 방향 체크
    Vector3 lookVec; //플레이어 방향 미리 예상 
    Vector3 tautVec;
    public float speed = 1000f;
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
                StopAllCoroutines();
            }
            if (isLook)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 1f; //플레이어 입력값으로 예측 벡터값 생성
                transform.LookAt(player.position + lookVec); //플레이어를 바라봄
            }
            if(!isLook || !isDead) { agent.SetDestination(tautVec); }
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
            float targetRadius = 3f;
            float targetRange = 5f;
            isLook = false;
            agent.isStopped = false;
            while (true)
            {
                 tautVec = player.position + lookVec;//돌진공격을 할 위치 변수 저장*/
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
    //원거리 공격 스킬 
    IEnumerator Skill()
    {
        isLook= true;
        anim.SetTrigger("isProjectile Attack");
        monsterCol.enabled = false;
        yield return new WaitForSeconds(0.8f);
        for(int i =0; i < 3; i++)
        {
            GameObject instantSkillA = Instantiate(skillA, skillPointA.position, skillPointA.rotation);
            instantSkillA.transform.LookAt(player);//플레이어의 방향으로 회전
            instantSkillA.GetComponent<Rigidbody>().AddForce(instantSkillA.transform.forward * speed);  //강체에 힘을 가하여 발사체의 속도를 설정합니다
            yield return new WaitForSeconds(0.2f);
        }
        monsterCol.enabled = true;
        StartCoroutine(Think());
    }
    //광역스킬
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
    //몬스터 체력 UI 
    IEnumerator BossHealth()
    {
        while(true)
        {
            bossHealthBar.localScale = new Vector3((float)curHealth / maxHealth, 1, 1);
            if (curHealth < 0) { bossHealthBar.localScale = Vector3.zero; };
            yield return null;
        }
    }
    //시네마틱이 끝났을때
    public void SignalBattleOn()
    {
        GetComponent<GolemEarth>().enabled = true;
        cinema.SetActive(false);
    }
}
