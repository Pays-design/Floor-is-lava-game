using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class BuyButton : MonoBehaviour
    {
        [SerializeField] private GameObject m_targetPrefab;
        [SerializeField] private uint m_price;

        public GameObject TargetPrefab => m_targetPrefab;

        public void OnMouseDown()
        {
            if (Shop.GetShop().TryBuySomething(m_price, m_targetPrefab))
            {
                GameObject soldTextPrefab = UnityEngine.Resources.Load("Prefabs/SoldText") as GameObject;
                Instantiate(soldTextPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            };
        }
    }
}