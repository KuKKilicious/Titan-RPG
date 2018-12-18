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
        raycaster.notifyLayerChangeObservers += OnLayerChange;
	}
	
	// Update is called once per frame
	void OnLayerChange (int newLayer) {
        Debug.Log("OnLayerChange: " + newLayer.ToString());
        switch (newLayer) {
            case 5:
                Debug.Log("toDo: need to set UI Cursor here");
                break;
            case 10:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;
            case 9:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(questionCursor, cursorHotspot, CursorMode.Auto);
                break;
            
        }
	}
}
