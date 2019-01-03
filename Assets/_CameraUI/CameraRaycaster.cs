using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Characters; //to detect Type
namespace RPG.CameraUI
{


    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField]
        float maxRaycastDepth = 100f; // Hard coded value

        [SerializeField]
        Texture2D walkCursor = null;
        [SerializeField]
        Texture2D enemyCursor = null;
        [SerializeField]
        Vector2 cursorHotspot = new Vector2(0, 0);

        const int WALKABLE_LAYER = 9;
        //new delegates
        public delegate void OnMouseOverWalkable(Vector3 destination); // declare new delegate type
        public event OnMouseOverWalkable notifyMouseOverWalkableObservers; // instantiate an observer set
        // OnMouseOverEnemy(Enemy enemy)
        public delegate void OnMouseOverEnemy(Enemy enemy); // declare new delegate type
        public event OnMouseOverEnemy notifyMouseOverEnemy; // instantiate an observer set

        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //UI interaction
                return; // Stop looking for other objects
            }
            else
            {
                PerformRaycasts();
            }

        }

        private void PerformRaycasts()
        {
            //Specify layer priorities here
            //enemy first
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RaycastForEnemy(ray)) { return; }
            //for walkable
            if (RaycastForWalkable(ray)) { return; }
        }

        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            if (!gameObjectHit) { return false; }
            Enemy enemy = gameObjectHit.GetComponent<Enemy>();
            if (enemy)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                notifyMouseOverEnemy(enemy);
                return true;
            }
            return false;
        }

        private bool RaycastForWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask walkableLayer = 1 << WALKABLE_LAYER;
            bool walkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, walkableLayer);
            if (walkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                notifyMouseOverWalkableObservers(hitInfo.point);
            }
            return walkableHit;
        }

    }
}