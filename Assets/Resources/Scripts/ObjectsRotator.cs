using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public abstract class ObjectsRotator : MonoBehaviour
    {
        [SerializeField] protected string m_objectName;
        protected List<GameObject> m_generatedObjects;
        protected Coroutine m_rotationCoroutine;
        protected CannonBall m_cannonBall;
        protected virtual void Start()
        {
            m_cannonBall = FindObjectOfType<CannonBall>();
            m_cannonBall.OnAcceleration += (accelerator) => { StopCoroutine(m_rotationCoroutine); };
            m_cannonBall.OnAccelerationStopped += () => { m_rotationCoroutine = StartCoroutine(rotateObjects()); };
            LevelGenerator.GetLevelGenerator().OnGenerate += () =>
            {
                m_generatedObjects = LevelGenerator.GetLevelGenerator().GetGeneratedObjectsWithName(m_objectName);
                if (m_generatedObjects.Count == 0) Destroy(this);
                m_rotationCoroutine = StartCoroutine(rotateObjects());
            };
        }

        protected abstract IEnumerator rotateObjects();
    }
}
