using UnityEngine;

public class SizeAutoCamera : MonoBehaviour
{
    public float targetAspect = 1080f / 1920f; // Tỉ lệ khung hình mục tiêu của bạn
    public float minFov = 30f; // Giá trị FOV tối thiểu
    public float maxFov = 60f; // Giá trị FOV tối đa

    private Camera cam;
    private float currentAspect;

    private void Start()
    {
        cam = GetComponent<Camera>();
        currentAspect = (float)Screen.width / (float)Screen.height;
        UpdateCameraAspect();
    }

    private void Update()
    {
        // Kiểm tra nếu tỉ lệ khung hình của thiết bị thay đổi
        if ((float)Screen.width / (float)Screen.height != currentAspect)
        {
            currentAspect = (float)Screen.width / (float)Screen.height;
            UpdateCameraAspect();
        }
    }

    private void UpdateCameraAspect()
    {
        // Tính toán FOV mới để bức tường 2 bên nằm trong màn hình
        float fov = Mathf.Atan(Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2f) * targetAspect / currentAspect) * 2f * Mathf.Rad2Deg;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        
        // Cập nhật FOV cho camera
        cam.fieldOfView = fov;
    }
}