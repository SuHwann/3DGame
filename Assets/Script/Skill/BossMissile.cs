using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class BossMissile : Slash
{
    [SerializeField]
    GameObject targetPosition;     //�� ����
    [SerializeField]
    float skillSpeed = 0f;      //���ư��� �ӵ�
    private void Start()
    {
        targetPosition = GameObject.Find("Player");
        StartCoroutine(NavStart());
    }

    IEnumerator NavStart()
    {
        while (true)
        {
            transform.DOMove(targetPosition.transform.position, skillSpeed);
            yield return new WaitForSeconds(1);
        }
    }
}
