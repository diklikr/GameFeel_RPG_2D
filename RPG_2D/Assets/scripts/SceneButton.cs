using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
   public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
