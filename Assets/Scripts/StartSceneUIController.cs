using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartSceneUIController : MonoBehaviour
{
    
    private void Start()
    {
        Time.timeScale = 1;
        SoundManager.instance.PlayBgm("StartSound");
    }
    public void StartGame()
    {
        StartCoroutine(Delay());
        SceneManager.LoadScene(1);
    }

    public void ExplainGameRule()
    {

    }
    public void ExitGame()
    {
        StartCoroutine(Delay());
        Application.Quit();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
    }
}
