using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // The length of the sprite
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect; // Parallax scrolling
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Preload the next background to avoid gaps
        if (movement > startPos + length / 2) // Adjusted to trigger earlier
        {
            startPos += length;
        }
        else if (movement < startPos - length / 2) // Adjusted to trigger earlier
        {
            startPos -= length;
        }
    }
}
