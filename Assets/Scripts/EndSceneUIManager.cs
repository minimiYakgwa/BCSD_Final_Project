using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneUIManager : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private TextMeshProUGUI bestRecordText;
    [SerializeField]
    private TextMeshProUGUI bestRecord;
    [SerializeField]
    private TextMeshProUGUI currentRecordText;
    [SerializeField]
    private TextMeshProUGUI currentRecord;
    [SerializeField]
    private TextMeshProUGUI renewText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private GameObject buttons;

    private bool isFade = false;

    [SerializeField]
    private float fadeOutTime;

    [SerializeField]
    private Image fadeOutImage;

    [SerializeField]
    private CanvasGroup inputCanvasGroup;

    private void Start()
    {
        ShowBackground();
        StartCoroutine(ShowRecord());  
    }
    private void ShowBackground()
    {
        if (GameManager.instance.isFinish())
            anim.SetTrigger("Finish");
        else
            anim.SetTrigger("Fail");
    }

    private IEnumerator ShowRecord()
    {
        if (GameManager.instance.level == 2)
        {
            levelText.text = "Easy";
            levelText.color = Color.cyan;
        }
        else if (GameManager.instance.level == 3)
        {
            levelText.text = "Normal";
            levelText.color = Color.cyan;
        }
        else if (GameManager.instance.level == 5)
        {
            levelText.text = "Hard";
            levelText.color = Color.red;
        }
            

        bestRecord.text = GameManager.instance.bestPlayTimeMin.ToString() + " : " + GameManager.instance.bestPlayTimeSec.ToString();
        currentRecord.text = GameManager.instance.currentPlayTimeMin.ToString() + " : " + GameManager.instance.currentPlayTimeSec.ToString();

        yield return new WaitForSeconds(1f);
        bestRecordText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        bestRecord.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (GameManager.instance.bestPlayTimeMin == GameManager.instance.currentPlayTimeMin &&
            GameManager.instance.currentPlayTimeSec == GameManager.instance.bestPlayTimeSec && GameManager.instance.isFinish())
            renewText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        currentRecordText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        currentRecord.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        buttons.SetActive(true);
    }
    public void StartGame()
    {
        if (isFade)
            return;

        SoundManager.instance.PlaySE("PressButtonSound");
        StartCoroutine(FadeOutAndRestart());
    }
    public void ExitGame()
    {
        if (isFade)
            return;

        Application.Quit();
    }
    public void ReturnTitle()
    {
        if (isFade)
            return;

        SoundManager.instance.PlaySE("PressButtonSound");

        StartCoroutine(FadeOutAndReturnTitle());
    }



    private IEnumerator FadeOutAndRestart()
    {
        SetInputEnabled(false);

        float currentTime = Time.deltaTime;

        while (currentTime <= fadeOutTime)
        {
            fadeOutImage.fillAmount = currentTime / fadeOutTime;
            currentTime += Time.deltaTime;
            yield return null;
        }

        fadeOutImage.fillAmount = 1f;

        RestoreEventSystemBeforeSceneLoad();
        SceneManager.LoadScene(1);
    }

    private IEnumerator FadeOutAndReturnTitle()
    {
        SetInputEnabled(false);

        float currentTime = Time.deltaTime;

        while (currentTime <= fadeOutTime)
        {
            fadeOutImage.fillAmount = currentTime / fadeOutTime;
            currentTime += Time.deltaTime;
            yield return null;
        }

        fadeOutImage.fillAmount = 1f;

        RestoreEventSystemBeforeSceneLoad();
        SceneManager.LoadScene(0);
    }

    private void RestoreEventSystemBeforeSceneLoad()
    {
        if (EventSystem.current != null)
            EventSystem.current.enabled = true;
    }

    private void SetInputEnabled(bool isEnabled)
    {
        isFade = !isEnabled;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.enabled = isEnabled;
        }

        if (inputCanvasGroup == null)
            return;

        inputCanvasGroup.interactable = isEnabled;
        inputCanvasGroup.blocksRaycasts = isEnabled;
    }

}
