using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Switch Property ���� ��ũ��Ʈ
public enum Choice
{
    Mon,
    Tue,
    Wed,
    Thu,
    Fri,
    Sat,
    Sun
}
public class Test : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            State = Choice.Wed;
        }
    }
    //���� ���� 
    [SerializeField]
    Choice choice;
    public Choice State
    {
        get => choice; //���� ���¸� �о��
        set
        {
            Operate = value switch
            {
                Choice.Mon => Mon,
                Choice.Tue => Tue,
                Choice.Wed => Wed,
                _ => null,//switch�� �ش�Ǵ°� ������ null�� 
            };
            Operate?.Invoke();//Operate�� �����ϰڴٴ� ��
        }
    }
    public System.Action Operate;
    void Mon()
    {
        print("�������Դϴ�.");
    }
    void Tue()
    {
        print("ȭ�����Դϴ�.");
    }
    void Wed()
    {
        print("�������Դϴ�.");
    }
}
