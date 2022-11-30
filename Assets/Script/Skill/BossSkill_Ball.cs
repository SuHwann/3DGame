using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill_Ball : Slash
{
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 1.5f;
    bool isShoot; //기를 모으고 관리할 변수 

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    //에너지를 모으는 시간
    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
        rigid.AddForce(transform.forward * 8000f, ForceMode.Force);      //스킬 발사
    }
    //에너지를 모으는 시간이 끝날때 까지 계속 반복 
    IEnumerator GainPower()
    {
        while(!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}
