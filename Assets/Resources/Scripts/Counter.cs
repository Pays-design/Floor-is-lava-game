using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class Counter : MonoBehaviour
    {
        [SerializeField] private float m_animationTime;
        [SerializeField] protected string m_counterValueName;
        private TextMeshProUGUI m_textLabel;
        private float m_displayedValue;

        protected virtual void Start()
        {
            m_textLabel = GetComponent<TextMeshProUGUI>();
        }

        protected virtual IEnumerator animateChangingAmountOfMoney(int moneyCount)
        {
            float time = 0;
            float deltaDisplayedValue = m_displayedValue;
            while (time < m_animationTime)
            {
                time += Time.deltaTime;
                deltaDisplayedValue += (moneyCount / m_animationTime * Time.deltaTime);
                m_textLabel.text = m_counterValueName + "" + ((int)deltaDisplayedValue).ToString();
                yield return null;
            }
            m_textLabel.text = m_counterValueName + "" + (m_displayedValue + moneyCount).ToString();
            m_displayedValue += moneyCount;
        }
    }
}
