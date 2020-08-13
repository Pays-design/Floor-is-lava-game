using System;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private uint m_maximumMoneyCount;
        private uint m_moneyCount = 10000;
        public uint MaximumMoneyCount => m_maximumMoneyCount;
        public bool CanAddMoney { get { return !(m_maximumMoneyCount == m_moneyCount); } }
        public uint MoneyCount 
        {
            get { return m_moneyCount;  }
            set 
            {
                if (m_moneyCount > m_maximumMoneyCount)
                    throw new Exception("Money count cannot be larger than maximum money count.");
                m_moneyCount = value;
            }
        }

        public event Action<uint> OnMoneyAdded;
        public event Action<uint> OnMoneyDecreased;

        public void AddMoney(uint value)
        {
            if (m_moneyCount + value > m_maximumMoneyCount)
            {
                m_moneyCount = m_maximumMoneyCount;
                OnMoneyAdded?.Invoke(m_maximumMoneyCount - m_moneyCount);
            }
            else
            {
                m_moneyCount += value;
                OnMoneyAdded?.Invoke(value);
            }
        }

        public void DecreaseMoney(uint value)
        {
            if ((int) m_moneyCount - (int)value < 0) throw new System.Exception("Money cant be lower than zero!");
            else
            {
                m_moneyCount -= value;
                OnMoneyDecreased?.Invoke(value);
            }
        }

        public bool CanDecreaseMoney(uint value)
        {
            return (int)m_moneyCount - (int)value >= 0;
        }
    }
}
