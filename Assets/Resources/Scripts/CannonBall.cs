using System;
using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Wallet))]
    [RequireComponent(typeof(MusicPlayer))]
    public class CannonBall : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float m_maximumAcceleration;
        [SerializeField] private Color m_deathFromLavaColor;
        [SerializeField] private float m_startJumpForce;
        [SerializeField] private uint m_startMaximumJumpCount;
        [SerializeField] private uint m_maximumExperience;

        private AcceleratorIndicatorFiller m_acceleratorIndicatorFiller;
        private uint m_currentMaximumJumpCount;
        private float m_currentJumpForce;
        private MusicPlayer m_player;
        private uint m_experience = 10000;
        private Rigidbody m_rigidbody;
        private uint m_jumpCount;
        private Action m_state;
        private Wallet m_wallet;
        private Transform m_transform;
        private Gyroscope m_gyroscope;

        public float StartJumpForce => m_startJumpForce;
        public uint StartMaximumJumpCount => m_startMaximumJumpCount;
        public uint JumpCount => m_jumpCount;
        public float MaximumAcceleration => m_maximumAcceleration;
        public bool CanJump { get { return !(m_jumpCount == m_currentMaximumJumpCount); } }
        public uint MaximumExperience => m_maximumExperience;
        public bool CanAddExperience { get { return !(m_experience == m_maximumExperience); } }
        public Wallet Wallet => m_wallet;

        public uint MaximumJumpCount
        {
            get { return m_currentMaximumJumpCount; }
            set
            {
                if (m_startMaximumJumpCount > value)
                    throw new ArgumentException("Maximum jump count cannot be lower than start maximum jump count.");
                m_currentMaximumJumpCount = value;
            }
        }

        public uint Experience 
        {
            get { return m_experience; }
            set 
            {
                if (m_experience > m_maximumExperience)
                    throw new Exception("Experience count cannot be larger than maximum experience count");
                m_experience = value;
            }
        }

        public float JumpForce
        {
            get { return m_currentJumpForce; }
            set
            {
                if (m_startJumpForce > value)
                    throw new ArgumentException("Jump force cannot be lower than start jump force.");
                m_currentJumpForce = value;
            }
        }


        public event Action<DeathType> OnDeath;
        public event Action OnWin;
        public event Action<uint> OnExperienceAdded;
        public event Action<uint> OnExperienceDecreased;
        public event Action<GameObject> OnAcceleration;
        public event Action OnAccelerationStopped;

        #region MonoBehaviour
        private void Awake()
        {
            m_acceleratorIndicatorFiller = FindObjectOfType<AcceleratorIndicatorFiller>();
            Input.gyro.enabled = true;
            m_rigidbody = GetComponent<Rigidbody>();
            m_transform = GetComponent<Transform>();
            m_wallet = GetComponent<Wallet>();
            m_state = Move;
            m_player = GetComponent<MusicPlayer>();
            m_gyroscope = Input.gyro;
        }

        private void Update() => m_state?.Invoke();

        private void OnValidate()
        {
            m_startJumpForce = m_startJumpForce < 0 ? 0 : m_startJumpForce;
            m_speed = m_speed < 0 ? 0 : m_speed;
            m_maximumAcceleration = m_maximumAcceleration < 0 ? 0 : m_maximumAcceleration;
        }

        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Accelerator":
                    StartCoroutine(Accelerate(collision.gameObject.transform.forward, collision.gameObject));
                    break;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Experience":
                    Destroy(other.gameObject);
                    AddExperience((uint)(LevelSetupper.GetLevelSetupper().GainExperience * other.gameObject.transform.localScale.x));
                    break;
                case "CenterOfTarget":
                    Win(10);
                    break;
                case "SecondCenterOfTarget":
                    Win(6);
                    break;
                case "ThirdCenterOfTarget":
                    Win(4);
                    break;
                case "Bomb":
                    Destroy(other.gameObject);
                    DeathFromBomb();
                    break;
                case "Coin":
                    m_wallet.AddMoney(LevelSetupper.GetLevelSetupper().GainMoney * (uint)other.transform.localScale.x);
                    break;
                case "GigaCoin":
                    m_wallet.AddMoney(LevelSetupper.GetLevelSetupper().GainMoney * (uint)other.transform.localScale.x * 2);
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag.Contains("Target")) Destroy(gameObject);
        }
        #endregion

        public void AddExperience(uint value)
        {
            if (m_experience + value > m_maximumExperience)
            {
                OnExperienceAdded?.Invoke(m_maximumExperience - m_experience);
                m_experience = m_maximumExperience;
            }
            else
            {
                m_experience += value;
                OnExperienceAdded?.Invoke(value);
            }
        }

        public void DecreaseExperience(uint value)
        {
            if (m_experience - value < 0) throw new System.Exception("Experience cant be lower than zero");
            else
            {
                m_experience -= value;
                OnExperienceDecreased?.Invoke(value);
            }
        }

        public bool CanDecreaseExperience(uint value)
        {
            return (int)m_experience - (int)value >= 0;
        }

        private void Move()
        {
            Vector3 velocity = new Vector3(-m_gyroscope.attitude.z * 0.1f, Mathf.Clamp(-m_gyroscope.attitude.x, -1, 0) * 0.1f);
            m_transform.position += velocity * m_speed;
            m_rigidbody.AddTorque(Vector3.Cross(velocity, Vector3.up) * m_currentJumpForce * 2);
            if (transform.position.y < -1.5f) StartCoroutine(DeathFromLava());
        }

        public void Jump()
        {
            if (CanJump)
            {
                m_rigidbody.AddForce(Vector3.up * m_currentJumpForce + Vector3.up * (m_rigidbody.velocity.y > 0 ? 0 : -m_rigidbody.velocity.y * 100));
                m_rigidbody.AddTorque(transform.right * m_currentJumpForce * 2);
                m_jumpCount++;
            }
        }

        public void SetAppearance(GameObject cannonBall) 
        {
            GetComponent<MeshFilter>().mesh = cannonBall.GetComponent<MeshFilter>().sharedMesh;
            GetComponent<MeshCollider>().sharedMesh = cannonBall.GetComponent<MeshFilter>().sharedMesh;
            GetComponent<Renderer>().materials = cannonBall.GetComponent<Renderer>().sharedMaterials;
            m_transform.localScale = cannonBall.transform.localScale;
        }

        private void Win(uint multiplayerValue)
        {
            AddExperience(LevelSetupper.GetLevelSetupper().GainExperience * multiplayerValue);
            OnWin?.Invoke();
        }

        private IEnumerator Accelerate(Vector3 direction, GameObject accelerator)
        {
            Vector3 deltaVelocity = m_rigidbody.velocity;
            OnAcceleration?.Invoke(accelerator);
            m_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            m_state = null;
            yield return new WaitForSeconds(4);
            OnAccelerationStopped?.Invoke();
            m_rigidbody.constraints = RigidbodyConstraints.None;
            m_state = Move;
            m_rigidbody.velocity = deltaVelocity;
            m_rigidbody.AddForce(direction * m_acceleratorIndicatorFiller.FillAmount * m_maximumAcceleration);
        }

        private IEnumerator DeathFromLava()
        {
            m_state = null;
            OnDeath?.Invoke(DeathType.DeathFromLava);
            Camera.main.GetComponent<CameraManager>().SetTarget(null);
            m_rigidbody.isKinematic = true;
            Material currentMaterial = GetComponent<Renderer>().material;
            float t = 0;
            while (t < 1)
            {
                transform.position -= new Vector3(0, Time.deltaTime / 1.6f, 0);
                currentMaterial.color = Color.Lerp(currentMaterial.color, m_deathFromLavaColor, t / 10);
                t += Time.deltaTime / 2;
                yield return null;
            }
        }

        private void DeathFromBomb()
        {
            ExplosionEffect.GetExplosionEffect().Make();
            OnDeath?.Invoke(DeathType.DeathFromBomb);
            Destroy(gameObject);
        }

    }

    public enum DeathType
    {
        DeathFromLava,
        DeathFromBomb
    }
}