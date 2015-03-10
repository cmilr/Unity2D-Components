using UnityEngine;
using System.Collections;


public class BaseBehaviour : MonoBehaviour {

	protected const int TILE_SIZE = 16;
	protected const float ALIGN_ENTITY_TO = .124f;
	protected const float HUD_FADE_IN_AFTER = .75f;
	protected const float HUD_FADE_OUT_AFTER = .25f;

    protected void Dbug()
    {
        Debug.Log(">>>>>>>> DEBUG METHOD CALLED <<<<<<<<");
    }
}
