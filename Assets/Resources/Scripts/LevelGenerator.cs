using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private float m_distanceOfGeneration;
        [SerializeField] private Vector3 m_sizeOfGenerationCube;
        [SerializeField] private Transform m_startPosition;
        [SerializeField] private GeneratorPairList m_spawnObjects = new GeneratorPairList();
        [SerializeField] private int m_maxObjectsInCube;
        [SerializeField] private Vector2Int m_sizeOfGeneration;
        [SerializeField] private GameObject m_targetPrefab;
        [SerializeField] private float m_distanceBetweenLastGenerationCubeAndTarget;
        private List<GameObject> m_generatedObjects = new List<GameObject>();

        public List<GameObject> GeneratedObjects => m_generatedObjects;

        public event System.Action OnGenerate;

        private static LevelGenerator _instance;

        public static LevelGenerator GetLevelGenerator()
        {
            if (_instance != null) return _instance;
            else throw new System.Exception("Level generator must be in scene!");
        }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            m_spawnObjects.Instance.Sort();
            GenerateCubeMatrix(m_sizeOfGeneration.x, m_sizeOfGeneration.y);
        }

        public List<GameObject> GetGeneratedObjectsWithName(string prefabName)
        {
            return m_generatedObjects.FindAll((GameObject g) => g.name.Replace("(Clone)", "") == prefabName);
        }

        private void GenerateCubeMatrix(int width, int height)
        {
            float x = -width / 2 * m_sizeOfGenerationCube.x + m_sizeOfGenerationCube.x / 2;
            float z = m_startPosition.position.z + m_startPosition.position.z / 2;
            float y = height * m_sizeOfGenerationCube.y + m_sizeOfGenerationCube.y / 2;
            while (z < m_distanceOfGeneration)
            {
                for (int i = 0; i < width; i++)
                {
                    GenerateColumn(x,z,height);
                    x += m_sizeOfGenerationCube.x;
                }
                z += m_sizeOfGenerationCube.z;
                x = -width / 2 * m_sizeOfGenerationCube.x + m_sizeOfGenerationCube.x / 2;
            }
            Instantiate(m_targetPrefab, new Vector3(m_startPosition.position.x, m_startPosition.transform.position.y, z + m_distanceBetweenLastGenerationCubeAndTarget), Quaternion.identity);
            OnGenerate?.Invoke();
        }

        private void Generate(float x, float y, float z)
        {
            int numOfObjects = Random.Range(0, m_maxObjectsInCube + 1);
            for (int i = 0; i < numOfObjects; i++)
            {
                float deltaChance = Random.Range(0f, 1f);
                int indexOfObjectFromChance = getIndexOfObjectFromChance(deltaChance);
                if (indexOfObjectFromChance == -1) continue;
                SpawnObject(indexOfObjectFromChance, new Vector3(x, y, z));
            }
        }

        private void GenerateColumn(float x, float z, float height) 
        {
            float y = height * m_sizeOfGenerationCube.y + m_sizeOfGenerationCube.y / 2;
            for (int j = 0; j < height; j++)
            {
                y -= m_sizeOfGenerationCube.y;
                Generate(x, y, z);
            }
        }

        private void SpawnObject(int indexOfObjectFromChance, Vector3 position) 
        {
            GameObject g = Instantiate(m_spawnObjects.Instance[indexOfObjectFromChance].Prefab) as GameObject;
            g.transform.position += new Vector3(position.x, position.y, position.z) + new Vector3(Random.Range(-m_sizeOfGenerationCube.x / 2, m_sizeOfGenerationCube.x / 2), Random.Range(-m_sizeOfGenerationCube.y / 2, m_sizeOfGenerationCube.y / 2), Random.Range(-m_sizeOfGenerationCube.z / 2, m_sizeOfGenerationCube.z / 2));
            float rotationT = Random.Range(0f, 1f);
            float sizeT = Random.Range(0f, 1f);
            g.transform.rotation = Quaternion.Lerp(m_spawnObjects.Instance[indexOfObjectFromChance].MinimumRotationInQuaternion, m_spawnObjects.Instance[indexOfObjectFromChance].MaximumRotationInQuaternion, rotationT);
            g.transform.localScale = Vector3.Lerp(m_spawnObjects.Instance[indexOfObjectFromChance].MinimumSize, m_spawnObjects.Instance[indexOfObjectFromChance].MaximumSize, sizeT);
            Collider collider = m_spawnObjects.Instance[indexOfObjectFromChance].Prefab.GetComponent<Collider>();
            if (Physics.OverlapBox(g.transform.position, collider.bounds.size, g.transform.rotation).Length > 0)
                Destroy(g);
            else m_generatedObjects.Add(g);
        }

        private int getIndexOfObjectFromChance(float chance)
        {
            List<float> chancesIntervals = new List<float>();
            if (m_spawnObjects.Instance.Count < 2)
                if (m_spawnObjects.Instance.Count == 1 && m_spawnObjects.Instance[0].Chance > chance) return 0;
                else return -1;
            chancesIntervals.Add(m_spawnObjects.Instance[0].Chance);
            for (int i = 1; i < m_spawnObjects.Instance.Count; i++)
                chancesIntervals.Add(m_spawnObjects.Instance[i].Chance + m_spawnObjects.Instance[i - 1].Chance);

            for (int i = 0; i < chancesIntervals.Count; i++)
                if (chancesIntervals[i] > chance) return i;
            return -1;
        }
    }
}
