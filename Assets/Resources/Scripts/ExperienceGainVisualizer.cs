namespace Assets.Resources.Scripts
{
    public class ExperienceGainVisualizer : GainVisualizer
    {
        protected void Start()
        {
            m_cannonBall.OnExperienceAdded += Visualize;
        }
    }
}
