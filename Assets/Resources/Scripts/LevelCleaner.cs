using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class LevelCleaner : MonoBehaviour
    {
        private GameObject m_player;
        private static LevelCleaner _instance;
        public static LevelCleaner GetInstance()
        {
            if (_instance == null)
            {
                GameObject g = new GameObject();
                _instance = g.AddComponent<LevelCleaner>();
            }
            return _instance;
        }

        private void Start()
        {
            _instance = this;
            LevelGenerator.GetLevelGenerator().OnGenerate += () => StartCleaning(LevelGenerator.GetLevelGenerator().GeneratedObjects);
        }
        public void StartCleaning(System.Collections.Generic.List<GameObject> objectsToClean)
        {
            m_player = FindObjectOfType<CannonBall>().gameObject;
            StartCoroutine(Clean(new System.Collections.Generic.List<GameObject>(objectsToClean)));
        }

        private IEnumerator Clean(System.Collections.Generic.List<GameObject> objects)
        {
            while (true)
            {
                for(int i = 0; i < objects.Count; i++)
                {     
                    if (objects[i].transform.position.z + 10 < m_player.transform.position.z)
                    {
                        Destroy(objects[i]);
                        objects.RemoveAt(i);
                    }
                }
                yield return null;
            }
        }
    }
}
