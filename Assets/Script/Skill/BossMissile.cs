using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class BossMissile : Slash
{
    [SerializeField]
    Transform targetPosition;     //끝 지점
    [SerializeField]
    float skillSpeed = 0f;      //날아가는 속도
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
