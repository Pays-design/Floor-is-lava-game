using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class LevelSetupper : MonoBehaviour
    {
        [SerializeField] private uint m_startGainExperience, m_startGainMoney;
        [SerializeField] private float m_gainMoneyMultiplicator, m_gainExperienceMultiplicator;
        [SerializeField] private uint m_maximumLevelOfMultiplication;
        private uint m_currentLevel = 1;

        public uint CurrentLevel
        {
            get { return m_currentLevel; }
            set
            {
                if (value == 0)
                    throw new System.Exception("Level cannon be lower than 1");
                m_currentLevel = value;
            }
        }

        public void ToNextLevel() 
        {
            if(m_currentLevel < m_maximumLevelOfMultiplication)
            m_currentLevel++;
        }

        public uint GainExperience => (uint)(m_startGainExperience * m_currentLevel * m_gainExperienceMultiplicator);
        public uint GainMoney => (uint) (m_startGainMoney * m_currentLevel * m_gainMoneyMultiplicator);

        private static LevelSetupper _instance;
        public static LevelSetupper GetLevelSetupper()
        {
            if (_instance != null) return _instance;
            else throw new System.Exception("LevelSetupper must be in scene.");
        }

        private void Awake()
        {
            _instance = this;
        }
    }
}
