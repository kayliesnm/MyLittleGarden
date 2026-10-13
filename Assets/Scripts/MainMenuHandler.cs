using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuHandler : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Play Game");
        SceneManager.LoadScene("GameScene");
    }

        public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

}
