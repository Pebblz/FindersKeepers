using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Focus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AudioSource hover;
    [SerializeField] AudioSource click;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover.Stop();
    }
}
