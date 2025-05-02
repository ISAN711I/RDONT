using UnityEngine;

public class wojack_ai : MonoBehaviour
{
    bool isrunning = false;

    public GameObject projectile1;
    
    public float rotate_speed;

    public Transform bullet_pos;

    public GameObject minion;
    float rotationz=0;
    void Start()
    {
        Invoke("Begin", 1f);
        Invoke("summon_minions", 2f);
    }

    void Update()
    {
        if (isrunning)
        {
            // Generate a random rotation
            rotationz += rotate_speed*Time.deltaTime;
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, rotationz);  // Create a Quaternion with random Z rotation
            
            // Spawn the projectile at the current position with the random rotation
            Instantiate(projectile1,bullet_pos.position, randomRotation);
        }
    }

    void Begin()
    {
        isrunning = true;
    }
    void summon_minions(){
        Instantiate(minion,transform.position,Quaternion.identity);
        Invoke("summon_minions",.5f);
    }
}
