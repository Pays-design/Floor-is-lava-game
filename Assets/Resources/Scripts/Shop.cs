using UnityEngine;
using UnityEngine.Events;

namespace Assets.Resources.Scripts
{
    public class Shop : MonoBehaviour
    {
        private Wallet m_playerWallet;
        private Mirror m_mirror;
        [Space(20)]
        public UnityEvent OnFailedToBuy;
        public UnityEvent OnSuccessBuy;

        private static Shop s_instance;
        public static Shop GetShop()
        {
            if (s_instance == null)
            {
                GameObject instanceObject = new GameObject();
                s_instance = instanceObject.AddComponent<Shop>();
            }
            return s_instance;
        }
        private void Awake()
        {
            s_instance = this;
            m_playerWallet = FindObjectOfType<Wallet>();
        }

        public bool TryBuySomething(uint price, GameObject prefab)
        {
            if (m_playerWallet.CanDecreaseMoney(price))
            {
                Mirror.GetMirror().AddBall(prefab);
                OnSuccessBuy?.Invoke();
                m_playerWallet.DecreaseMoney(price);
                return true;
            }
            OnFailedToBuy?.Invoke();
            return false;
        }
    }
}
