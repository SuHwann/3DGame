using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //����Ÿ�� 
    public enum Type { Melee, Range }
    //����ɼ� ����
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
