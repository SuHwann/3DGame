using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 offset;
    public float followSpeed = 0.15f;

    [SerializeField]
    GameObject player;

    private void Update()
    {
        Vector3 camera_Pos = player.transform.position + offset;
        Vector3 lerp_Pos = Vector3.Lerp(transform.position, camera_Pos, followSpeed);
        transform.position = lerp_Pos;
        transform.LookAt(player.transform);
    }
}