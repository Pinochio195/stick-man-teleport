using UnityEngine;
public class ObjectMover : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.z != 4)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 4);
        }
    }
}