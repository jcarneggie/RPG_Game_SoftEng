using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void StartFloor1Normal()
    {
        Debug.Log("Memuat Scene: Floor1 Normal...");


        SceneManager.LoadScene("Floor1 Normal");
    }
}