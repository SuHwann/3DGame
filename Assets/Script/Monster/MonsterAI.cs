using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    Transform[] movePoint;    //���� ��ǥ ��ġ ����
    NavMeshAgent agent;       //NMA ����
    Vector3 dePosition;       //���� ��ġ 
    int randomInt;            //movePoint ������ �����ϰ� ���� �� ����
    Rigidbody rigid;          //�浹�� �Ͼ�� ���� ��Ȳ ���� ����
    private void Awake()
    {
        GetPoint();
        randomInt = Random.Range(0, movePoint.Length); //���� ���� ����
        dePosition = transform.position; //ù���۽� ��ġ ����
        StartCoroutine(AiMonster());
    }
    void GetPoint()  //Point���� ��ġ�� �ν����Ϳ� �����Ѵ�.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
    }
    IEnumerator AiMonster()
    {
        while(true)
        {
            agent.SetDestination(movePoint[randomInt].transform.position); //ai���� �������� �̵� ����
            FreezeVelocity();
            Destination();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    void FreezeVelocity()   //ai ���� �浹�� �ڷ� �и��� �浹 ���� 
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    void Destination() //�������� �����ϸ� ������ ����
    {
        if (Vector3.Distance(transform.position, movePoint[randomInt].transform.position) < 1f)
        {
            randomInt = Random.Range(0, movePoint.Length);
        }
    }
}
