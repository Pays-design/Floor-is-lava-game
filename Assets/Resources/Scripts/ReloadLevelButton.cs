using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(Button))]
    public class ReloadLevelButton : MonoBehaviour
    {
        public event System.Action OnReloadLevel;

        private void Start() => GetComponent<Button>().onClick.AddListener(ReloadLevel);

        public void ReloadLevel()
        {
            OnReloadLevel?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
