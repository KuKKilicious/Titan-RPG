using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CameraRaycaster))]
public class RayCursor : MonoBehaviour {
    [SerializeField]
    Texture2D walkCursor = null;

    [SerializeField]
    Texture2D attackCursor = null;

    [SerializeField]
    Texture2D questionCursor = null;
    [SerializeField] Vector2 cursorHotspot= new Vector2(0,0);
    CameraRaycaster raycaster;
	// Use this for initialization
	void Start () {
        raycaster = GetComponent<CameraRaycaster>();
        raycaster.onLayerChange += OnLayerChange;
	}
	
	// Update is called once per frame
	void OnLayerChange (Layer newLayer) {
        switch (newLayer) {
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(questionCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("Cursor to show is not set for layer:" + raycaster.currentLayerHit);
                return;
        }
	}
}
