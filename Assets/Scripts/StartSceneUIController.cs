using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUIController : MonoBehaviour
{
    private bool isExplainRule = false;

    [SerializeField]
    private float fadeOutTime;

    [SerializeField]
    private Image fadeOutImage;

    [SerializeField]
    private GameObject howToPlayUI;

    [SerializeField]
    private GameObject[] howToPlayUIRules;

    [SerializeField]
    private GameObject nextButton;
    [SerializeField]
    private GameObject closeButton;
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject levelSelectUI;

    private int ruleCount = 0;

    [SerializeField]
    private Button selectLevelButton;

    private const int easy = 2;
    private const int normal = 3;
    private const int hard = 5;
    private void Start()
    {
        SoundManager.instance.PlayBgm("StartSound");
        Time.timeScale = 1;
    }
    private void Update()
    {
        IsPushSpace();
    }

    public void StartGame()
    {
        SoundManager.instance.StopSE("PressButtonSound");
        SoundManager.instance.PlaySE("PressButtonSound");

        SelectLevel();
        
        
    }

    private void SelectLevel()
    {
        menuUI.SetActive(false);
        levelSelectUI.SetActive(true);
        selectLevelButton.Select();
        //StartCoroutine(FadeOutAndLoadScene());
    }

    public void SelectLevelEasy()
    {
        SoundManager.instance.StopSE("PressButtonSound");
        SoundManager.instance.PlaySE("PressButtonSound");
        GameManager.instance.level = easy;
        StartCoroutine(FadeOutAndLoadScene());
    }

    public void SelectLevelNormal()
    {
        SoundManager.instance.StopSE("PressButtonSound");
        SoundManager.instance.PlaySE("PressButtonSound");
        GameManager.instance.level = normal;
        StartCoroutine(FadeOutAndLoadScene());
    }

    public void SelectLevelHard()
    {
        SoundManager.instance.StopSE("PressButtonSound");
        SoundManager.instance.PlaySE("PressButtonSound");
        GameManager.instance.level = hard;
        StartCoroutine(FadeOutAndLoadScene());
    }

    private void IsPushSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isExplainRule)
        {
            //SoundManager.instance.PlaySE("PressButtonSound");
            if (closeButton.activeSelf)
                CloseHowToPlayUI();
            else if (nextButton.activeSelf)
                NextHowToPlayUI();
        }
            

    }
    public void ExplainGameRule()
    {
        SoundManager.instance.StopSE("PressButtonSound");
        SoundManager.instance.PlaySE("PressButtonSound");
        StartCoroutine(SetHowToPlayUI());
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        float currentTime = Time.deltaTime;

        while (currentTime <= fadeOutTime)
        {
            fadeOutImage.fillAmount = currentTime / fadeOutTime;
            currentTime += Time.deltaTime;
            yield return null;
        }

        fadeOutImage.fillAmount = 1f;

        SceneManager.LoadScene(1);
    }

    private IEnumerator SetHowToPlayUI()
    {
        menuUI.SetActive(false);
        howToPlayUI.SetActive(true);
        howToPlayUIRules[0].SetActive(true);
        nextButton.SetActive(true);
        closeButton.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        isExplainRule = true;
    }
    public void NextHowToPlayUI()
    {
        SoundManager.instance.StopSE("PressButtonSound");
        SoundManager.instance.PlaySE("PressButtonSound");
        howToPlayUIRules[ruleCount].SetActive(false);

        ruleCount++;

        if (ruleCount>= howToPlayUIRules.Length-1)
        {
            nextButton.SetActive(false);
            closeButton.SetActive(true);
        }

        howToPlayUIRules[ruleCount].SetActive(true);


    }


    public void CloseHowToPlayUI()
    {
        SoundManager.instance.StopSE("PressButtonSound");
        SoundManager.instance.PlaySE("PressButtonSound");
        menuUI.SetActive(true);
        howToPlayUIRules[ruleCount].SetActive(false);
        ruleCount = 0;
        howToPlayUI.SetActive(false);
        isExplainRule = false;
    }
}
