using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace ExtremeSnake.Utils
{
    public class UtilsClass
    {
        public static (int, int) SecondsToMinutesAndSeconds(int timeInSeconds) {
            return (timeInSeconds / 60, timeInSeconds % 60);
        }

        public static string FormatTime(int minutes, int seconds) {
            return String.Format("{0}:{1:00}",minutes,seconds);
        }

        public static int FloorPower2(int x) {
            if (x < 1) {
                return 1;
            }
            return (int)Math.Pow(2,(int)Math.Log(x,2));
        }

        public static Vector2Int DirectionTo(Vector2Int from,Vector2Int to) {
            return to - from;
        }
        public static Vector2 DirectionTo(Vector2 from,Vector2 to) {
            return to - from;
        }
        public static Vector3 DirectionTo(Vector3 from, Vector3 to) {
            return to - from;
        }

        public static Vector3 GetMouseWorldPosition() {
            Vector3 v = GetMouseWorldPositionWithZ(Input.mousePosition,Camera.main);
            v.z = 0f;
            return v;
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            return worldCamera.ScreenToWorldPoint(screenPosition);
        }
        public static TextMesh CreateWorldText(string text,Transform parent = null,Vector3 localPosition = default(Vector3),int fontSize = 40,Color color = default(Color),TextAnchor textAnchor = default(TextAnchor),TextAlignment textAlignment = default(TextAlignment),int sortingOrder = 0) {
            if (color == null) color = Color.white;
            return CreateWorldText(parent,text,localPosition,fontSize,(Color)color,textAnchor,textAlignment,sortingOrder);
        }
        private static TextMesh CreateWorldText(Transform parent,string text,Vector3 localPosition,int fontSize,Color color,TextAnchor textAnchor,TextAlignment textAlignment,int sortingOrder) {
            GameObject gameObject = new GameObject("World Text",typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent,false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        private static readonly object sync = new object();

        public static T RandomElement<T>(IEnumerable<T> enumerable) {
            if (enumerable == null)
                throw new System.ArgumentNullException("enumerable");

            var count = enumerable.Count();

            var ndx = 0;
            lock (sync)
                ndx = UnityEngine.Random.Range(0,count); // returns non-negative number less than max

            return enumerable.ElementAt(ndx);
        }
    }
}
