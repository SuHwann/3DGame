using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill_Ball : Slash
{
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 1.5f;
    bool isShoot; //�⸦ ������ ������ ���� 

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    //�������� ������ �ð�
    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
        rigid.AddForce(transform.forward * 8000f, ForceMode.Force);      //��ų �߻�
    }
    //�������� ������ �ð��� ������ ���� ��� �ݺ� 
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
