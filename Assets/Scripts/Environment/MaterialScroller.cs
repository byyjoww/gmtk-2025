using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialScroller : MonoBehaviour
{
    [Tooltip("Speed of scrolling in UV units per second (can be negative).")]
    public Vector2 scrollSpeed = new Vector2(0.5f, 0f); // X = horizontal, Y = vertical

    private Renderer rend;
    private Vector2 currentOffset = Vector2.zero;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Time.deltaTime ensures smooth scrolling regardless of frame rate
        currentOffset += scrollSpeed * Time.deltaTime;

        // Apply the offset to the material's main texture
        rend.material.SetTextureOffset("_BaseMap", currentOffset);
    }
}