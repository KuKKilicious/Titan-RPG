using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityStandardAssets.CinematicEffects
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Bloom_RFX4))]
    public class BloomEditor_RFX4 : Editor
    {
        [NonSerialized]
        private List<SerializedProperty> m_Properties = new List<SerializedProperty>();

        BloomGraphDrawer_RFX4 _graph;

        bool CheckHdr(Bloom_RFX4 target)
        {
            var camera = target.GetComponent<Camera>();
#if UNITY_5_6_OR_NEWER
            return camera != null && camera.allowHDR;
#else
            return camera != null && camera.hdr;
#endif
        }

        void OnEnable()
        {
            var settings = FieldFinder_RFX4<Bloom_RFX4>.GetField(x => x.settings);
            foreach (var setting in settings.FieldType.GetFields())
            {
                var prop = settings.Name + "." + setting.Name;
                m_Properties.Add(serializedObject.FindProperty(prop));
            }

            _graph = new BloomGraphDrawer_RFX4();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.Space();
                var bloom = (Bloom_RFX4)target;
                _graph.Prepare(bloom.settings, CheckHdr(bloom));
                _graph.DrawGraph();
                EditorGUILayout.Space();
            }

            foreach (var property in m_Properties)
                EditorGUILayout.PropertyField(property);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
