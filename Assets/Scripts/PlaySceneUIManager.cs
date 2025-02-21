using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal;
using UnityEditor;

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
    private GameObject countDownUI;
    [SerializeField]
    private TextMeshProUGUI countDownText;

    public int lapCount = 2;
    public int currentLapCount = 1;

    private bool isStart = false;

    [SerializeField]
    private Image spImage;

    private float currentPlayTime;

    [SerializeField]
    private StatusController statusController;

    private Coroutine blinkCoroutine;
    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (!GameManager.instance.isStart)
            return;
        SpGagueUpdate();
        CheckPlayTime();
        PlayTimeUIUpdate();
        UpdateLapCountUI();
        PauseScene();
    }

    private void SpGagueUpdate()
    {
        spImage.fillAmount = (float)statusController.currentSp / statusController.sp;
    }

    private void CheckPlayTime()
    {
        if (isStart)
            currentPlayTime += Time.deltaTime;
    }

    private void PlayTimeUIUpdate()
    {
        playTime_Text.text = ((int)(currentPlayTime / 60)).ToString() + " : " + ((int)(currentPlayTime % 60)).ToString();
    }

    private void UpdateLapCountUI()
    {
        lap_Text.text = currentLapCount.ToString() + " / " + lapCount.ToString();
    }

    public void UpdateCurrentLapCount()
    {
        if (currentLapCount >= lapCount)
        {
            Debug.Log("게임 클리어!!");
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

    private void PauseScene()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    private IEnumerator StartGame()
    {
        countDownUI.SetActive(true);

        countDownText.color = Color.yellow;

        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.color = Color.green;
        countDownText.text = "Start";
        yield return new WaitForSeconds(0.5f);

        countDownUI.SetActive(false);

        currentPlayTime = Time.deltaTime;
    }

}
