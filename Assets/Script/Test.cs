using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Switch Property 공부 스크립트
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
    //현재 상태 
    [SerializeField]
    Choice choice;
    public Choice State
    {
        get => choice; //현재 상태를 읽어옴
        set
        {
            Operate = value switch
            {
                Choice.Mon => Mon,
                Choice.Tue => Tue,
                Choice.Wed => Wed,
                _ => null,//switch에 해당되는게 없으면 null로 
            };
            Operate?.Invoke();//Operate를 실행하겠다는 뜻
        }
    }
    public System.Action Operate;
    void Mon()
    {
        print("월요일입니다.");
    }
    void Tue()
    {
        print("화요일입니다.");
    }
    void Wed()
    {
        print("수요일입니다.");
    }
}
