using System;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    [System.Serializable]
    public class GeneratorPair : IComparable<GeneratorPair>
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public float Chance;
        [SerializeField] public Vector3 MinimumRotation, MaximumRotation;
        [SerializeField] public Vector3 MinimumSize, MaximumSize;
        public Quaternion MinimumRotationInQuaternion => Quaternion.Euler(MinimumRotation);
        public Quaternion MaximumRotationInQuaternion => Quaternion.Euler(MaximumRotation);

        public int CompareTo(GeneratorPair other)
        {
            if (Chance > other.Chance) return 1;
            else if (Chance < other.Chance) return -1;
            return 0;
        }
    }
}
