using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    Dictionary<string, Object> resources = new Dictionary<string, Object>();    // FlyWight패턴

    /// <summary>
    /// 참조를하기 위해서 직렬화하기 위한 것들이나 깜빡하고 넣지않거나 하는것을 방지하기위해서 사용을 한다.
    /// 한 번 생성되고 다음번에 사용할때는 생성했던 걸 보관을 하니 보관된 걸 사용하게 된다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object      // 이름으로 불러오는 Load함수
    {
        string key = $"{typeof(T)}.{path}";     // typeof하고 path경로를 key값으로 하신다

        if (resources.ContainsKey(key))     //resources에 똑같은 키값이 있드면 아래를 반환
            return resources[key] as T;

        // 없으면 새로로딩을 해서 넣어줘야하니까
        T resource = Resources.Load<T>(path);
        resources.Add(key, resource);
        return resource;
    }

    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
    {
        if (pooling)
            return GameManager.Pool.Get(original, position, rotation, parent);
        else
            return Object.Instantiate(original, position, rotation, parent);
    }

    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, position, rotation, null, pooling);
    }

    public new T Instantiate<T>(T original, Transform parent, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, parent, pooling);
    }

    public T Instantiate<T>(T original, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, null, pooling);
    }


    // 경로자체path를 줘서 생성하는 Instantiate Resource.Instantiate<ParticleSystem> "Prefabs/HitEffect"
    public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
    {
        T original = Load<T>(path);
        return Instantiate<T>(original, position, rotation, parent, pooling);
    }

    public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
    {
        return Instantiate<T>(path, position, rotation, null, pooling);
    }

    public T Instantiate<T>(string path, Transform parent, bool pooling = false) where T : Object
    {
        return Instantiate<T>(path, Vector3.zero, Quaternion.identity, parent, pooling);
    }

    public T Instantiate<T>(string path, bool pooling = false) where T : Object
    {
        return Instantiate<T>(path, Vector3.zero, Quaternion.identity, null, pooling);
    }

    

    public void Destroy(GameObject go)
    {
        if (GameManager.Pool.IsContain(go))
            GameManager.Pool.Release(go);
        else
            GameObject.Destroy(go);
    }

    public void Destroy(GameObject go, float delay)
    {
        if (GameManager.Pool.IsContain(go))
            StartCoroutine(DelayReleaseRoutine(go, delay));
        else
            GameObject.Destroy(go, delay);
    }

    IEnumerator DelayReleaseRoutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Pool.Release(go);
    }

    public void Destroy(Component component, float delay = 0f)
    {
        Component.Destroy(component, delay);
    }
}
