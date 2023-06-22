using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [CustomEditor(typeof(SelectableResource))]
    public class SelectableResourceCE : Editor
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SelectableResource selectableResource = (SelectableResource)target;

            if (selectableResource.GetWithRandomFactor)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(string.Format("Min Value: {0:00.0}", selectableResource.itemMinQuantity), GUILayout.Width(110f), GUILayout.Height(20f));

                EditorGUILayout.LabelField(string.Format("Max Value: {0:00.0}", selectableResource.itemMaxQuantity), GUILayout.Width(110f), GUILayout.Height(20f));

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider(ref selectableResource.itemMinQuantity, ref selectableResource.itemMaxQuantity, 0, 100, GUILayout.Height(20f), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}