using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Vector3 offset;

    private void Update()
    {
        //ī�޶� tartget�� ���󰡰Բ� ����
        transform.position = target.position + offset;
    }
}
