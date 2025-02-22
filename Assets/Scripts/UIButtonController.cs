using System.Collections;
using System.Collections.Generic;
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.StopSE("SelectButtonSound");
        SoundManager.instance.PlaySE("SelectButtonSound");
        button.Select();
        isPoint = true;

        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPoint = false;
        var navigation = button.navigation;
        navigation.mode = Navigation.Mode.Automatic;
        button.navigation = navigation;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //SoundManager.instance.StopSE("SelectButtonSound");
        SoundManager.instance.PlaySE("SelectButtonSound");
    }

    public void OnButtonClick(BaseEventData eventData)
    {
        SoundManager.instance.PlaySE("PressButtonSound");

    }
}
