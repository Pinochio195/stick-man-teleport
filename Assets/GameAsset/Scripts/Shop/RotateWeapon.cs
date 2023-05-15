using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    public float speed = 1f; // tốc độ di chuyển
    private float amplitude = 0.125f; // khoảng cách di chuyển

    private Vector3 initialPosition; // vị trí ban đầu của vật thể
    private Vector3 globalOffset;
    private void Start()
    {
        initialPosition = transform.position;
        globalOffset = transform.TransformDirection(Vector3.forward * amplitude);
    }

    private void Update()
    {
        // di chuyển vật thể lên-xuống theo global space
        Vector3 offset = Mathf.Sin(Time.time * speed) * globalOffset;
        transform.position = initialPosition + offset;
    }
}