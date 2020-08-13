using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    [System.Serializable]
    public class GeneratorPairList
    {
        [SerializeField]
        public List<GeneratorPair> Instance = new List<GeneratorPair>();
    }
}
