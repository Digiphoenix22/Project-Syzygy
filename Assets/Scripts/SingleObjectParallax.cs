using UnityEngine;

public class SingleObjectParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffectMultiplier = 0.5f;

    private Vector3 lastCameraPosition;
    private Vector3 parallaxMovement;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        parallaxMovement = new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);

        transform.position -= parallaxMovement;
        lastCameraPosition = cameraTransform.position;
    }
}
