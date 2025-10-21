using UnityEditor;
using UnityEngine;

namespace FirAnimations
{
    [CustomEditor(typeof(FirAnimationsManager))]
    public class FirAnimationsManagerEditor : Editor
    {
        private bool isPlaying;

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                DrawDefaultInspector();
                return;
            }

            FirAnimationsManager script = (FirAnimationsManager)target;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("<<"))
            {
                Off();
                script.ToStartPoint();
            }

            if (GUILayout.Button(isPlaying ? "ll" : ">"))
            {
                if (isPlaying)
                {
                    Off();
                }
                else
                {
                    EditorApplication.update += script.Update;
                    isPlaying = true;
                    script.OnEndAllAnimations += Off;
                    script.Initialize();
                    script.StartAnimations();
                }
            }

            if (GUILayout.Button(">>"))
            {
                Off();
                script.ToEndPoint();
            }

            EditorGUILayout.EndHorizontal();
            DrawDefaultInspector();
        }

        private void Off()
        {
            FirAnimationsManager script = (FirAnimationsManager)target;

            isPlaying = false;
            script.OnEndAllAnimations -= Off;
            EditorApplication.update -= script.Update;
        }
    }
    [CustomPropertyDrawer(typeof(FirAnimationManagerComponent))]
    public class Vector2IntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
        
            position = EditorGUI.PrefixLabel(position, label);
        
            var xProp = property.FindPropertyRelative("Animation");
            var yProp = property.FindPropertyRelative("StartTime");
        
            float width = position.width / 4f - 2f;
        
            Rect xRect = new Rect(position.x, position.y, width*3, position.height);
            Rect yRect = new Rect(position.x + width*3 + 4f, position.y, width, position.height);
        
            EditorGUI.PropertyField(xRect, xProp, GUIContent.none);
            EditorGUI.PropertyField(yRect, yProp, GUIContent.none);
        
            EditorGUI.EndProperty();
        }
    }
}