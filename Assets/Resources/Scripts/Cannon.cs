using System;
using UnityEngine;
namespace Assets.Resources.Scripts
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_cannonballRigidbody;
        [SerializeField] private float m_startImpulsePower;
        [SerializeField] private ParticleSystem m_shootParticleSystem;
        public float ImpulsePower => m_startImpulsePower;
        public Rigidbody CannonBallRigidBody => m_cannonballRigidbody;

        public void Start()
        {
            m_cannonballRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            m_cannonballRigidbody.GetComponent<CannonBall>().enabled = false;
        }

        public void Shoot()
        {
            m_cannonballRigidbody.constraints = RigidbodyConstraints.None;
            float sliderAmount = FindObjectOfType<FireButtonSlider>().SliderAmount;
            m_cannonballRigidbody.AddForce(m_cannonballRigidbody.transform.forward * m_startImpulsePower * (1 + sliderAmount));
            m_cannonballRigidbody.AddTorque(m_cannonballRigidbody.transform.right * m_startImpulsePower * (1 + sliderAmount));
            m_cannonballRigidbody.transform.parent = null;
            m_cannonballRigidbody.GetComponent<CannonBall>().SetAppearance(Mirror.GetMirror().CurrentBall);
            m_shootParticleSystem.Play();
            OnShoot();
            m_cannonballRigidbody.GetComponent<CannonBall>().enabled = true;
        }

        public event Action OnShoot;
    }
}
