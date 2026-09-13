using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
   
    void OnMouseDown()
    {
     
        SceneManager.LoadScene("GameScene");
    }
}