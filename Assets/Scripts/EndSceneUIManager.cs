using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private GameObject buttons;

    private void Start()
    {
        ShowBackground();
        StartCoroutine(ShowRecord());    
    }
    private void ShowBackground()
    {
        SoundManager.instance.PlayBgm("StartSound");
        if (GameManager.instance.isFinish())
            anim.SetTrigger("Finish");
        else
            anim.SetTrigger("Fail");
    }

    private IEnumerator ShowRecord()
    {
        bestRecord.text = GameManager.instance.bestPlayTimeMin.ToString() + " : " + GameManager.instance.bestPlayTimeSec.ToString();
        currentRecord.text = GameManager.instance.currentPlayTimeMin.ToString() + " : " + GameManager.instance.currentPlayTimeSec.ToString();

        yield return new WaitForSeconds(1f);
        bestRecordText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        bestRecord.gameObject.SetActive(true);
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
        StartCoroutine(Delay());
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        StartCoroutine(Delay());
        Application.Quit();
    }
    public void ReturnTitle()
    {
        StartCoroutine(Delay());
        SceneManager.LoadScene(0);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
    }
}
