using UnityEngine;

public class ObjectVisibilityChecker : MonoBehaviour
{
    private Camera mainCamera;
    private Renderer objectRenderer;

    private void Start()
    {
        // Lấy tham chiếu tới camera chính
        mainCamera = Camera.main;

        // Lấy tham chiếu tới Renderer của đối tượng
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Kiểm tra nếu Renderer và Camera tồn tại
        if (objectRenderer != null && mainCamera != null)
        {
            // Kiểm tra nếu đối tượng lọt vào cửa sổ xem của camera
            bool isVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), objectRenderer.bounds);

            // Kiểm tra isVisible
            if (isVisible)
            {
                Debug.Log("Đối tượng nằm trong tầm nhìn của camera");
            }
            else
            {
                Debug.Log("Đối tượng không nằm trong tầm nhìn của camera");
            }
        }
    }
}