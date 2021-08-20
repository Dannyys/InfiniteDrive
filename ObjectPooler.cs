using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;


public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public GameObject[] prefabs;
        public int size;
    }
    
    public static ObjectPooler Instance;
    private void Awake() {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++){
                GameObject obj ;
                if(pool.prefab != null) obj = Instantiate(pool.prefab);
                else obj = Instantiate(pool.prefabs[Random.Range(0, pool.prefabs.Length)]);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
        Instance = this;
    }
    private void OnDestroy() {
        Instance = null;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    // Start is called before the first frame update


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, bool usePrefabY){
        if(!poolDictionary.ContainsKey(tag)){
            Debug.LogWarning("Pool with tag " + tag +" Doesn't exist");
            return null;
        }
        
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        if(usePrefabY) position.y = objToSpawn.transform.localPosition.y;
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }

}
