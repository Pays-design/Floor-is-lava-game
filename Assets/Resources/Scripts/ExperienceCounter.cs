
namespace Assets.Resources.Scripts
{
    public class ExperienceCounter : Counter
    {
        private CannonBall m_player;
        protected override void Start()
        {
            base.Start();
            m_player = FindObjectOfType<CannonBall>();
            StartCoroutine(animateChangingAmountOfMoney((int)m_player.Experience));
            m_player.OnExperienceAdded += (experienceCount) => StartCoroutine(animateChangingAmountOfMoney((int)experienceCount));
            m_player.OnExperienceDecreased += (experienceCount) => StartCoroutine(animateChangingAmountOfMoney(-(int)experienceCount));
        }
    }
}
