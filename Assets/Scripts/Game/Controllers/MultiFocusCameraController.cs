using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;

public class MultiFocusCameraController : MonoBehaviour
{
    //debug properties
    public bool debug = false;
    public Transform mid;
    public Transform xmid;
    public Transform ymid;

    //list of the focuses of this camera
    public List<Transform> foci = new List<Transform>();

    //movement properties
    public float buffer = 0.1f;


    protected float minZoomRatio = 0.5f;
    public float minOrthographicSize = 5f;
    public float maxOrthographicSize = 25f;
    public float zoomSpeed = 5f;
    public float followSpeed = 5f;

    protected virtual void Start() {
        GameManager.Instance?.GameEmitter.Subscribe<SnakeCreatedEventArgs>("SnakeCreated",AddFocus);
    }

    public void AddFocus(object sender,SnakeCreatedEventArgs args) {
        foci.Add(args.SnakeHead);
    }

    protected (Vector3, float) GetPositionData(List<Transform> verticies) {
        if (verticies.Count < 1) throw new System.Exception();
        if (verticies.Count == 1) return (verticies[0].position, 0f);

        Vector3 xFrom = Vector3.zero, xTo = Vector3.zero, yFrom = Vector3.zero, yTo = Vector3.zero;

        float x = 0f;
        float y = 0f;

        float distance = 0f;

        foreach (var vertex in verticies) {
            verticies.Where(v => v != vertex).ToList().ForEach(v => {
                distance = Mathf.Max(distance,Vector3.Distance(vertex.position,v.position));
                Vector3 diff = v.position - vertex.position;
                if (diff.x >= x) {
                    x = diff.x;
                    xFrom = vertex.position;
                    xTo = v.position;
                }
                if (diff.y >= y) {
                    y = diff.y;
                    yFrom = vertex.position;
                    yTo = v.position;
                }
            });
        }

        Vector3 xPos = (xFrom + xTo) / 2f;
        Vector3 yPos = (yFrom + yTo) / 2f;

        if (debug) {
            if (ymid != null && xmid != null) {
                xmid.position = xPos;
                ymid.position = yPos;
            }
            Debug.DrawLine(xFrom,xTo,Color.green);
            Debug.DrawLine(yFrom,yTo,Color.red);
        }


        return (new Vector3(xPos.x,yPos.y,0), distance);
    }
}
