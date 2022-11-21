using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //무기타입 
    public enum Type { Melee, Range }
    //무기옵션 변수
    [SerializeField]
    private Type type;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float rate;
    [SerializeField]
    private BoxCollider meleeArea;
    [SerializeField]
    private TrailRenderer trailEffect;
}
