using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    // 해당 게임오브젝트의 이름가지고 사용을 하자string
    protected Dictionary<string, RectTransform> transforms;
    protected Dictionary<string, Button> buttons;
    protected Dictionary<string, TMP_Text> texts;
    // TODO : add ui component

    protected virtual void Awake()
    {
        BindChildren();
    }
    private void BindChildren()
    {
        transforms = new Dictionary<string, RectTransform>();       // ui컴포넌트들의 초기화
        buttons = new Dictionary<string, Button>();
        texts = new Dictionary<string, TMP_Text>();

        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform child in children)
        {
            string key = child.gameObject.name;

            if (transforms.ContainsKey(key))
                continue;

            transforms.Add(key, child);

            Button button = child.GetComponent<Button>();
            if (button != null )
                buttons.Add(key, button);

            TMP_Text text = child.GetComponent<TMP_Text>();
            if(text != null )
                texts.Add(key, text);
        }
    }
}
