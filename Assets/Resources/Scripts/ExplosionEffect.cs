using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(Image))]
    public class ExplosionEffect : MonoBehaviour
    {
        [SerializeField] private float m_fadeTime, m_waitingTime;
        private Image m_image;

        private static ExplosionEffect _instance;

        public static ExplosionEffect GetExplosionEffect()
        {
            return _instance;
        }

        void Start()
        {
            m_image = GetComponent<Image>();
            m_image.canvasRenderer.SetAlpha(0);
            _instance = this;
        }

        public void Make()
        {
            StartCoroutine(makeExplosiontEffect());
        }

        public IEnumerator makeExplosiontEffect()
        {
            m_image.canvasRenderer.SetAlpha(1);
            yield return new WaitForSeconds(m_waitingTime);
            m_image.CrossFadeAlpha(0, m_fadeTime, true);
        }
    }
}