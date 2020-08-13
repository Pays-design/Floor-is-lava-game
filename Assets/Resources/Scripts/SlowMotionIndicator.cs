using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    public class SlowMotionIndicator : MonoBehaviour
    {
        [SerializeField] private IndicatorButton m_startSlowmotionButton;
        [SerializeField] private Image m_slowmotionCooldownIndicatorImage;
        [SerializeField] private Image m_slowmotionTimeIndicatorImage;

        private Slowmotion m_slowmotion;

        private void Start()
        {
            m_slowmotion = FindObjectOfType<Slowmotion>();
            m_startSlowmotionButton.OnClick.AddListener(TryToStartSlowmotion);
            m_startSlowmotionButton.OnClickUp.AddListener(StopVisualizingSlowmotionTime);
            m_slowmotion.Player.OnDeath += (death) =>
            {
                UIViewSwitcher.GetUIViewSwitcher().ChangeColorAdjustmentsInOneFrame(false);
                Destroy(m_slowmotionCooldownIndicatorImage);
            };
        }

        private void TryToStartSlowmotion()
        {
            if (m_slowmotion.CanStartSlowmotion)
            {
                UIViewSwitcher.GetUIViewSwitcher().ChangeColorAdjustmentsInOneFrame(true);
                m_slowmotion.StartSlowmotion();
                StartCoroutine(VisualizeSlowmotionTime());
            }
        }

        private void StopVisualizingSlowmotionTime()
        {
            if (!m_slowmotion.IsSlowmotionStopped)
                m_slowmotion.StopSlowmotion();
            UIViewSwitcher.GetUIViewSwitcher().ChangeColorAdjustmentsInOneFrame(false);
            m_startSlowmotionButton.enabled = false;
            m_slowmotionTimeIndicatorImage.fillAmount = 0;
            m_startSlowmotionButton.GetComponent<Button>().enabled = false;
            StartCoroutine(VisualizeCooldown());
        }

        private IEnumerator VisualizeSlowmotionTime()
        {
            while (!m_slowmotion.IsSlowmotionStopped)
            {
                m_slowmotionTimeIndicatorImage.fillAmount = m_slowmotion.SlowmotionTime / m_slowmotion.MaximumSlowmotionTime;
                yield return null;
            }
            StopVisualizingSlowmotionTime();
        }

        private IEnumerator VisualizeCooldown()
        {
            m_slowmotionCooldownIndicatorImage.enabled = true;
            while (m_slowmotion.SlowmotionCooldown > 0)
            {
                m_slowmotionCooldownIndicatorImage.fillAmount = m_slowmotion.SlowmotionCooldown / m_slowmotion.MaximumSlowmotionCooldown;
                yield return null;
            }
            m_slowmotionCooldownIndicatorImage.enabled = false;
            m_startSlowmotionButton.enabled = true;
            m_startSlowmotionButton.GetComponent<Button>().enabled = true;
        }
    }
}