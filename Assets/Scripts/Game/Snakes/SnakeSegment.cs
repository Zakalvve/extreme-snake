using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public class SnakeSegment
    {
        public GameObject Segment { get; set; }
        public SpriteRenderer Renderer { get; set; }
        public Vector2Int ModelPosition { get; set; }
        public int Layer { get { return Segment.layer; } }
        public SnakeSegment(GameObject segment,Vector2Int modelPosition) {
            Segment = segment;
            ModelPosition = modelPosition;
            Renderer = segment.GetComponent<SpriteRenderer>();
        }

        public void ChangeLayer(int layer) {
            Segment.layer = layer;
            Segment.GetComponent<SpriteRenderer>().sortingLayerName = LayerMask.LayerToName(layer);

            SpriteRenderer[] spriteRenderers = Segment.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in spriteRenderers) {
                renderer.sortingLayerName = LayerMask.LayerToName(layer);
            }
        }
    }
}
