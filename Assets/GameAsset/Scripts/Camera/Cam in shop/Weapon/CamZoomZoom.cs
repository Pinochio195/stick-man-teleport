using UnityEngine;

public class CamZoomZoom : MonoBehaviour
{
    public Transform target;
    public float distance;
    private float height;
    public float smoothSpeed;
    public float offset = 1f; // khoảng cách lệch sang phải

    #region Chỉnh các thông số cho các tab weapon và skin

    [Header("Weapon")] [Range(0f, 25f)] public float Distance_Weapon = 7; //dành cho tab weapon
    [Range(-10f, 10f)] public float height_Weapon = -1.15f;
    [Header("Skin")] [Range(0f, 25f)] public float Distance_Skin = 10.17f; //dành cho tab skin
    [Range(-10f, 10f)] public float height_Skin = -1.89f;

    #endregion

    [Header("Tốc độ di chuyển cam khi chuyển tab")] [Range(0f, 25f)] [SerializeField]
    float distanceSpeed;

    private Camera cam;
    private bool isAtTarget;
    private bool isCheckStart;

    private void Start()
    {
        cam = GetComponent<Camera>();
        isCheckStart = false;
        CheckTabWeaponOrSkins();
        if (target != null)
        {
            transform.position = target.position - distance * transform.forward + height * Vector3.up +
                                 offset * Vector3.Cross(Vector3.up, transform.forward);
        }
    }

    void CheckTabWeaponOrSkins()
    {
        #region Kiểm tra xem button weapon hay skin bật để hiển thị item của button đó , cho distance và height chạy dần đến giá trị cần thiết

        if (ControllerShop.Instance.MenuShop_Weapon.activeSelf)
        {
            distance = isCheckStart
                ? Mathf.Lerp(distance, Distance_Weapon, distanceSpeed * Time.deltaTime)
                : Distance_Weapon;
            height = isCheckStart ? Mathf.Lerp(height, height_Weapon, distanceSpeed * Time.deltaTime) : height_Weapon;
            isCheckStart = true;
        }
        else if (ControllerShop.Instance.MenuShop_Skin.activeSelf)
        {
            distance = isCheckStart
                ? Mathf.Lerp(distance, Distance_Skin, distanceSpeed * Time.deltaTime)
                : Distance_Skin;
            height = isCheckStart ? Mathf.Lerp(height, height_Skin, distanceSpeed * Time.deltaTime) : height_Skin;
            isCheckStart = true;
        }

        #endregion
    }

    private void LateUpdate()
    {
        #region Check tab được chuyển chưa và cam đã đến vị trí cần đến chưa

        if (ControllerShop.Instance.isCheckTab)
        {
            CheckTabWeaponOrSkins();
            if (ControllerShop.Instance.MenuShop_Skin.activeSelf)
            {
                if (Distance_Skin - Mathf.Round(distance * 100f) / 100 <= 0.01f &&
                    Mathf.Abs(height_Skin - Mathf.Round(height * 100f) / 100) <= 0.01f)
                {
                    ControllerShop.Instance.isCheckTab = false;
                }
            }
            else if (ControllerShop.Instance.MenuShop_Weapon.activeSelf)
            {
                if (Distance_Weapon - Mathf.Abs(Mathf.Round(distance * 100f) / 100)<=0.01f &&
                    Mathf.Abs(height_Weapon - Mathf.Round(height * 100f) / 100) <=0.01f)
                {
                    ControllerShop.Instance.isCheckTab = false;
                }
            }
        }

        #endregion

        #region Cam di chuyển vòng quay player

        if (target != null && !isAtTarget)
        {
            Vector3 targetPos = target.position + height * Vector3.up;
            Vector3 desiredPos = targetPos - distance * transform.forward;
            Vector3 right = Vector3.Cross(Vector3.up, transform.forward); // tính toán vector pháp tuyến sang phải
            Vector3 offsetPos = desiredPos + offset * right; // tính toán vị trí mới dịch chuyển sang phải
            Vector3 smoothedPos = Vector3.Lerp(transform.position, offsetPos, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPos;
            transform.LookAt(targetPos);
            
        }

        #endregion
    }
}