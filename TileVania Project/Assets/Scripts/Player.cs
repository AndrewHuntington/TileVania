using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput; // Needed to access CrossPlatformInputManager

public class Player : MonoBehaviour {

    [SerializeField] float runSpeed = 5f; // Modifies run speed by being multiplied to controlThrow

    Rigidbody2D myRigidBody; // Needed to control the player sprite
    

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Run();
	}

    // Controls movement along the X axis
    // Go to Edit > Project Settings > Input > Horizontal > Sensitivity to adjust input responsiveness
    // Higer values (e.g. 50) may be prefered
    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }
}
