using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Resources.Scripts
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Vector3 m_offset;
        [SerializeField] private Vector3 m_rotation;
        [Header("Transitions")]
        [SerializeField] private Vector3 m_shopPosition;
        [SerializeField] private Vector3 m_rotationAtShop, m_mirrorPosition, m_rotationAtMirror;
        [SerializeField] private float m_toShopTransitionTime, m_toMirrorTransitionTime, m_toCannonTransitionTime;
        private Transform m_target;
        private ParticleSystem m_windParticles;
        private Vector3 m_deltaPosition;
        private Vector3 m_startPosition;
        private Quaternion m_startRotation;
        [Space(20)]
        public UnityEvent OnToShopTransitionEnded;
        public UnityEvent OnToMirrorTransitionEnded;
        public UnityEvent OnToCannonTransitionEnded;

        private void Start()
        {
            m_windParticles = GetComponentInChildren<ParticleSystem>();
            m_startPosition = transform.position;
            m_startRotation = transform.rotation;
            FindObjectOfType<Cannon>().OnShoot += () => SetTarget(FindObjectOfType<CannonBall>().transform);
        }

        public void SetTarget(Transform target)
        {
            m_target = target;
            StartCoroutine(rotate());
        }

        public void LookOnShop()
        {
            StartCoroutine(LookOnTarget(m_shopPosition, m_rotationAtShop, m_toShopTransitionTime, OnToShopTransitionEnded));
        }

        public void LookOnMirror()
        {
            StartCoroutine(LookOnTarget(m_mirrorPosition, m_rotationAtMirror, m_toMirrorTransitionTime, OnToMirrorTransitionEnded));
        }

        public void LookOnCannon()
        {
            StartCoroutine(LookOnTarget(m_startPosition, m_startRotation.eulerAngles, m_toCannonTransitionTime, OnToCannonTransitionEnded));
        }

        public IEnumerator LookOnTarget(Vector3 finalPosition, Vector3 finalRotation, float time, UnityEvent OnEndEvent)
        {
            UIViewSwitcher.GetUIViewSwitcher().ClearCurrentView();
            float t = 0;
            Quaternion startRotation = transform.rotation;
            Vector3 startPosition = transform.position;
            while (t < 1)
            {
                t += Time.deltaTime / time;
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(finalRotation), t);
                transform.position = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
            OnEndEvent?.Invoke();
        }

        private IEnumerator rotate()
        {
            float t = 0;
            Quaternion rotation = Quaternion.Euler(m_rotation);
            while (t < 1)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, t);
                t += Time.deltaTime / 10;
                yield return null;
            }
        }

        private void LateUpdate()
        {
            if (m_target != null)
            {
                m_deltaPosition = transform.position;
                RaycastHit raycastHit;
                Physics.Raycast(m_target.position, m_offset, out raycastHit, m_offset.magnitude);
                if (raycastHit.collider != null)
                    transform.position = raycastHit.point;
                else
                    transform.position = m_target.position + m_offset;
                Vector3 velocity = (transform.position - m_deltaPosition);
                float speed = new Vector2(velocity.x, velocity.y).magnitude / Time.deltaTime;
                if (speed == 0) return;
                if (speed < 20 && m_windParticles.isPlaying) m_windParticles.Stop();
                else
                {
                    if (!m_windParticles.isPlaying) m_windParticles.Play();
                    m_windParticles.startSpeed = speed / 4;
                }
            }
            else if (m_windParticles.isPlaying)
            {
                m_windParticles.Stop();
            }
        }
    }
}
