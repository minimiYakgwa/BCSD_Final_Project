using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    Button button;

    [SerializeField]
    private float pointerEnterMoveThreshold = 30f;

    private static bool hasLastPointerEnterPosition = false;
    private static Vector2 lastPointerEnterPosition;
    private static GameObject lastSelectedButton;
    private static bool isRestoringSelection = false;

    private bool isPoint = false;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (isPoint && IsMoveButtonInput())
            StartCoroutine(ClearPointerEffectAfterButtonInput());

        if (Input.GetMouseButtonDown(0))
            StartCoroutine(RestoreSelectionAfterBackgroundClick());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //SoundManager.instance.StopSE("SelectButtonSound");
        //SoundManager.instance.PlaySE("SelectButtonSound");
        if (!isPoint && CanUsePointerEnter(eventData.position))
        {
            button.Select();
            isPoint = true;
            lastPointerEnterPosition = eventData.position;
            hasLastPointerEnterPosition = true;
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

        lastSelectedButton = gameObject;

        if (isRestoringSelection)
            return;

        SoundManager.instance.StopSE("SelectButtonSound");
        SoundManager.instance.PlaySE("SelectButtonSound");
        Debug.Log("키보드로 버튼 선택 중");
    }

    private bool CanUsePointerEnter(Vector2 pointerPosition)
    {
        if (!hasLastPointerEnterPosition)
            return true;

        return Vector2.Distance(lastPointerEnterPosition, pointerPosition) >= pointerEnterMoveThreshold;
    }

    private bool IsMoveButtonInput()
    {
        return Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.S);
    }

    private IEnumerator ClearPointerEffectAfterButtonInput()
    {
        yield return null;

        if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == gameObject)
            yield break;

        isPoint = false;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        button.OnPointerExit(pointerEventData);
    }

    private IEnumerator RestoreSelectionAfterBackgroundClick()
    {
        yield return null;

        if (!CanRestoreLastSelectedButton())
            yield break;

        isRestoringSelection = true;
        EventSystem.current.SetSelectedGameObject(lastSelectedButton);
        isRestoringSelection = false;
    }

    private bool CanRestoreLastSelectedButton()
    {
        if (EventSystem.current == null || !EventSystem.current.enabled)
            return false;

        if (EventSystem.current.currentSelectedGameObject != null)
            return false;

        if (lastSelectedButton == null || !lastSelectedButton.activeInHierarchy)
            return false;

        Button lastSelectedButtonComponent = lastSelectedButton.GetComponent<Button>();
        return lastSelectedButtonComponent != null && lastSelectedButtonComponent.interactable;
    }
}
