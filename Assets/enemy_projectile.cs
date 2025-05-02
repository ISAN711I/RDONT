using UnityEngine;

public class enemy_projectile : MonoBehaviour
{
  
    public float speed;
 public float damage;
    void Start()
    {
        Invoke("kill",2f);
        speed += speed*(Random.value-.5f)/10;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right*speed*Time.deltaTime);
    }
private void OnCollisionEnter2D(Collision2D collision)
{
if (collision.gameObject.CompareTag("Player"))
{
PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
if (playerHealth != null)
{
playerHealth.TakeDamage(damage);
 Destroy(gameObject);
}
}
}
void kill(){
    Destroy(gameObject);
}
}
