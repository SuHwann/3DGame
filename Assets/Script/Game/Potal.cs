using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Potal : MonoBehaviour
{
    [SerializeField]
    string mapname;     //������ �̸�
    [SerializeField]
    bool nextScene;     //���������� ���� �����̵����� üũ
    [SerializeField]
    Transform teleport; //�����̵� ����
    [SerializeField]
    GameObject cinema;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && nextScene)
        {
            SceneManager.LoadScene(mapname);
        }
        if (other.CompareTag("Player") && nextScene == false)
        {
            other.gameObject.transform.position = teleport.position;
            StartCoroutine(LastBossCinema());
        }

    }
    IEnumerator LastBossCinema()
    {
        yield return new WaitForSeconds(1.5f);
        cinema.SetActive(true);
        yield return null;
    }
}
