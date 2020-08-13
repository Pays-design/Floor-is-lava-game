using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    [System.Serializable]
    public class View
    {
        [SerializeField] private List<GameObject> m_objects;
        [SerializeField] private ViewType m_type;
        public List<GameObject> Objects => m_objects;
        public ViewType Type { get { return m_type; } set { m_type = value; } }

    }
}
