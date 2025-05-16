using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    
    private Dictionary<string,List<GameObject>> objectsPool = new Dictionary<string,List<GameObject>>(); 
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject GetObject(string path)
    {
        if (!objectsPool.ContainsKey(path)) {
            objectsPool.Add(path, new List<GameObject>());
        }
     
        if (objectsPool[path].Count == 0)
            AddObject(path);

        return AllocateObject(path);
    }

    public void ReleaseObject(string path, GameObject prefab)
    {
        prefab.gameObject.SetActive(false);

        if (!objectsPool.ContainsKey(path)) {
            objectsPool.Add(path, new List<GameObject>());
        }
        
        prefab.transform.SetParent(transform);

        objectsPool[path].Add(prefab);
    }

    private void AddObject(string path)
    {
        GameObject instance = Instantiate(ResourceManager.Instance.GetGameObject(path), transform);
        instance.transform.position = this.transform.position;
        instance.gameObject.SetActive(false);
        objectsPool[path].Add(instance);
    }

    private GameObject AllocateObject(string path)
    {
        GameObject objectPool = objectsPool[path][0];
        objectsPool[path].RemoveAt(0);
        objectPool.gameObject.SetActive(true);
        return objectPool;
    }
}