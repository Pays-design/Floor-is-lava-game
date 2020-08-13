using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class ExperienceObjectsRotator : ObjectsRotator
    {
        [SerializeField] private Vector3 m_cubeRotationAngles;
        [SerializeField] private float m_sphereRotationAngle;

        protected override IEnumerator rotateObjects()
        {
            while (true)
            {
                for (int i = 0; i < m_generatedObjects.Count; i++)
                {
                    GameObject experienceObject = m_generatedObjects[i];
                    if (experienceObject == null)
                    {
                        m_generatedObjects.Remove(experienceObject);
                        continue;
                    }
                    Transform cubeTransform = experienceObject.transform.GetChild(0);
                    Transform sphereTransform = experienceObject.transform.GetChild(1);
                    cubeTransform.Rotate(m_cubeRotationAngles * Time.deltaTime, Space.World);
                    sphereTransform.RotateAround(cubeTransform.position, Vector3.up, m_sphereRotationAngle * Time.deltaTime);
                }
                yield return null;
            }
        }
    }
}
