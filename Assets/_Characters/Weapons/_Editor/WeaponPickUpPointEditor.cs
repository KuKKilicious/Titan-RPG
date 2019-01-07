
using UnityEditor;
using UnityEngine;
namespace RPG.Characters
{

    [CustomEditor(typeof(WeaponPickUpPointEditor))]
    public class WeaponPickUpPointEditor : Editor
    {
        WeaponPickupPoint mySelf;
        private void OnEnable()
        {
            mySelf = target as WeaponPickupPoint;
            if(mySelf == null|| serializedObject == null)
            {
                DestroyImmediate(this);
                return;
            }
        }


        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();
            if (EditorGUI.EndChangeCheck())
            {
                mySelf.RefreshPrefab();
            }
        }
    }
}
