namespace Assets.Resources.Scripts
{
    public class JumpSkillUpgradeButton : SkillUpgradeButton
    {
        protected override void UpgradeSkill()
        {
            base.UpgradeSkill();
            m_player.MaximumJumpCount = (uint)((m_decreasedExperience / m_maximumDecreasedExperience + 1) * m_player.StartMaximumJumpCount);
            m_player.JumpForce = (m_decreasedExperience / m_maximumDecreasedExperience + 1) * m_player.StartJumpForce;
        }
    }
}
