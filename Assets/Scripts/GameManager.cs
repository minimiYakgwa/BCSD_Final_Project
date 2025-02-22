using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            Destroy(gameObject);
    }
    #endregion singleton

    public bool isStart = false;

    public float currentLapCount = 1;
    public float lapCount = 1;

    public int bestPlayTimeMin = 0;
    public int bestPlayTimeSec = 0;

    public int currentPlayTimeMin;
    public int currentPlayTimeSec;

    public IEnumerator GamePlayCoroutine()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isStart = false;
        SoundManager.instance.StopAllBgm();

        yield return new WaitForSeconds(3.5f);

        isStart = true;
        SoundManager.instance.PlayBgm("PlaySound");
        SoundManager.instance.PlayBgm("PlaySound2");
    }

    public IEnumerator FinishGame(int clearTimeMin, int clearTimeSec)
    {
        isStart = false;
        Time.timeScale = 0f;

        currentPlayTimeMin = clearTimeMin;
        currentPlayTimeSec = clearTimeSec;

        if (currentPlayTimeMin >= bestPlayTimeMin && currentPlayTimeSec >= bestPlayTimeSec)
        {
            bestPlayTimeSec = currentPlayTimeSec;
            bestPlayTimeMin = currentPlayTimeMin;
        }
            


        SoundManager.instance.StopAllBgm();
        SoundManager.instance.StopAllSE();

        SoundManager.instance.PlayBgm("FinishSound");
        yield return new WaitForSecondsRealtime(3f);

        SoundManager.instance.StopAllSE();
        SoundManager.instance.StopAllBgm();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(2);
    }

    public IEnumerator FailGame()
    {
        yield return new WaitForSeconds(3f);

        isStart = false;
        Time.timeScale = 0f;

        SoundManager.instance.StopAllSE();
        SoundManager.instance.StopAllBgm();


        SoundManager.instance.PlayBgm("FailSound");
        yield return new WaitForSecondsRealtime(3f);

        SoundManager.instance.StopAllSE();
        SoundManager.instance.StopAllBgm();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(2);

        

    }

    public bool isFinish()
    {
        Time.timeScale = 1f;
        if (currentPlayTimeMin > 0 || currentPlayTimeSec > 0)
            return true;
        else return false;
    }

    
}
