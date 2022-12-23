using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Potal : MonoBehaviour
{
    [SerializeField]
    string mapname;     //다음씬 이름
    [SerializeField]
    bool nextScene;     //다음씬으로 갈지 순간이동할지 체크
    [SerializeField]
    Transform teleport; //순간이동 지점
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
