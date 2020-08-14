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
            m_cannonBall.OnExperienceAdded += (exp) => m_audioSource.PlayOneShot(m_experienceGainSound);
            m_cannonBall.Wallet.OnMoneyAdded += (mon) => m_audioSource.PlayOneShot(m_moneyGainSound);
            m_playMusicCoroutine = StartCoroutine(PlayMusic(m_shopMusicClips));
        }

        public void PlaySuccessBuySound() => m_audioSource.PlayOneShot(m_successBuySound);

        public void PlayDeathSound(DeathType deathType)
        {
            StopPlayingMusic();
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

        public void PlayWinSound()
        {
            StopPlayingMusic();
            m_audioSource.PlayOneShot(m_winSound);
        }

        public void PlayCannonShootSound()
        {
            m_audioSource.PlayOneShot(m_cannonShootSound);
            StopPlayingMusic(); 
            m_playMusicCoroutine = StartCoroutine(PlayMusic(m_NonShopMusicClips));
        }

        public void PlayMoneyGainSound() => m_audioSource.PlayOneShot(m_moneyGainSound);

        public void StopPlayingMusic() 
        {
            m_audioSource.Stop();
            if(m_playMusicCoroutine != null)
            StopCoroutine(m_playMusicCoroutine);
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
