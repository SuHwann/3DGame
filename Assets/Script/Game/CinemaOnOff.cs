using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaOnOff : MonoBehaviour
{
    public static Action cineAction;

    private void Awake()
    {
        cineAction = () => { CinemaOn(); };
    }
    public void CinemaOff()
    {
        this.gameObject.SetActive(false);
    }
    public void CinemaOn()
    {
        this.gameObject.SetActive(true);    
    }
}
