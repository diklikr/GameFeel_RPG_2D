using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Animator animator;

    GhostState gstate;
    private void UpdateState(GhostState state)
    {
        gstate = state;

        switch (state)
        {
            case GhostState.Idle:

                break;

            case GhostState.Walk:


                break;

            case GhostState.Attack:

                break;

            case GhostState.Dead:
                gameObject.SetActive(false);
                break;
        }
    }
}
