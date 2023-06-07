using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro.EditorUtilities;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static PoolManager pool;
    private static ResourceManager resource;
    private static UiManager ui;

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return pool; } }
    public static ResourceManager Resource { get { return resource; } }
    public static UiManager UI { get { return ui; } }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject resourceManager = new GameObject();
        resourceManager.name = "ResourceManager";
        resourceManager.transform.parent = transform;
        resource = resourceManager.AddComponent<ResourceManager>();         // 순서도 중요하다

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        pool = poolObj.AddComponent<PoolManager>();

        GameObject uiObj = new GameObject();
        uiObj.name = "UiManager";
        uiObj.transform.parent = transform;
        ui = uiObj.AddComponent<UiManager>();

    }
}
