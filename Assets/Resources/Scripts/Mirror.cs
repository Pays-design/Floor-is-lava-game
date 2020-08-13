using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class Mirror : MonoBehaviour
    {
        [SerializeField] private Transform m_mirrorPlace;
        private readonly List<GameObject> m_balls = new List<GameObject>();
        private int m_indexOfCurrentBall = 0;
        private GameObject m_currentBall;
        private GameObject m_instantiatedBall;
        private static Mirror _instance;
        public GameObject CurrentBall => m_currentBall;

        public List<GameObject> Balls => new List<GameObject>(m_balls);

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            AddBall(UnityEngine.Resources.Load("Prefabs/CannonBallOriginal") as GameObject);
            m_currentBall = m_balls[0];
            m_instantiatedBall = Instantiate(m_balls[m_indexOfCurrentBall], m_mirrorPlace.position, Quaternion.identity);
            m_instantiatedBall.AddComponent<Rigidbody>();
        }

        public static Mirror GetMirror()
        {
            if (_instance == null)
            {
                GameObject instanceObject = new GameObject();
                _instance = instanceObject.AddComponent<Mirror>();
            }
            return _instance;
        }

        public void AddBall(GameObject prefab)
        {
            m_balls.Add(prefab);
        }

        public void SetIndexOfCurrentBall(bool IsNext)
        {
            if (IsNext) m_indexOfCurrentBall = (int)Mathf.Repeat(++m_indexOfCurrentBall, m_balls.Count);
            else m_indexOfCurrentBall = (int)Mathf.Repeat(--m_indexOfCurrentBall, m_balls.Count);
            if (m_currentBall != null) Destroy(m_instantiatedBall);
            m_currentBall = m_balls[m_indexOfCurrentBall];
            m_instantiatedBall = Instantiate(m_balls[m_indexOfCurrentBall], m_mirrorPlace.position, m_currentBall.transform.rotation);
            m_instantiatedBall.AddComponent<Rigidbody>();
        }
    }
}