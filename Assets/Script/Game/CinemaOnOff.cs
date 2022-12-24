using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaOnOff : MonoBehaviour
{
    public static Action cineAction;
    [SerializeField]
    GameObject cinema;
    [SerializeField]
    Wraith wraith; //보스 스크립트
    private void Awake()
    {
        wraith = FindObjectOfType<Wraith>();
        cineAction = () => { CinemaOn(); };
    }
    public void CinemaOff()
    {
        cinema.SetActive(false);
    }
    public void CinemaOn()
    {
        cinema.SetActive(true);    
    }
    public void BossBattleOn()
    {
        wraith.enabled = true;
        wraith.agent.enabled = true;
    }
}
