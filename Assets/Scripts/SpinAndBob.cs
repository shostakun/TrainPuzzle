using UnityEngine;

public class SpinAndBob : MonoBehaviour
{
    protected float baseY;
    public float bobAmplitude = 0.1f;
    public float bobFrequency = 1f;
    public float spinSpeed = 1f;

    void Start()
    {
        baseY = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            baseY + bobAmplitude * Mathf.Sin(Time.time * bobFrequency),
            transform.position.z
        );
        transform.Rotate(Vector3.up, Time.deltaTime * spinSpeed * 180);
    }
}
