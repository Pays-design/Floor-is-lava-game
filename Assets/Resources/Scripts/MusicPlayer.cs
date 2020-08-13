using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(CannonBall))]
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> m_NonShopMusicClips = new List<AudioClip>();
        [SerializeField] private List<AudioClip> m_shopMusicClips = new List<AudioClip>();
        [SerializeField] private AudioClip m_cannonShootSound, m_deathFromLavaSound, m_deathFromBombSound, m_winSound, m_moneyGainSound, m_experienceGainSound, m_failedToBuySound, m_successBuySound;
        private AudioSource m_audioSource;
        private CannonBall m_cannonBall;
        private Coroutine m_playMusicCoroutine;

        private void Start()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_cannonBall = GetComponent<CannonBall>();
            m_cannonBall.OnDeath += (death) => PlayDeathSound(death);
            m_cannonBall.OnWin += () => PlayWinSound();
            FindObjectOfType<Cannon>().OnShoot += () => PlayCannonShootSound();
            m_cannonBall.OnExperienceAdded += (exp) => { PlayExperienceGainSound(); };
            m_cannonBall.Wallet.OnMoneyAdded += (mon) => { PlayMoneyGainSound(); };
            m_playMusicCoroutine = StartCoroutine(PlayMusic(m_shopMusicClips));
        }

        public void PlaySuccessBuySound()
        {
            m_audioSource.PlayOneShot(m_successBuySound);
        }

        public void PlayDeathSound(DeathType deathType)
        {
            m_audioSource.Stop();
            StopCoroutine(m_playMusicCoroutine);
            switch (deathType)
            {
                case DeathType.DeathFromBomb:
                    m_audioSource.PlayOneShot(m_deathFromBombSound);
                    break;
                case DeathType.DeathFromLava:
                    m_audioSource.PlayOneShot(m_deathFromLavaSound);
                    break;
            }
        }

        public void PlayFailedToBuySound()
        {
            m_audioSource.PlayOneShot(m_failedToBuySound);
        }

        public void PlayWinSound()
        {
            m_audioSource.Stop();
            StopCoroutine(m_playMusicCoroutine);
            m_audioSource.PlayOneShot(m_winSound);
        }

        public void PlayCannonShootSound()
        {
            m_audioSource.PlayOneShot(m_cannonShootSound);
            if (m_playMusicCoroutine != null)
                StopCoroutine(m_playMusicCoroutine);
            m_playMusicCoroutine = StartCoroutine(PlayMusic(m_NonShopMusicClips));
        }

        public void PlayExperienceGainSound()
        {
            m_audioSource.PlayOneShot(m_experienceGainSound);
        }

        public void PlayMoneyGainSound()
        {
            m_audioSource.PlayOneShot(m_moneyGainSound);
        }

        private IEnumerator PlayMusic(List<AudioClip> clips)
        {
            int indexOfCurrentSong = Random.Range(0, clips.Count);
            m_audioSource.clip = clips[indexOfCurrentSong];
            m_audioSource.Play();
            while (true)
            {
                if (!m_audioSource.isPlaying)
                {
                    indexOfCurrentSong = (int)Mathf.Repeat(++indexOfCurrentSong, clips.Count - 1);
                    m_audioSource.clip = clips[indexOfCurrentSong];
                    m_audioSource.Play();
                }
                yield return null;
            }
        }
    }
}
