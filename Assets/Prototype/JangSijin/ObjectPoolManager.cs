using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Defense.Manager
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance;
        
        [Serializable]
        public struct PrefabObjectKeyValuePair
        {
            public string name;
            public GameObject prefab;
        }

        public List<PrefabObjectKeyValuePair> managedPrefabs;

        private Dictionary<string, List<GameObject>> _objectPool;

        private void Awake()
        {
            Instance = this;
            _objectPool = new Dictionary<string, List<GameObject>>();
        }

        public GameObject Spawn(string spawnObjectName, Vector3 position = default, Quaternion rotation = default)
        {
            if (!managedPrefabs.Exists(keyValuePair => keyValuePair.name == spawnObjectName))
            {
                print($"{spawnObjectName} 라는 Prefab은 존재하지 않습니다.");
                return null;
            }

            var foundedPrefabData = managedPrefabs.FirstOrDefault(pair => pair.name == spawnObjectName);
            
            // 풀링할 Container 마련하기
            if(!_objectPool.ContainsKey(spawnObjectName))
                _objectPool.Add(spawnObjectName, new List<GameObject>());

            var founded = _objectPool[spawnObjectName].FirstOrDefault(go => !go.activeInHierarchy);
            
            if(founded != null)
                founded.SetActive(true);
            else
            {
                founded = Instantiate(foundedPrefabData.prefab);
                _objectPool[spawnObjectName].Add(founded);
            }

            if (position != default) founded.transform.position = position;
            if (rotation != default) founded.transform.rotation = rotation;

            return founded;
        }
    }
}
