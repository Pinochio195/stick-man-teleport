using UnityEngine;

public class BulletBumerang : MonoBehaviour
{
    private Vector3 startPosition;
    private float t = 0f;
    private GameObject Player;
    private Rigidbody rb;
    public float speed = 10f;
    public float curveHeight = 2f;
    public float curveFrequency = 2f;
    public float returnTime = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        Player= GameObject.FindWithTag("Enemy");
    }

    public void Shoot()
    {
        Vector3 direction = (new Vector3(Player.transform.position.x,Player.transform.position.y+1,Player.transform.position.z)-transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void FixedUpdate()
    {
        if (t < 1f)
        {
            t += Time.fixedDeltaTime / returnTime;
            Vector3 nextPos = ParabolicCurve(startPosition, Player.transform.position + Vector3.up * curveHeight, curveFrequency, t);
            Vector3 direction = (nextPos - transform.position).normalized;
            rb.velocity = direction * speed;
            transform.LookAt(transform.position + direction);
        }
        else
        {
            Vector3 direction = (startPosition - transform.position).normalized;
            rb.velocity = direction * speed;
            transform.LookAt(transform.position + direction);
        }
    }

    private Vector3 ParabolicCurve(Vector3 start, Vector3 end, float freq, float t)
    {
        float phase = t * freq * 2f * Mathf.PI;
        float sin = Mathf.Sin(phase);
        float cos = Mathf.Cos(phase);

        return Vector3.Lerp(start, end, t) + new Vector3(0f, sin * curveHeight, cos * curveHeight);
    }
}