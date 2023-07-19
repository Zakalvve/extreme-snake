using ExtremeSnake.Game;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class MultiplayerCameraController : MonoBehaviour
{
    public List<Transform> foci = new List<Transform>();
    public float buffer = 0.8f; // Buffer ratio around the players
    public int minOrthographicSize = 8;
    public int maxOrthographicSize = 25;
    public float zoomSpeed = 1f;
    public float followSpeed = 5f;
    public Transform mid;
    public Transform xmid;
    public Transform ymid;

    private PixelPerfectCamera pixelPerfectCamera;
    private int targetPPU;
    public int defaultPPU;
    public bool debug = false;

    private void Start() {
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        targetPPU = pixelPerfectCamera.assetsPPU;
        GameManager.Instance?.GameEmitter.Subscribe<CameraEventArgs>("OnPlayerSnakeCreated",AddFocus);
    }

    public void AddFocus(object sender, CameraEventArgs args) {
        foci.Add(args.Focus);
    }

    private void FixedUpdate() {
        if (foci.Count == 0) return;

        var (midPoint, distance) = GetPositionData(foci);
        if (debug && mid != null) mid.position = midPoint;

        // Move camera towards the midpoint
        transform.position = Vector3.Lerp(transform.position,midPoint,followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x,transform.position.y,-10);

        if (distance > 0) {
            // Calculate the target orthographic size based on the distance and aspect ratio
            float aspectRatio = Screen.width / (float)Screen.height;
            float ppuX = (pixelPerfectCamera.refResolutionX) / (distance * aspectRatio);
            float ppuY = (pixelPerfectCamera.refResolutionY) / (distance * aspectRatio);

            targetPPU = Mathf.RoundToInt(Mathf.Lerp(pixelPerfectCamera.assetsPPU,Mathf.Clamp(Mathf.Max(ppuX,ppuY) * buffer,minOrthographicSize,maxOrthographicSize),zoomSpeed));


            // Set the calculated assetsPPU to achieve the desired zoom level
            pixelPerfectCamera.assetsPPU = targetPPU;
        }
    }

    private (Vector3, float) GetPositionData(List<Transform> verticies) {
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
                if (diff.x > x) {
                    x = diff.x;
                    xFrom = vertex.position;
                    xTo = v.position;
                }
                if (diff.y > y) {
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
