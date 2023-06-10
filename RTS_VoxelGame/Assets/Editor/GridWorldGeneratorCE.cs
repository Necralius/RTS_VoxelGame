using UnityEditor;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [CustomEditor(typeof(GridWorldGenerator))]
    public class GridWorldGeneratorCE : Editor
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //GridWorldGeneratorCE - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridWorldGenerator gridScript = (GridWorldGenerator)target;

            if (GUILayout.Button("Regenerate World")) gridScript.GenerateCompleteMap();

        }
    }
}