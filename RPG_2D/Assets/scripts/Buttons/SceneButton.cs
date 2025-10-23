using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public Fader fader;

    // Call from a UI Button OnClick
    public void ChangeToGameScene()
    {
        if (fader == null)
            fader = Object.FindFirstObjectByType<Fader>();

        if (fader != null)
            fader.Fade(true);
        else
            Debug.LogWarning("No Fader found or assigned.");
    }
}
