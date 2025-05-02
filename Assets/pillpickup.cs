using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int value = 1; // Amount to add (coins, health, etc.)
    public AudioClip collectSound; // Optional sound effect

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Optionally play sound
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Call a method on the player (optional)
            PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddCoins(value); // Replace with your own method if needed
            }

            // Destroy the collectable object
            Destroy(gameObject);
        }
    }
}
