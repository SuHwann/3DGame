using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform objectTofollow;
    public float followSpeed = 10f;
    public float sensitibity = 100f;
    public float clamAngle = 70f;

    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 dirNoramlized;
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;

    public float finalDistance;
    public float smoothness = 10f;
    private void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNoramlized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
    }
    private void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitibity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitibity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clamAngle, clamAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
        VecMove();
    }
    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);
        finalDir = transform.TransformPoint(dirNoramlized * maxDistance);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNoramlized * finalDistance, Time.deltaTime * smoothness);
    }
    private void VecMove()
    {
       
    }
    
}
