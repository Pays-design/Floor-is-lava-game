using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Resources.Scripts
{
    public class SetterOfAngleOfCannon : MonoBehaviour, IDragHandler
    {
        [SerializeField]
        private float m_maximum_angle;
        private Vector2 m_handleStartPosition;
        private float m_cannonStartAngle;
        [SerializeField]
        private GameObject m_cannonRotator;
        [SerializeField]
        private GameObject m_handle;
        private GameObject m_currentCanvas;
        private void Start()
        {
            m_currentCanvas = GameObject.Find("Canvas");
            m_handleStartPosition = m_handle.transform.position;
            m_cannonStartAngle = m_cannonRotator.transform.eulerAngles.x;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float y = eventData.position.y - m_handleStartPosition.y;
            if (Mathf.Abs(y) > 300 * m_currentCanvas.transform.localScale.y) return;
            float a = m_maximum_angle / (300 * m_currentCanvas.transform.localScale.y) * y;
            m_handle.transform.position = new Vector3(m_handle.transform.position.x, y + 270 * m_currentCanvas.transform.localScale.y, m_handle.transform.position.z);
            m_cannonRotator.transform.eulerAngles = new Vector3(m_cannonStartAngle + a, 0, 0);
        }
    }
}
