using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
   public void ChangetoGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
