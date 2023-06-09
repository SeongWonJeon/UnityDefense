using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUI : BaseUI, IDragHandler, IPointerDownHandler
{
    protected override void Awake()
    {
        base.Awake();

        buttons["CloseButton"].onClick.AddListener(() => { GameManager.UI.CloseWindowUI(this); });      // ��ư�� ������ �ݾƴ޶�
    }

    public void OnDrag(PointerEventData eventData)      // ���콺�� �巡���ؼ� �˾�â�� �ű� �� �ֵ��� �ϴ� ���
    {
        transform.position += (Vector3)eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.UI.SelectWindodwUI(this);           // Ŭ������ �� ���� �����ߴ�
    }
}
