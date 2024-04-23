using UnityEngine;

public class OmnidirectionalParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float[] layerParallaxMultipliers;

    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
        if (layerParallaxMultipliers.Length != transform.childCount) {
            Debug.LogWarning("Please ensure you have set parallax multipliers for all background layers.");
        }
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            // Applying custom multiplier for each layer
            float parallaxMultiplier = layerParallaxMultipliers[i];
            Vector3 parallaxMovement = new Vector3(deltaMovement.x * parallaxMultiplier, deltaMovement.y * parallaxMultiplier, 0);

            // Optional: Use a smoother linear interpolation for subtle effect
            child.position -= parallaxMovement;
            // or child.position = Vector3.Lerp(child.position, child.position - parallaxMovement, Time.deltaTime * smoothFactor);
        }

        lastCameraPosition = cameraTransform.position;
    }
}
