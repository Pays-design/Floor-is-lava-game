using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Assets.Resources.Scripts
{
    public class UIViewSwitcher : MonoBehaviour
    {
        private static UIViewSwitcher s_instance;

        [SerializeField] private List<View> m_views = new List<View>();

        private View m_currentView;
        private CannonBall m_player;

        public static UIViewSwitcher GetUIViewSwitcher()
        {
            return s_instance;
        }

        private void Awake()
        {
            s_instance = this;
            m_player = FindObjectOfType<CannonBall>();
        }

        private void Start()
        {
            foreach (var v in m_views)
                ClearView(v.Type);
            ShowView(ViewType.StartView);
            FindObjectOfType<Cannon>().OnShoot += () => ShowView(ViewType.CannonballControlView);
            m_player.OnDeath += (death) =>
            {
                ShowView(ViewType.DeathView);
                DestroyView(ViewType.CannonballControlView);
            };
            m_player.OnWin += () => ShowView(ViewType.WinView);
            m_player.OnAccelerationStopped += StopAcceleration;
            m_player.OnAcceleration += (acclerator) => StartAcceleration(acclerator);
        }

        private void OnValidate()
        {
            if (m_views.Count > Enum.GetNames(typeof(ViewType)).Length)
                m_views.RemoveRange(Enum.GetNames(typeof(ViewType)).Length - 1, m_views.Count - Enum.GetNames(typeof(ViewType)).Length);
            for (int i = 0; i < Enum.GetNames(typeof(ViewType)).Length; i++)
            {
                if (i >= m_views.Count)
                {
                    View fixedView = new View();
                    fixedView.Type = (ViewType)Enum.GetValues(typeof(ViewType)).GetValue(i);
                    m_views.Add(fixedView);
                }
                else if (m_views[i].Type != (ViewType)Enum.GetValues(typeof(ViewType)).GetValue(i))
                    m_views[i].Type = (ViewType)Enum.GetValues(typeof(ViewType)).GetValue(i);
            }
        }

        public void StartAcceleration(GameObject accelerator)
        {
            AcceleratorIndicatorFiller acceleratorIndicatorFiller = FindObjectOfType<AcceleratorIndicatorFiller>();
            acceleratorIndicatorFiller.SetCurrentAccelerator(accelerator);
            acceleratorIndicatorFiller.FillAmount = 0;
            ChangeColorAdjustmentsInOneFrame(true);
            ShowView(ViewType.AccleratorView);
        }

        public void ChangeColorAdjustmentsInOneFrame(bool isInverting)
        {
            ColorAdjustments ca = ((ColorAdjustments)Camera.main.GetComponent<Volume>().profile.components[0]);
            if (isInverting)
                ca.saturation.value = -100;
            else ca.saturation.value = 0;
        }

        public void StopAcceleration()
        {
            ChangeColorAdjustmentsInOneFrame(false);
            ShowView(ViewType.CannonballControlView);
        }

        public void ShowView(ViewType viewType)
        {
            ClearCurrentView();
            m_currentView = FindViewByType(viewType);
            foreach (var obj in FindViewByType(viewType).Objects)
            {
                if (obj != null)
                    foreach (var behaviour in obj.GetComponents<Behaviour>())
                        behaviour.enabled = true;
            }
        }

        public void ClearView(ViewType viewType)
        {
            foreach (var obj in FindViewByType(viewType).Objects)
            {
                if (obj != null)
                    foreach (var behaviour in obj.GetComponents<Behaviour>())
                        behaviour.enabled = false;
            }
        }

        public void DestroyView(ViewType viewType)
        {
            foreach (var obj in FindViewByType(viewType).Objects)
                if (obj != null)
                    GameObject.Destroy(obj);
        }

        public void ShowView(string viewType) => ShowView(FindViewByType(viewType).Type);

        public void DestroyView(string viewType) => DestroyView(FindViewByType(viewType).Type);

        public void ClearView(string viewType) => ClearView(FindViewByType(viewType).Type);

        public void ClearCurrentView()
        {
            if (m_currentView != null)
                ClearView(m_currentView.Type);
        }

        private View FindViewByType(ViewType viewType)
        {
            return m_views[(int)viewType];
        }

        private View FindViewByType(string viewType)
        {
            return m_views[(int)((ViewType)Enum.Parse(typeof(ViewType), viewType))];
        }
    }

    public enum ViewType
    {
        AccleratorView,
        CannonballControlView,
        CannonControlView,
        DeathView,
        WinView,
        StartView,
        ShopView,
        MirrorView,
        SkillsUpgradeView
    }
}