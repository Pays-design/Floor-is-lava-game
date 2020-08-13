namespace Assets.Resources.Scripts
{
    public class SlowmotionSkillUpgradeButton : SkillUpgradeButton
    {
        private Slowmotion m_slowmotion;

        protected override void Awake()
        {
            base.Awake();
            m_slowmotion = m_player.GetComponent<Slowmotion>();
        }

        protected override void UpgradeSkill()
        {
            base.UpgradeSkill();
            m_slowmotion.MaximumSlowmotionTime = (m_decreasedExperience / m_maximumDecreasedExperience + 1) * m_slowmotion.StartMaximumSlowmotionTime;
        }
    }
}
