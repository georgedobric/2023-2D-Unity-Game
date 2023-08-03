using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Range(1, 50)]
    public float jumpVelocity;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown ("Jump"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
        }
    }
}
