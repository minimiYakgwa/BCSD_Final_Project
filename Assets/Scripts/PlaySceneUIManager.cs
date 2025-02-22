using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlaySceneUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playTime_Text;
    [SerializeField]
    private TextMeshProUGUI lap_Text;
    [SerializeField]
    private Image runKeyImage;
    [SerializeField]
    private TextMeshProUGUI runKeyText;
    [SerializeField]
    private Image jumpKeyImage;
    [SerializeField]
    private TextMeshProUGUI jumpKeyText;

    [SerializeField]
    private TextMeshProUGUI countDownText;
    [SerializeField]
    private GameObject menuUI;

    public int lapCount = 2;
    public int currentLapCount = 1;

    public bool isPause = false;

    [SerializeField]
    private Image spImage;

    private float currentPlayTime;
    private int currentPlayTimeMin;
    private int currentPlayTimeSec;

    [SerializeField]
    private StatusController statusController;

    private Coroutine blinkCoroutine;
    private void Start()
    {
        StartCoroutine(GameManager.instance.GamePlayCoroutine());
        StartCoroutine(GameStartCountUI());
    }

    private void Update()
    {
        PauseScene();

        if (GameManager.instance.isStart)
        {
            SpGagueUpdate();
            CheckPlayTime();
            PlayTimeUIUpdate();
            UpdateLapCountUI();
        }
        
        
    }

    private void SpGagueUpdate()
    {
        spImage.fillAmount = (float)statusController.currentSp / statusController.sp;
    }

    private void CheckPlayTime()
    {
        currentPlayTime += Time.deltaTime;
        currentPlayTimeMin = (int)(currentPlayTime / 60);
        currentPlayTimeSec = (int)(currentPlayTime % 60);
    }

    private void PlayTimeUIUpdate()
    {
        playTime_Text.text = currentPlayTimeMin.ToString() + " : " + currentPlayTimeSec.ToString();
    }

    private void UpdateLapCountUI()
    {
        lap_Text.text = currentLapCount.ToString() + " / " + lapCount.ToString() + " lap";
    }

    public void UpdateCurrentLapCount()
    {
        if (currentLapCount >= lapCount)
        {
            countDownText.gameObject.SetActive(true);
            countDownText.text = "FINISH!!";
            StartCoroutine(GameManager.instance.FinishGame(currentPlayTimeMin, currentPlayTimeSec));
        }
        else
        {
            currentLapCount++;
        }
    }

    public void BlinkRunKeyImage(bool _isRun)
    {
        if (_isRun)
        {
            runKeyImage.color = Color.red;
            runKeyText.color = Color.red;
        }
        else
        {
            runKeyImage.color = Color.white;
            runKeyText.color = Color.white;
        }
    }

    public void BlinkJumpKeyImage()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkJumpKeyImageCoroutine());   
    }

    private IEnumerator BlinkJumpKeyImageCoroutine()
    {
        jumpKeyImage.color = Color.gray;
        jumpKeyText.color = Color.gray;

        yield return new WaitForSeconds(0.5f);

        jumpKeyImage.color = Color.white;
        jumpKeyText.color = Color.white;
    }

    

    private IEnumerator GameStartCountUI()
    {
        menuUI.SetActive(false);
        countDownText.gameObject.SetActive(true);

        countDownText.color = Color.yellow;

        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.color = Color.green;
        countDownText.text = "Start";
        yield return new WaitForSeconds(0.5f);

        countDownText.gameObject.SetActive(false);

        currentPlayTime = Time.deltaTime;
    }

    public void ShowGameOverUI()
    {
        countDownText.text = "GAME OVER..";
        countDownText.gameObject.SetActive(true);
    }
    public void ActiveMenuUI()
    {
        menuUI.SetActive(GameManager.instance.isStart);
        GameManager.instance.isStart = !GameManager.instance.isStart;

        if (GameManager.instance.isStart)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else
        {
            SoundManager.instance.StopAllSE();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    public void RestartScene()
    {
        ActiveMenuUI();
        SceneManager.LoadScene(1);
    }

    private void PauseScene()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.isStart)
        {
            ActiveMenuUI();
            
        }
    }
    public void ReturnTitle()
    {
        SoundManager.instance.StopAllBgm();
        SoundManager.instance.StopAllSE();

        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
