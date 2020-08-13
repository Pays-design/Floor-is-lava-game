using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Resources.Scripts
{
    public class ShopDataSaverLoader : SaverLoader<List<string>>
    {
        public override List<string> GetDataForSave()
        {
            List<GameObject> balls = Mirror.GetMirror().Balls;
            List<string> data = (from ball in balls select ball.name).ToList();
            return data;
        }

        public override void LoadData(List<string> data)
        {
            BuyButton[] shopButtons = FindObjectsOfType<BuyButton>();
            for(int i = 0; i < data.Count; i++)
            {
                foreach (var button in shopButtons) 
                {
                    if (button != null && button.TargetPrefab.name == data[i])
                    {
                        GameObject soldTextPrefab = UnityEngine.Resources.Load("Prefabs/SoldText") as GameObject;
                        Instantiate(soldTextPrefab, button.transform.position, Quaternion.identity);
                        Mirror.GetMirror().AddBall(button.TargetPrefab);
                        Destroy(button.gameObject);
                    }
                }
            }
        }
    }
}