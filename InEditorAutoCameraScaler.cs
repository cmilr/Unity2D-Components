using UnityEngine;
using UnityEngine.UI;

// keeps editor Game View in sync with camera & resolution settings.
// press play to refresh Game View.
[ExecuteInEditMode]
public class InEditorAutoCameraScaler : BaseBehaviour {

    private CanvasScaler[] canvasScaler;

	void Start() 
    {
        canvasScaler = Camera.main.GetComponentsInChildren<CanvasScaler>();
	}

    void Update()
    {
        Camera.main.orthographicSize = FINAL_ORTHOGRAPHIC_SIZE;

        foreach (CanvasScaler c in canvasScaler)
        {
            c.scaleFactor = CANVAS_SCALE;
            c.referencePixelsPerUnit = BASE_PIXELS_PER_UNIT;
        }
    }
}
