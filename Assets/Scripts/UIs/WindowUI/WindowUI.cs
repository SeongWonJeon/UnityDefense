using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUI : BaseUI, IDragHandler, IPointerDownHandler
{
    protected override void Awake()
    {
        base.Awake();

        buttons["CloseButton"].onClick.AddListener(() => { GameManager.UI.CloseWindowUI(this); });      // 버튼을 누르면 닫아달라
    }

    public void OnDrag(PointerEventData eventData)      // 마우스로 드래그해서 팝업창을 옮길 수 있도록 하는 방법
    {
        transform.position += (Vector3)eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.UI.SelectWindodwUI(this);           // 클릭했을 때 나를 선택했다
    }
}
