using System;
using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(CannonBall))]
    public class Slowmotion : MonoBehaviour
    {
        [SerializeField] private float m_maximumSlowmotionCooldown;
        [SerializeField] private float m_maximumSlowmotionTime;
        [SerializeField] private float m_slowmotionTimeScale;

        private CannonBall m_cannonBall;
        private float m_slowmotionTime;
        private Coroutine m_slowmotionCoroutine;
        private float m_slowmotionCooldown;
        private bool m_isSlowmotionStopped;
        private float m_startMaximumSlowmotionTime;
        private float m_startMaximumSlowmotionCooldown;

        public float StartMaximumSlowmotionTime => m_startMaximumSlowmotionTime;
        public float SlowmotionCooldown => m_slowmotionCooldown;
        public float MaximumSlowmotionCooldown => m_maximumSlowmotionCooldown;
        public float SlowmotionTime => m_slowmotionTime;
        public bool CanStartSlowmotion { get { return m_slowmotionCooldown <= 0; } }
        public bool IsSlowmotionStopped => m_isSlowmotionStopped;
        public CannonBall Player => m_cannonBall;
        public float MaximumSlowmotionTime
        {
            get { return m_maximumSlowmotionTime; }
            set
            {
                if (value < StartMaximumSlowmotionTime)
                    throw new ArgumentException("Maximum slowmotion time cannot be lower than start maximum slowmotion time.");
                m_maximumSlowmotionCooldown = m_startMaximumSlowmotionCooldown * (1 + value / m_startMaximumSlowmotionTime);
                m_maximumSlowmotionTime = value;
            }
        }

        #region MonoBehaviour
        private void Awake()
        {
            m_startMaximumSlowmotionTime = m_maximumSlowmotionTime;
            m_startMaximumSlowmotionCooldown = m_maximumSlowmotionCooldown;
            m_cannonBall = GetComponent<CannonBall>();
            m_cannonBall.OnDeath += (death) =>
            {
                StopSlowmotion();
                Destroy(this);
            };
        }

        private void OnValidate() 
        {
            m_maximumSlowmotionTime = m_maximumSlowmotionTime < 0 ? 0 : m_maximumSlowmotionTime;
            m_slowmotionTimeScale = m_slowmotionTimeScale < 0 ? 0 : m_slowmotionTimeScale;
            m_maximumSlowmotionCooldown = m_maximumSlowmotionCooldown < 0 ? 0 : m_maximumSlowmotionCooldown;
        }
        #endregion

        public void StartSlowmotion()
        {
            Time.timeScale = m_slowmotionTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            m_slowmotionCoroutine = StartCoroutine(PerformSlowmotion());
        }

        public void StopSlowmotion()
        {
            m_isSlowmotionStopped = true;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            m_slowmotionCooldown = m_slowmotionTime / m_maximumSlowmotionTime * m_maximumSlowmotionCooldown;
            if (m_slowmotionCoroutine != null)
                StopCoroutine(m_slowmotionCoroutine);
            StartCoroutine(SlowmotionCooldownSetting());
        }

        private IEnumerator PerformSlowmotion()
        {
            m_slowmotionTime = 0;
            while (m_slowmotionTime < m_maximumSlowmotionTime)
            {
                m_slowmotionTime += Time.deltaTime * (1 / Time.timeScale);
                yield return null;
            }
            StopSlowmotion();
        }

        private IEnumerator SlowmotionCooldownSetting()
        {
            while (m_slowmotionCooldown > 0)
            {
                m_slowmotionCooldown -= Time.deltaTime;
                yield return null;
            }
            m_isSlowmotionStopped = false;
        }
    }
}
