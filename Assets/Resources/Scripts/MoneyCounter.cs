namespace Assets.Resources.Scripts
{
    public class MoneyCounter : Counter
    {
        private Wallet m_playerWallet;
        protected override void Start()
        {
            base.Start();
            m_playerWallet = FindObjectOfType<Wallet>();
            StartCoroutine(animateChangingAmountOfMoney((int)m_playerWallet.MoneyCount));
            m_playerWallet.OnMoneyAdded += (moneyCount) => StartCoroutine(animateChangingAmountOfMoney((int)moneyCount));
            m_playerWallet.OnMoneyDecreased += (moneyCount) => StartCoroutine(animateChangingAmountOfMoney(-(int)moneyCount));
        }
    }
}