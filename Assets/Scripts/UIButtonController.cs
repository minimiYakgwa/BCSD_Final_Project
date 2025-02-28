using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;

    private bool isPoint = false;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.S))
            if (EventSystem.current.currentSelectedGameObject != null)
                EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //SoundManager.instance.StopSE("SelectButtonSound");
        //SoundManager.instance.PlaySE("SelectButtonSound");
        if (!isPoint)
        {
            button.Select();
            isPoint = true;
        }
        

        /*var navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPoint = false;
        /*isPoint = false;
        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.Automatic;
        button.navigation = navigation;*/
    }

    public void OnSelect(BaseEventData eventData)
    {

        SoundManager.instance.StopSE("SelectButtonSound");
        SoundManager.instance.PlaySE("SelectButtonSound");
        Debug.Log("키보드로 버튼 선택 중");
    }
}
