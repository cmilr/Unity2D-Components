using UnityEngine;
using System.Collections;

public class CreatureState : BaseBehaviour {

    private bool blocked;

    private void SetBlockedState(bool status)
    {
        blocked = status;
    }

    private bool GetBlockedState()
    {
        return blocked;
    }
}
