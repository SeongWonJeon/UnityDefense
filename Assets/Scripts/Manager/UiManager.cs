using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{
    private EventSystem eventSystem;

    private Canvas popUpCanvas;
    private Stack<PopUpUI> popUpStack;

    private Canvas windowCanvas;
    private List<WindowUI> windowList;  // �˾�â���� �ٸ��� ������â�� ����Ʈ�� �����ؾ� ����ϴµ� �� ���ϴ�

    private Canvas inGameCanvas;

    private void Awake()
    {
        eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
        eventSystem.transform.SetParent(transform);

        popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        popUpCanvas.gameObject.name = "PopUpCanvas";
        popUpCanvas.sortingOrder = 100;
        popUpStack = new Stack<PopUpUI>();

        windowCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        windowCanvas.gameObject.name = "windowCanvas";
        windowCanvas.sortingOrder = 10;

        // gameSceneCanvas.sortingOrder = 1;

        inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        inGameCanvas.gameObject.name = "inGameCanvas";
        inGameCanvas.sortingOrder = 0;
    }

    public T ShowPopUpUI<T>(T popUpUI) where T : PopUpUI
    {
        if (popUpStack.Count > 0)                   // ���� �ִ� UI�� ��� �Ⱥ��̵���
        {
            PopUpUI prevUI = popUpStack.Peek();
            prevUI.gameObject.SetActive(false);
        }

        T ui = GameManager.Pool.GetUI(popUpUI);
        ui.transform.SetParent(popUpCanvas.transform, false);       // bool�� �ڽ����� �� �� �θ��� ������� transform�� ���󰡳� �Ȱ��ĸ� ǥ��

        popUpStack.Push(ui);

        Time.timeScale = 0;     // ������ �Ͻ������Ѵ�

        return ui;
    }

    public T ShowPopUpUI<T>(string path) where T : PopUpUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowPopUpUI(ui);
    }

    public void ClosePopUpUI()
    {
        PopUpUI ui = popUpStack.Pop();
        GameManager.Pool.ReleaseUI(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            PopUpUI curUI = popUpStack.Peek();
            curUI.gameObject.SetActive(true);
        }
        else if (popUpStack.Count == 0)          // ������ ī��Ʈ�� 0�̸�
        {
            Time.timeScale = 1f;            //  �ٽ� �ð��� �帣����
        }

    }

    public void ShowWindowUI(WindowUI windowUI)
    {
        WindowUI ui = GameManager.Pool.GetUI(windowUI);
        ui.transform.SetParent(windowCanvas.transform, false);
    }

    public void ShowWindowUI(string path)
    {
        WindowUI ui = GameManager.Resource.Load<WindowUI>(path);
        ShowWindowUI(ui);
    }

    public void SelectWindodwUI(WindowUI windowUI)
    {
        windowUI.transform.SetAsLastSibling();          // SetAsLastSibling �ڽĵ��߿��� ���������� �����Ѵ�
    }

    public void CloseWindowUI(WindowUI windowUI)
    {
        GameManager.Pool.ReleaseUI(windowUI.gameObject);
    }

    public T ShowInGameUI<T>(T inGameUI) where T : InGameUI
    {
        T ui = GameManager.Pool.GetUI(inGameUI);
        ui.transform.SetParent(inGameCanvas.transform, false);

        return ui;
    }

    public T ShowInGameUI<T>(string path) where T : InGameUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowInGameUI(ui);
    }

    public void CloseInGameUI<T>(T inGameUI) where T : InGameUI
    {
        GameManager.Pool.ReleaseUI(inGameUI.gameObject);
    }
}
