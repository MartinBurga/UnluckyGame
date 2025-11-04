using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 🔹 Llamar esta función desde el botón "Jugar"
    public void PlayGame()
    {
        SceneManager.LoadScene("GameAlpha");
    }

    // 🔹 Si tienes botón "Salir"
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Esto solo funciona en compilado, no en el editor.
    }
}
