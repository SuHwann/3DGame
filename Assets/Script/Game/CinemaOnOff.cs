using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaOnOff : MonoBehaviour
{
    public static Action cineAction;
    [SerializeField]
    GameObject cinema;

    private void Awake()
    {
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
}
