using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(Image))]
    public class FireButtonSlider : MonoBehaviour
    {
        private Image m_image;
        [SerializeField] private Image m_imageOfFirePowerIndicator;
        private float m_amount;
        public float SliderAmount => m_amount;

        void Start()
        {
            m_image = GetComponent<Image>();
        }

        public void StartChangingOfAmountOfSlider()
        {
            StartCoroutine(ChangeAmountOfSlider());
        }

        private IEnumerator ChangeAmountOfSlider()
        {
            while (m_amount < 1)
            {
                m_amount = Mathf.Clamp(m_amount + Time.deltaTime, 0, 1);
                m_image.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0) / 2;
                m_image.fillAmount = m_amount;
                m_imageOfFirePowerIndicator.fillAmount = m_amount;
                yield return null;
            }
        }
    }
}
