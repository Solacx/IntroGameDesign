using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Color32 baseColor;
    [SerializeField] Color32 onHoverColor;
    [SerializeField] Color32 onClickColor;

    private TextMeshProUGUI TMP;

    void Start() {
        TMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        TMP.color = onClickColor;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        TMP.color = onHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        TMP.color = baseColor;
    }
}
