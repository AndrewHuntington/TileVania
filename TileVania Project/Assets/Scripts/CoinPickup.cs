using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int coinValue = 100;

    bool addedToScore = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!addedToScore)
        {
            addedToScore = true;

            // because PlayClipAtPoint() by default plays the audio where the obj is in 3D space, 
            // the second parameter is the main camera position
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(coinValue);
            Destroy(gameObject);
        }
        
    }
}
