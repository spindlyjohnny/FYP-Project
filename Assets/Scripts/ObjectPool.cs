using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool : MonoBehaviour {
    int removalIndex = 0;
    public List<GameObject> activeObject = new List<GameObject>();
    //public bool once = false;
    int spawningIndex = 0;
    public List<Pool> L1pools,L2pools,L3pools,MRTPools;
    public Dictionary<string, Queue<GameObject>> poolDict;
    private void Awake() {
        //if (FindObjectOfType<LevelManager>().level == LevelManager.Level.BusInterior) return;
        poolDict = new Dictionary<string, Queue<GameObject>>(); // dictionary containing pools of tiles to be spawned
        List<Pool> pools = new();
        if (GetComponent<LevelManager>()) {
            if (GetComponent<LevelManager>().level != LevelManager.Level.MRT) {
                switch (LevelManager.levelNum) { // pools in this case contain tiles
                    case LevelManager.LevelNum.Level1:
                        pools = L1pools;
                        break;
                    case LevelManager.LevelNum.Level2:
                        pools = L2pools;
                        break;
                    case LevelManager.LevelNum.Level3:
                        pools = L3pools;
                        break;
                }
            } 
            else {
                pools = MRTPools;
            }
        } 
        else {
            switch (LevelManager.levelNum) { // pools in this case contain NPCs and this block runs on Tiles
                case LevelManager.LevelNum.Level1:
                    pools = L1pools;
                    break;
                case LevelManager.LevelNum.Level2:
                    pools = L2pools;
                    break;
                case LevelManager.LevelNum.Level3:
                    pools = L3pools;
                    break;
            }
        }
        foreach (Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++) {
                GameObject go = Instantiate(pool.prefab); // generate objects
                go.SetActive(false);
                objectPool.Enqueue(go);
            }
            poolDict.Add(pool.tag, objectPool);
        }
    }
    public GameObject SpawnFromPool(string tag,Vector3 position) {
        if (!poolDict.ContainsKey(tag)) return null; // tags in pools array have to exactly match names of gameobjects in levelmanager tiles array
        if (spawningIndex == 8)
        {
            spawningIndex += 1;
            return null;
        }
        spawningIndex += 1;
        GameObject spawnedObj = poolDict[tag].Dequeue();
        spawnedObj.SetActive(true);
        spawnedObj.transform.position = position;
        spawnedObj.transform.rotation = Quaternion.identity;
        activeObject.Add(spawnedObj);
        poolDict[tag].Enqueue(spawnedObj);
        return spawnedObj;
    }

    public void Remove()
    {
        if (activeObject.Count >= 10)
        {
            activeObject[removalIndex].SetActive(false);
            activeObject.RemoveAt(0);
        }
    }

    [System.Serializable]
    public class Pool {
        public string tag; // name of object
        public GameObject prefab;
        public int size; // number of this kind of object that should be in the scene
    }
}
