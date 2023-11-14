using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    #region Methods
    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }
    public void Reload()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}