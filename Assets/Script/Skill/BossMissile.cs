using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class BossMissile : Slash
{
    [SerializeField]
    Transform targetPosition;     //�� ����
    [SerializeField]
    float skillSpeed = 0f;      //���ư��� �ӵ�
    private void Start()
    {
        StartCoroutine(NavStart());
    }

    IEnumerator NavStart()
    {
        while (true)
        {
            transform.DOMove(targetPosition.position, skillSpeed);
            yield return new WaitForSeconds(1);
        }
    }
}
