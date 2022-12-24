using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith : MonsterAI
{
    #region 변수
    bool isLook; //플레이어를 바라볼지 판단
    [SerializeField]
    Transform skillPoint; //스킬생성 위치
    [SerializeField]
    GameObject skillOb;   //스킬 오브젝트
    [SerializeField]
    RectTransform bossHealthGroup, bossHealthBar; //보스 체력 UI
    Vector3 lookVec; //플레이어 방향 미리 예상 벡터
    [SerializeField]
    float skillSpeed; //스킬 스피드
    GameManager manager; //게임메니저
    #endregion
    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        isLook = true;
        agent.isStopped = true;
        StartCoroutine(Look());
        StartCoroutine(BossHealth());
        StartCoroutine(Think());
    }
    //플레이어를 바라봄 , 플레이어 움직임 미리 예상
    IEnumerator Look()
    {
        while(true)
        {
            if (isDead)
            {
                manager.bossText.text = "x 1";
                StopAllCoroutines();
            }
            //Input입력값을 받아와서 방향을 1f 미리 예상한다
            if (isLook)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 1f; //플레이어 입력값으로 예측 벡터값 생성
                transform.LookAt(player.position + lookVec); //플레이어를 바라봄
            }
            if (!isLook || !isDead) { agent.SetDestination(player.position); }
            yield return null;
        }
    }
    //랜덤한 공격
    IEnumerator Think()
    {
        yield return new WaitForSeconds(2f);
        int randomAction = Random.Range(0, 2);
        switch (randomAction)
        {
            case 0:
                StartCoroutine(RushAttack());
                break;
            case 1:
                StartCoroutine(FarSkill());
                break;
            case 2:
                StartCoroutine(WideSkill());
                break;
        }
        print("생각중");
    }
    //플레이어에게 돌진 근접 공격 시작
    IEnumerator RushAttack()
    {
        float targetRadius = 10f;
        float targetRange = 33f;
        isLook = false;
        agent.isStopped = false;
        while (true)
        {
            RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
            LayerMask.GetMask("Player"));
            if (rayHits.Length > 0)
            {
                agent.isStopped = true;
                anim.SetTrigger("doRushAttack");
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
    IEnumerator FarSkill()
    {
        isLook = true;
        agent.isStopped = false;
        anim.SetTrigger("doFarSkill");
        monsterCol.enabled = false;
        yield return new WaitForSeconds(0.8f);
        agent.isStopped = true;
        GameObject instantSkill = Instantiate(skillOb, skillPoint.position, skillPoint.rotation);
        yield return new WaitForSeconds(1f);
        monsterCol.enabled = true;
        isLook = false;
        Destroy(instantSkill);
        StartCoroutine(Think());
        yield return null;
    }
    //광역스킬
    IEnumerator WideSkill()
    {
        yield return null;
    }
    //몬스터 체력 UI 
    IEnumerator BossHealth()
    {
        while (true)
        {
            bossHealthBar.localScale = new Vector3((float)curHealth / maxHealth, 1, 1);
            if (curHealth < 0) { bossHealthBar.localScale = Vector3.zero; };
            yield return null;
        }
    }
}
