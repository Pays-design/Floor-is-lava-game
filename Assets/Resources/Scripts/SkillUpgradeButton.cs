using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    public abstract class SkillUpgradeButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private uint m_experienceDecreaseAmount;
        [SerializeField] protected uint m_maximumDecreasedExperience;
        [SerializeField] private Image m_imageOfIndicator;

        protected float m_decreasedExperience;
        protected CannonBall m_player;
        private Coroutine m_experienceDecreasingCoroutine;
        private float m_experienceToDecrease;
        
        public uint DecreasedExperience 
        {
            get { return (uint) m_decreasedExperience; }
            set 
            {
                if (value > m_maximumDecreasedExperience)
                    throw new System.Exception("Decreased Experience cannot be larger than maximum decreased experience");
                UpgradeSkill();
                m_decreasedExperience = value;
                RefillSkillUpgradeIndicator();
            }
        }

        protected virtual void Awake() => m_player = FindObjectOfType<CannonBall>();

        private IEnumerator GetExperienceToDecrease()
        {
            float fixedDecreaseAmount = m_experienceDecreaseAmount;
            while (m_player.CanDecreaseExperience((uint) Mathf.CeilToInt(m_experienceToDecrease)) && m_decreasedExperience < m_maximumDecreasedExperience)
            {
                m_experienceToDecrease += fixedDecreaseAmount * Time.deltaTime;
                m_decreasedExperience += fixedDecreaseAmount * Time.deltaTime;
                RefillSkillUpgradeIndicator();
                yield return null;
            }
        }

        protected virtual void UpgradeSkill()
        {
            m_player.DecreaseExperience((uint)m_experienceToDecrease);
            m_experienceToDecrease = 0;
        }

        public void RefillSkillUpgradeIndicator()
        {
            m_imageOfIndicator.fillAmount = m_decreasedExperience / m_maximumDecreasedExperience;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_experienceDecreasingCoroutine != null)
                StopCoroutine(m_experienceDecreasingCoroutine);
            UpgradeSkill();
            if (m_decreasedExperience > m_maximumDecreasedExperience) 
                m_decreasedExperience = m_maximumDecreasedExperience;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if ((int)m_decreasedExperience < (int)m_maximumDecreasedExperience)
                m_experienceDecreasingCoroutine = StartCoroutine(GetExperienceToDecrease());
        }
    }
}
