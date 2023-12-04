using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    public Vector2 startingPosition;
    // Start Z position for the parallax game object
    float startingZ;

    // Distance the camera has moved from starting position off the parallax Object
    // arrow method ensures update on every frame
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    // If object is in front of target use nearClipPlane else use farClipPlane
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //The further the Object is from the player, the faster the ParallaxEffect object will move. Drag z value closer to target  to move slower
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        //cast to Vector2
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //When the target moves, move the parallax Object the same distance times a multiplier
        Vector2 newPosition = startingPosition = camMoveSinceStart * parallaxFactor;

        // The X/Y position changes based on target travel speed times the parallaxFactor, but z stays consistent
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);

    }
}
