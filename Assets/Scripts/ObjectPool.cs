using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool : MonoBehaviour {
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;
    private void Awake() {
        poolDict = new Dictionary<string, Queue<GameObject>>(); // dictionary containing pools of tiles to be spawned
        foreach (Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++) {
                GameObject go = Instantiate(pool.prefab); // generate objects
                go.SetActive(false);
                objectPool.Enqueue(go);
            }
            poolDict.Add(pool.tag, objectPool);
        }  
    }
    public GameObject SpawnFromPool(string tag,Vector3 position) {
        if (!poolDict.ContainsKey(tag)) return null;
        GameObject spawnedObj = poolDict[tag].Dequeue();
        spawnedObj.SetActive(true);
        spawnedObj.transform.position = position;
        spawnedObj.transform.rotation = Quaternion.identity;
        poolDict[tag].Enqueue(spawnedObj);
        return spawnedObj;
    }
    [System.Serializable]
    public class Pool {
        public string tag; // name of object
        public GameObject prefab;
        public int size; // number of this kind of object that should be in the scene
    }
}