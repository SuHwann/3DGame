using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith : MonsterAI
{
    #region ����
    bool isLook; //�÷��̾ �ٶ��� �Ǵ�
    [SerializeField]
    Transform skillPoint; //��ų���� ��ġ
    [SerializeField]
    GameObject skillOb;   //��ų ������Ʈ
    [SerializeField]
    RectTransform bossHealthGroup, bossHealthBar; //���� ü�� UI
    Vector3 lookVec; //�÷��̾� ���� �̸� ���� ����
    [SerializeField]
    float skillSpeed; //��ų ���ǵ�
    GameManager manager; //���Ӹ޴���
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
    //�÷��̾ �ٶ� , �÷��̾� ������ �̸� ����
    IEnumerator Look()
    {
        while(true)
        {
            if (isDead)
            {
                manager.bossText.text = "x 1";
                StopAllCoroutines();
            }
            //Input�Է°��� �޾ƿͼ� ������ 1f �̸� �����Ѵ�
            if (isLook)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 1f; //�÷��̾� �Է°����� ���� ���Ͱ� ����
                transform.LookAt(player.position + lookVec); //�÷��̾ �ٶ�
            }
            if (!isLook || !isDead) { agent.SetDestination(player.position); }
            yield return null;
        }
    }
    //������ ����
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
        print("������");
    }
    //�÷��̾�� ���� ���� ���� ����
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
    //���Ÿ� ���� ��ų 
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
    //������ų
    IEnumerator WideSkill()
    {
        yield return null;
    }
    //���� ü�� UI 
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
