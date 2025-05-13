using UnityEngine;

public class random_rotation : MonoBehaviour
{
    public float maxAngularVelocity = 5f;   // Maximum magnitude of angular velocity
    public float interval = 1f;             // Time between velocity updates

    private Rigidbody rb;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity; // Optional: limit spin speed
        timer = interval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ApplyRandomAngularVelocity();
            timer = interval;
        }
    }

    void ApplyRandomAngularVelocity()
    {
        Vector3 randomAngularVelocity = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * Random.Range(0f, maxAngularVelocity);

        rb.angularVelocity = randomAngularVelocity;
    }
}
