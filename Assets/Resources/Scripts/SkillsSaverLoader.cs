using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class SkillsSaverLoader : SaverLoader<SkillsDataContainer>
    {
        private JumpSkillUpgradeButton m_jumpSkillUpgradeButton;
        private SlowmotionSkillUpgradeButton m_slowmotionSkillUpgradeButton;

        protected override void Awake()
        {
            base.Awake();
            m_jumpSkillUpgradeButton = FindObjectOfType<JumpSkillUpgradeButton>();
            m_slowmotionSkillUpgradeButton = FindObjectOfType<SlowmotionSkillUpgradeButton>();
        }

        public override void LoadData(SkillsDataContainer data)
        {
            m_slowmotionSkillUpgradeButton.DecreasedExperience = data.SlowmotionSkillDecreasedExperience;
            m_jumpSkillUpgradeButton.DecreasedExperience = data.JumpSkillDecreasedExperience;
        }

        public override SkillsDataContainer GetDataForSave()
        {
            SkillsDataContainer data = new SkillsDataContainer();
            data.JumpSkillDecreasedExperience = m_jumpSkillUpgradeButton.DecreasedExperience;
            data.SlowmotionSkillDecreasedExperience = m_slowmotionSkillUpgradeButton.DecreasedExperience;
            return data;
        }
    }
}
