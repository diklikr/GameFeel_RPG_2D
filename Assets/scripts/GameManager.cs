using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public player p;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
