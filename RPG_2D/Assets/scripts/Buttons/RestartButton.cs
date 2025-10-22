using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void ChangetoStartScene()
    {
        SceneManager.LoadScene(0);
    }
}
