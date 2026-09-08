using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    public AudioClip collectSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCoins(value);
            if (collectSound != null) AudioManager.Instance.PlaySFX(collectSound);
            Destroy(gameObject);
        }
    }
}
