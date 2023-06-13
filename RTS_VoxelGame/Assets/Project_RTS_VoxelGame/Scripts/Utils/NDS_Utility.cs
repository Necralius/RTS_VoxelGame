using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public static class NDS_Utility
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //NDS_Utility - (0.2)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public const int sortingOrderDefault = 5000;

        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition = default(Vector3), Vector3 rotation = default(Vector3), int fontSize = 40, Color? color = null, Vector3 scale = default(Vector3), int sortingOrder = sortingOrderDefault, TextAlignment textAlignment = TextAlignment.Center)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, sortingOrder, rotation, scale, textAlignment);
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, int sortingOrder, Vector3 rotation, Vector3 scale, TextAlignment textAlignment)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));

            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.Rotate(rotation);
            transform.localScale = scale;

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
    }
}