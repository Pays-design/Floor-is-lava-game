using UnityEngine.Advertisements;
using UnityEngine;
using System;

namespace Assets.Resources.Scripts
{
    public class AdsPlayer : MonoBehaviour
    {
        private static AdsPlayer s_instance;

        private const string m_gameID = "3758315";
        private const string m_skippableAdID = "video";
        private const string m_nonSkippableAdID = "rewardedVideo";

        public static AdsPlayer GetAdsPlayer()
        {
            if (s_instance == null)
            {
                GameObject g = new GameObject();
                s_instance = g.AddComponent<AdsPlayer>();
            }
            return s_instance;
        }

        private void Awake() => s_instance = this;

        private void Start()
        {
            Advertisement.Initialize(m_gameID, false, false);
            FindObjectOfType<CannonBall>().OnDeath += (deathType) => TryShowAd(AdType.CanBeSkipped);
        }

        public bool TryShowAd(AdType adType) 
        {
            string adID = adType == AdType.CanBeSkipped ? m_skippableAdID : m_nonSkippableAdID;
            Advertisement.Load(adID);
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
                return true;
            }
            return false;
        }

        public void TryShowAd(string adType) => TryShowAd((AdType)Enum.Parse(typeof(AdType), adType));

        public void TryShowAd(string adType, out bool isSuccesed) 
        {
            if (TryShowAd((AdType)Enum.Parse(typeof(AdType), adType))) 
            {
                isSuccesed = true;
                return;
            }
            isSuccesed = false;
        }
    }

    public enum AdType 
    {
        CanBeSkipped,
        CannotBeSkipped
    }
}