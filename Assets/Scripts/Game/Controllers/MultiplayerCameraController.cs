using ExtremeSnake.Game;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PixelPerfectWithZoom))]
public class MultiplayerCameraController : MultiFocusCameraController
{
    private Camera mainCamera;
    private float targetOrthographicSize;

    protected override void Start() {
        mainCamera = GetComponent<Camera>();
        targetOrthographicSize = mainCamera.orthographicSize;
        
        base.Start();
    }

    private void FixedUpdate() {
        if (foci.Count == 0) return;

        var (midPoint, distance) = GetPositionData(foci);
        if (debug && mid != null) mid.position = midPoint;

        // Move camera towards the midpoint
        transform.position = Vector3.Lerp(transform.position,midPoint,followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x,transform.position.y,-10);

        if (distance > 0) {
            float aspectRatio = Screen.width / (float)Screen.height;
            float width = distance * aspectRatio * (1f + buffer);
            float height = distance * (1f + buffer);

            targetOrthographicSize = Mathf.Clamp(Mathf.Max(width,height) * minZoomRatio,minOrthographicSize,maxOrthographicSize);

            // Smoothly adjust the orthographic size towards the target size
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize,targetOrthographicSize,zoomSpeed * Time.deltaTime);
        }
    }
}
