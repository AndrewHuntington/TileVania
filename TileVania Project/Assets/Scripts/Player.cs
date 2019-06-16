using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput; // Needed to access CrossPlatformInputManager
// May need to import Standard Assests from Assest Store in order to acess CrossPlatformInput


public class Player : MonoBehaviour {

    // Config
    [SerializeField] float runSpeed = 5f; // Modifies run speed by being multiplied to controlThrow
    [SerializeField] float jumpSpeed = 5f; // Used in conjunction w/Edit > Project Settings > Physics 2D > Gravity (y value) to control jump feel
    [SerializeField] float climbSpeed = 5f;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody; // Needed to control the player sprite
    Animator myAnimator; // Needed to control animation states
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;
    

	// Message then methods
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isAlive) { return; } // stops player from moving after death

        Run();
        Jump();
        ClimbLadder();
        FlipSprite();
        Die();
        
    }

    // Controls movement along the X axis
    // Go to Edit > Project Settings > Input > Horizontal > Sensitivity to adjust input responsiveness
    // Higer values (e.g. 50) may be prefered
    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        // playerHasHorizontalSpeed checks if the player is moving
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is a stand in for 0
        myAnimator.SetBool("Running", playerHasHorizontalSpeed); // accesses the Running value in Paramters; playerHasHorizontalSpeed acts as on/off switch     
    }

    private void Jump()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; } //prevents mid-air jumping

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
        
    }

    private void ClimbLadder()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("Climbing", false);
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

    }


    // Checks which way the character is moving and flips the sprite accordingly
    private void FlipSprite()
    {
        // First, check if the player is moving
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is a stand in for 0

        // Mathf.Sign takes a number and returns a value of 1 if the value is positive or 0, -1 if the value is negative
        if (playerHasHorizontalSpeed)
        {
            //transform.localScale = new Vector2(x,y)  -- allows you access to the X and Y values of Transform > Scale
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Death");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

}
