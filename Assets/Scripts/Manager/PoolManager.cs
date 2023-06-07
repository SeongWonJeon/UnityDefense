using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;     // 유니티 오브젝트 풀이 생겨서 사용해주면 좋을 거 같다

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;
    Dictionary<string, Transform> poolContainer;
    Transform poolRoot;
    Canvas canvasRoot;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer = new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
        canvasRoot = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, prefab);

            GameObject obj = poolDic[key].Get();
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Get<T>(original, position, rotation, null);
    }

    public T Get<T>(T original, Transform parent) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, parent);
    }

    public T Get<T>(T original) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, null);
    }

    public bool Release<T>(T instance) where T : Object
    {
        if (instance is GameObject)
        {
            GameObject go = instance as GameObject;
            string key = go.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(go);
            return true;
        }
        else if (instance is Component)
        {
            Component component = instance as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(component.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsContain<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;

        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    // UI용 pooling
    private void CreatePool(string key, GameObject prefab)
    {
        GameObject root = new GameObject();
        root.gameObject.name = $"{key}Container";
        root.transform.parent = poolRoot;
        poolContainer.Add(key, root.transform);

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
                obj.transform.parent = null;
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.parent = poolContainer[key];
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );
        poolDic.Add(key, pool);
    }

    public T GetUI<T>(T original, Vector3 position) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject obj = poolDic[key].Get();
            obj.transform.position = position;
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            obj.transform.position = position;
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public T GetUI<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject obj = poolDic[key].Get();
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public bool ReleaseUI<T>(T instance) where T : Object
    {
        if (instance is GameObject)
        {
            GameObject go = instance as GameObject;
            string key = go.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(go);
            return true;
        }
        else if (instance is Component)
        {
            Component component = instance as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(component.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CreateUIPool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(canvasRoot.transform, false);
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );
        poolDic.Add(key, pool);
    }
}
/*public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;      // 오브젝트들이 여러개가 있을테니 Dictionary <string> 으로 이름가지고 찾으면 좋을 거 같으니 사용해주자
    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
    }
    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object // 제약조건으로 where T : Object를 사용해서 게임오브젝트만
    {
        if (original is GameObject)     // 만약에 받은게 게임오브젝트다
        {
            GameObject prefab = original as GameObject;

            if (!poolDic.ContainsKey(name))         // 안가지고 있으면 만든다
                CreatePool(prefab.name, prefab);    // 이름 잘못쓰지말고 그 prefab에 이름가지고 써주자

            ObjectPool<GameObject> pool = poolDic[prefab.name];
            GameObject go = pool.Get();
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go as T; // 게임오브젝트를 T로 형변환해서 리턴
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(component.gameObject.name))
                CreatePool(key, component.gameObject);

            GameObject go = poolDic[key].Get();
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go.GetComponent<T>();    // 게임오브젝트의 GetComponent를 통해서 반환
        }
        else
        {
            return null;
        }
    }
    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, prefab);

            GameObject obj = poolDic[key].Get();
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }
    *//*public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDic.ContainsKey(name))         // 안가지고 있으면 만든다
            CreatePool(prefab.name, prefab);    // 이름 잘못쓰지말고 그 prefab에 이름가지고 써주자

        ObjectPool<GameObject> pool = poolDic[prefab.name];
        GameObject go = pool.Get();
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }*//*

    public T Get<T>(T original) where T : Object // 가져다 쓰기
    {
        return Get(original, Vector3.zero, Quaternion.identity);      // 오버로딩을 통해 위의 함수와 같으니 이렇게 해준다
    }

    public bool IsContain<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;

        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public bool Release(GameObject go)     // 반납하기
    {
        if (!poolDic.ContainsKey(go.name))
            return false;

        ObjectPool<GameObject> pool = poolDic[go.name];
        pool.Release(go);
        return true;
    }

    private void CreatePool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>               // 람다식으로 작성
            {
                GameObject go = Instantiate(prefab);       // 만들어질때
                go.name = key;
                return go;
            },
            actionOnGet: (GameObject go) =>
            {
                go.SetActive(true);
                go.transform.SetParent(null);               // 사용할 때
            },
            actionOnRelease: (GameObject go) =>
            {
                go.SetActive(false);
                go.transform.SetParent(transform);          // 반납할 때
            },
            actionOnDestroy: (GameObject go) =>
            {
                Destroy(go);                                // 삭제할 때
            }
            );
        poolDic.Add(key, pool);

    }
}*/
