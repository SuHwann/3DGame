using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Coroutine co = null;
    // Start is called before the first frame update
    void Start()
    {
        co = StartCoroutine(CoroutineTest());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            print("k�� �������ϴ�");
            StopCoroutine(co); 
        }
    }
    IEnumerator CoroutineTest()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            print("�ڷ�ƾ ����");
            yield return null;
        }
    }
}
