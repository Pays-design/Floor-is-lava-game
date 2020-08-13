namespace Assets.Resources.Scripts
{
    public class MoneyGainVisualizer : GainVisualizer
    {
        protected void Start()
        {
            m_cannonBall.GetComponent<Wallet>().OnMoneyAdded += Visualize;
        }
    }
}
