using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform objectTofollow;
    public float followSpeed = 10f;
    public Transform realCamera;
    private Vector3 dirNoramlized;
    private Vector3 finalDir;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10f;
    private void Start()
    {
        dirNoramlized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
        StartCoroutine(CameraMove());
    }
    IEnumerator CameraMove()
    {
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime); //플레이어 이동 
            finalDir = transform.TransformPoint(dirNoramlized * maxDistance);
            RaycastHit hit;
            if (Physics.Linecast(transform.position, finalDir, out hit))
            {
                finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else { finalDistance = maxDistance; }
            realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNoramlized * finalDistance, Time.deltaTime * smoothness);
            yield return null;
        }
    }
}
