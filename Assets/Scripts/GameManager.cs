using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region singleton
    static public GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(instance);
    }
    #endregion singleton

    public bool isStart = false;

    private void Start()
    {
        StartScene();
    }

    private void StartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "PlayScene")
        {
            StartCoroutine(PlayScene());
        }
    }

    private IEnumerator PlayScene()
    {
        isStart = false;
        SoundManager.instance.StopAllBgm();

        yield return new WaitForSeconds(3.5f);
        
        isStart = true;
        SoundManager.instance.PlayBgm("PlaySound");
        SoundManager.instance.PlayBgm("PlaySound2");
    }
}
