using TMPro;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class GainVisualizer : MonoBehaviour
    {
        [SerializeField] protected GameObject m_textPrefab;
        [SerializeField] protected float m_scatterRadius;
        [SerializeField] protected float m_upForce;
        protected CannonBall m_cannonBall;
        protected virtual void Awake()
        {
            m_cannonBall = FindObjectOfType<CannonBall>();
        }

        protected virtual void Visualize(uint value)
        {
            Vector3 textPrefabStartPosition = Camera.main.WorldToScreenPoint(m_cannonBall.transform.position);
            Vector3 directionOfImpulse = new Vector3(Random.Range(-m_scatterRadius, m_scatterRadius), m_upForce);
            GameObject textObject = Instantiate(m_textPrefab, textPrefabStartPosition, Quaternion.identity, transform);
            textObject.GetComponent<TextMeshProUGUI>().text = "+" + value;
            textObject.GetComponent<Rigidbody2D>().AddForce(directionOfImpulse);
        }
    }
}
