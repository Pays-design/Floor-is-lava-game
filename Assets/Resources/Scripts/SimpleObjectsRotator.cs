using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class SimpleObjectsRotator : ObjectsRotator
    {
        [SerializeField] private Vector3 m_rotationAngles;
        protected override IEnumerator rotateObjects()
        {
            while (true)
            {
                for (int i = 0; i < m_generatedObjects.Count; i++)
                {
                    GameObject generatedObject = m_generatedObjects[i];
                    if (generatedObject == null)
                    {
                        m_generatedObjects.Remove(generatedObject);
                        continue;
                    }
                    generatedObject.transform.Rotate(m_rotationAngles * Time.deltaTime);
                }
                yield return null;
            }
        }
    }
}
