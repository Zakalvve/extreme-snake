using ExtremeSnake.Game;
using ExtremeSnake.Game.Data;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PixelPerfectWithZoom))]
public class PixelPerfectMultiplayerCameraController : MultiFocusCameraController
{
    public float DefaultZoom;
    private PixelPerfectWithZoom zoomControls;
    [Range(0f, 1f)]
    public float scaleFactor;

    protected override void Start() {
        zoomControls = GetComponent<PixelPerfectWithZoom>();
        base.Start();
        GameManager.Instance.GameEmitter.Subscribe<GameObjectEventArgs>("CameraDropFocus",DropFocus);
    }

    private void FixedUpdate() {
        if (foci.Count == 0) return;

        var (midPoint, distance) = GetPositionData(foci);
        if (debug && mid != null) mid.position = midPoint;

        // Move camera towards the midpoint
        transform.position = Vector3.Lerp(transform.position,midPoint,followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x,transform.position.y,-10);

        if (distance > 0) {
            distance += 1;
            // Calculate the target orthographic size based on the distance and aspect ratio
            float aspectRatio = Screen.width / (float)Screen.height;
            float width = distance * aspectRatio;
            float height = distance;

            float ppu = Screen.width / (Mathf.Max(width,height) * zoomControls.pixelsPerUnit * (1f + buffer));

            // Set the calculated assetsPPU to achieve the desired zoom level
            zoomControls.SetZoom(Mathf.Clamp(ppu, minOrthographicSize, maxOrthographicSize));
        } else {
            zoomControls.SetZoom(DefaultZoom);
        }
    }

    public void DropFocus(object sender, GameObjectEventArgs args) {
        foci.Remove(args.GO.transform);
    }
}