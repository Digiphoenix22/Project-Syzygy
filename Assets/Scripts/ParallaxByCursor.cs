using UnityEngine;

public class ParallaxByCursor : MonoBehaviour
{
    public float parallaxEffectMultiplier = 0.05f;
    private Vector2 startPosition;

    void Start()
    {
        // Save the starting position of the layer
        startPosition = transform.position;
    }

    void Update()
    {
        // Get the cursor position relative to the screen center
        Vector2 cursorPosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 fromCenter = cursorPosition - screenCenter;

        // Calculate new position based on cursor position
        Vector2 newPosition = startPosition + fromCenter * parallaxEffectMultiplier;

        // Apply the new position
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
