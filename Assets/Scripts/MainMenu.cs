using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("TicTacToe");
    }

    public void Quit()
    {
        Debug.Log("Quitting Application...");
        Application.Quit(0);
    }
}
