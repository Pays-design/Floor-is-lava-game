using UnityEngine;

namespace Assets.Resources.Scripts 
{
    [RequireComponent(typeof(CannonBall))]
    [RequireComponent(typeof(Wallet))]
    public class CannonballDataSaverLoader : SaverLoader<CannonballDataContainer>
    {
        private CannonBall m_player;
        private Wallet m_playerWallet;

        protected override void Awake()
        {
            base.Awake();
            m_player = GetComponent<CannonBall>();
            m_playerWallet = GetComponent<Wallet>();
        }

        public override void LoadData(CannonballDataContainer data)
        {
            m_player.Experience = data.Experience;
            m_playerWallet.MoneyCount = data.Money;
            LevelSetupper.GetLevelSetupper().CurrentLevel = data.CurrentLevelOfScene;
        }

        public override CannonballDataContainer GetDataForSave()
        {
            CannonballDataContainer data = new CannonballDataContainer();
            data.Experience = m_player.Experience;
            data.Money = m_playerWallet.MoneyCount;
            data.CurrentLevelOfScene = LevelSetupper.GetLevelSetupper().CurrentLevel;
            return data;
        }
    }
}
