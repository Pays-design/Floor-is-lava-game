using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(Button))]
    public class JumpButton : MonoBehaviour
    {
        private Button m_button;
        private CannonBall m_cannonBall;
        [SerializeField]
        private float m_vanishTime;
        private void Start()
        {
            m_button = GetComponent<Button>();
            m_cannonBall = FindObjectOfType<CannonBall>();
            m_button.onClick.AddListener(tryForceCannonBallToJump);
        }

        private void tryForceCannonBallToJump()
        {
            m_cannonBall.Jump();
            if (!m_cannonBall.CanJump)
                StartCoroutine(vanishButton());
        }

        private IEnumerator vanishButton()
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * (1 / m_vanishTime);
                transform.localScale = Vector3.Lerp(new Vector3(2, 2, 2), Vector3.zero, t);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
