using Assets.Resources.Interfaces;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public abstract class SaverLoader<TData> : MonoBehaviour, ISaverLoader<TData> where TData: class, new()
    {     

        protected virtual void Awake()
        {
            GlobalSaverLoader globalSaverLoader = GlobalSaverLoader.GetInstance();
            globalSaverLoader.OnLoad += () => LoadData(globalSaverLoader.TryGetData<TData>());
            globalSaverLoader.OnSave += () => globalSaverLoader.AddDataToSerialize(GetDataForSave());
        }

        public abstract void LoadData(TData data);
        public abstract TData GetDataForSave();
    }
}
