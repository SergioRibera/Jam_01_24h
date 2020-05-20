using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InputButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool isPlayerInteractable = true;
    public string nameButton;

    public Color colorPress;

    [FormerlySerializedAs("onClick")]
    [SerializeField]
    public Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    public Button.ButtonClickedEvent m_OnClickUp = new Button.ButtonClickedEvent();


    Color colorDefault = Color.white;
    Image i = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPlayerInteractable) return;
        ManagerDataPlayer.SetButton(nameButton, true);
        m_OnClick?.Invoke();
        if (colorDefault == Color.white)
            colorDefault = GetComponent<Image>().color;
        if (i == null)
            i = GetComponent<Image>();
        i.color = colorPress;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPlayerInteractable) return;
        ManagerDataPlayer.SetButton(nameButton, false);
        m_OnClickUp?.Invoke();
        i.color = colorDefault;
    }
}