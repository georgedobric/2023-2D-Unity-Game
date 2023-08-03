using System.Collections;
using UnityEngine;

public class BetterJump : MonoBehaviour {
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rbb;

    // Start is called before the first frame update
    void Awake() {
        rbb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (rbb.velocity.y > 0) {
            rbb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rbb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rbb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}