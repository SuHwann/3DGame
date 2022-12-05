using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform objectTofollow;   //카메라가 바라볼 플레이어 중심
    [SerializeField] 
    Transform realCamera;               //mainCamera
    public float followSpeed = 10f;     //카메라가 따라다닐 속도 
    private Vector3 dirNoramlized;      
    private Vector3 finalDir;           //최종 거리 
    public float minDistance;           //플레이어와의 최소 거리
    public float maxDistance;           //플레이어와의 최대 거리
    public float finalDistance;         //최종 거리     
    public float smoothness = 10f;      //따라다닐 속도를 얼마나 부드럽게 해줄것인가
    private void Start()
    {
        dirNoramlized = realCamera.localPosition.normalized;    //정규화
        finalDistance = realCamera.localPosition.magnitude;     //거리초기화
        StartCoroutine(CameraMove());
    }
    //플레이어를 목표로 카메라가 따라다님
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
