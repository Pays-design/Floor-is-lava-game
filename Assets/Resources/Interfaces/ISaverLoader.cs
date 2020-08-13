
namespace Assets.Resources.Interfaces
{
    public interface ISaverLoader<TData>
    {
        void LoadData(TData data);
        TData GetDataForSave();
    }
}
