using UnityEngine;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    public class AcceleratorIndicatorFiller : SmoothIndicatorFiller
    {
        [SerializeField] private uint m_clicksToFill;
        private GameObject m_currentAccelerator;
        private Button m_accelerationHitbox;

        public void SetCurrentAccelerator(GameObject accelerator) => m_currentAccelerator = accelerator;

        protected override void Start()
        {
            base.Start();
            m_accelerationHitbox = GetComponentInChildren<Button>();
            m_accelerationHitbox.onClick.AddListener(() => AddAmount(1 / m_clicksToFill));
        }

        public override void AddAmount(float amountToAdd)
        {
            base.AddAmount(amountToAdd);
            int numOfIndicator = (int)((m_image.fillAmount + amountToAdd) * 10);
            if (numOfIndicator % 3 == 0 && numOfIndicator != 0)
                m_currentAccelerator.transform.GetChild(0).GetChild(numOfIndicator / 3 - 1).GetComponent<Renderer>().material = UnityEngine.Resources.Load("Materials/Accelerator_enabled_indicator") as Material;
        }
    }
}
