using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pipeline;
using ExtremeSnake.Game;
using Assets.Testing;
using Assets.Scripts.Game.Data;
using System;

public class CinematicCameraController : MonoBehaviour
{
    public List<Transform> subjects;
    public float transitionDuration = 0.8f; // Time taken to transition to the next position
    public float waitDuration = 2f; // Time to wait at each position
    public float minZoom;
    public float maxZoom;
    [Range(0f, 5f)]
    public float buffer;
    public Camera Cinecam;

    private void Start() {
        Cinecam = GetComponent<Camera>();
        GameManager.Instance.GameEmitter.Subscribe<CinematicPipelineEventArgs>("CinematicCreate",HandleCreateCinematic);
    }

    public void HandleCreateCinematic(object sender,CinematicPipelineEventArgs args) {
        args.Pan = CameraProcess;
        args.Cinecam = Cinecam;
    }

    public IEnumerator CameraProcess(CinematicArgs args,Func<IEnumerator> next) {
        Vector3 initialPosition = transform.position;
        float initialZoom = Cinecam.orthographicSize;
        Vector3 targetPosition = new Vector3(args.Subject.position.x,args.Subject.position.y,-10f);
        float targetOrthographicSize = GetZoomedOrthographicSize(args.Subject);
        yield return MoveCameraToPosition(targetPosition,targetOrthographicSize);
        yield return next();
        yield return MoveCameraToPosition(initialPosition,initialZoom);
    }
    
    // Function to start the cinematic camera movement
    public void StartCinematic(List<Transform> positions) {
        StartCoroutine(MoveCamera(positions));
    }

    // Coroutine to move the camera through each position in a list
    private IEnumerator MoveCamera(List<Transform> positions) {
        Vector3 initialPosition = transform.position;
        float initialOrthographicSize = Cinecam.orthographicSize;

        // Move to the initial position
        yield return MoveCameraToPosition(initialPosition,initialOrthographicSize);

        // Move through the list of positions
        for (int i = 0; i < positions.Count; i++) {
            Vector3 targetPosition = new Vector3(positions[i].position.x,positions[i].position.y,-10f);
            float targetOrthographicSize = GetZoomedOrthographicSize(positions[i]);

            // Move to the target position
            yield return MoveCameraToPosition(targetPosition,targetOrthographicSize);

            // Wait for a few seconds
            yield return new WaitForSeconds(waitDuration);
            // Return to the initial position at the end of the cutscene
            yield return MoveCameraToPosition(initialPosition,initialOrthographicSize);
        }
    }

    // Coroutine to move the camera to a specific position with zooming
    private IEnumerator MoveCameraToPosition(Vector3 targetPosition,float targetOrthographicSize) {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        float initialOrthographicSize = Cinecam.orthographicSize;

        while (elapsedTime < transitionDuration) {
            elapsedTime += Time.deltaTime;

            // Calculate the current lerp value based on the time elapsed and the transition duration
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            // Move the camera position and zoom towards the target position and orthographic size
            transform.position = Vector3.Lerp(initialPosition,targetPosition,t);
            Cinecam.orthographicSize = Mathf.Lerp(initialOrthographicSize,targetOrthographicSize,t);

            yield return null;
        }
    }

    // Function to calculate the orthographic size needed to zoom into a specific position
    private float GetZoomedOrthographicSize(Transform targetTransform) {
        // Calculate the distance between the target position and the camera
        //float distanceToTarget = Vector3.Distance(targetPosition.position,transform.position);
        float aspectRatio = Screen.width / (float)Screen.height;
        float w = Mathf.Abs(targetTransform.localScale.x) * (1 + buffer);
        float h = Mathf.Abs(targetTransform.localScale.y) * (1 + buffer);
        // Calculate the desired orthographic size based on the distance and buffer ratio
        float desiredOrthographicSize = Mathf.Clamp(Mathf.Max(w,h) * 0.5f, minZoom, maxZoom);

        return desiredOrthographicSize;
    }
}

