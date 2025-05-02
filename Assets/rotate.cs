using UnityEngine;

public class rotate : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    private float time;

    public float frequency;
    public float amplitude;
    void Update()
    {
        time += Time.deltaTime;
         transform.Rotate(0, speed * Time.deltaTime, 0);
         transform.Translate(0,amplitude*Mathf.Cos(frequency*time),0);
    }
}
