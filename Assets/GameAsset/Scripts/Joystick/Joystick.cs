using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float initialVelocity = 5f;
    public float duration = 4f;

    #region Hàm không cần quan tâm

    public float Horizontal
    {
        get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; }
    }

    public float Vertical
    {
        get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; }
    }

    public Vector2 Direction
    {
        get { return new Vector2(Horizontal, Vertical); }
    }

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    public AxisOptions AxisOptions
    {
        get { return AxisOptions; }
        set { axisOptions = value; }
    }

    public bool SnapX
    {
        get { return snapX; }
        set { snapX = value; }
    }

    public bool SnapY
    {
        get { return snapY; }
        set { snapY = value; }
    }

    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;
    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    private Vector2 input = Vector2.zero;

    protected virtual void Start()
    {
        HandleRange = handleRange;
        DeadZone = deadZone;
        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && GameController.Instance.isCheckDrag)
        {
            #region Đưa mũi tên ngay lên đầu
            // Tính toán vị trí ban đầu của hình ảnh
            Vector3 screenPos = Camera.main.WorldToScreenPoint(GameController.Instance.Weapon.transform.position);
            Vector3 initialPosition = screenPos + Vector3.up * Config.Instance.distance;
            GameController.Instance.Arrow.transform.position = initialPosition;

            #endregion

            #region Xoay mũi tên khi vừa mới nhấn

            GameController.Instance.Arrow.transform.rotation = Quaternion.Euler(0, 0, 90);

            #endregion
        }
        else if(Input.GetMouseButton(0) && !GameController.Instance.isCheckDrag)
        {
            // Tính toán vị trí mới của hình ảnh
            Vector3 screenPos = Camera.main.WorldToScreenPoint(GameController.Instance.Weapon.transform.position);
            float angle = Mathf.Atan2(input.y, input.x);
            float x = screenPos.x + Config.Instance.distance * Mathf.Cos(angle);
            float y = screenPos.y + Config.Instance.distance * Mathf.Sin(angle);
            Vector3 newPosition = new Vector3(x, y, 0f);
            GameController.Instance.Arrow.transform.position = newPosition;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        GameController.Instance.isCheckDrag = true;
    }

    #endregion

    //điều chỉnh joystick
    //Khi nhấn
    public void OnDrag(PointerEventData eventData)
    {
        GameController.Instance.isCheckDrag = false;
        GameController.Instance.currentRotateSpeed = 0;
        GameController.Instance.Weapon_Rigidbody.angularVelocity = Vector3.zero;
        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;
        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        input = new Vector2(input.x, input.y );
        FormatInput();
        HandleInput(input.magnitude, input.normalized, radius, cam);
        handle.anchoredPosition = input * radius * handleRange;


        #region Code hiển thị mũi tên

        if (!GameController.Instance.Arrow.activeSelf)
        {
            GameController.Instance.Arrow.SetActive(true);
        }

        #region Scale mũi tên

        float newScale = Mathf.Lerp(45f, 325, (Mathf.Abs(input.x) + Mathf.Abs(input.y)) / Config.Instance.deadzone);
        Vector2 newSizeDelta = GameController.Instance.Arrow.GetComponent<RectTransform>().sizeDelta;
        newSizeDelta.x = newScale;
        GameController.Instance.Arrow.GetComponent<RectTransform>().sizeDelta = newSizeDelta;
        #endregion

        #region Xoay hướng mũi tên

        // Tính toán hướng xoay của mũi tên dựa trên giá trị input từ joystick
        Vector3 lookDirection = new Vector3(input.x, input.y, 0);
        float angleArrow = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        // Cập nhật xoay trục z của mũi tên
        GameController.Instance.Arrow.transform.rotation = Quaternion.Euler(0, 0, angleArrow);

        #endregion

        #endregion
    }

    #region Các hàm không cần quan tâm

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void FormatInput()
    {
        if (axisOptions == AxisOptions.Horizontal)
        {
            input = new Vector2(input.x, 0f);
            Config.Instance.forceMagnitude = input.x;
        }
        else if (axisOptions == AxisOptions.Vertical)
        {
            input = new Vector2(0f, input.y);
            Config.Instance.forceMagnitude = input.y;
        }

        //Debug.Log(input);
    }

    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
            return value;

        if (axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }

            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }

        return 0;
    }

    #endregion

    //Buông tay
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        #region Cho velocity về 0 để khi bắn không xảy ra hiện tượng còn lực và sai hướng

        GameController.Instance.Weapon_Rigidbody.velocity = Vector3.zero;
        GameController.Instance.Player_Position.GetComponent<Rigidbody>().velocity = Vector3.zero;

        #endregion

        if (!GameController.Instance.isCheckDrag)
        {
            #region Chỉnh góc , hướng của weapon khi nhấn

            Vector3 direction = Vector3.up * GameController.Instance.variableJoystick.Vertical +
                                Vector3.right * GameController.Instance.variableJoystick.Horizontal;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            GameController.Instance.Weapon_Rigidbody.rotation = lookRotation;
        
            
            
            
            GameController.Instance.currentRotateSpeed = GameController.Instance.rotateSpeed;

            GameController.Instance.roundedForce = Mathf.RoundToInt(direction.magnitude * 3);
            GameController.Instance.DirectionOfJoystick = direction;


            #endregion

            #region Bắn weapon

            Config.Instance.forceMagnitudeLeft = Config.Instance.forceMagnitude;
            GameController.Instance.Weapon_Rigidbody.useGravity = false;
            GameController.Instance.Weapon_Rigidbody.velocity =
                GameController.Instance.DirectionOfJoystick.normalized *
                (GameController.Instance.roundedForce < 1 ? 2:GameController.Instance.roundedForce) *
                Config.Instance.forceMagnitude;
            GameController.Instance.Weapon_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            GameController.Instance.Weapon_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;


            #endregion
            #region Chỉnh cho drag weapon bằng 0 khi bay

            GameController.Instance.Weapon_Rigidbody.drag = 0;

            #endregion
            
            #region Tác dụng 1 lực lên Player và Weapon

            /*GameController.Instance.Player_Position.GetComponentsInChildren<Rigidbody>().velocity = -direction.normalized *
                GameController.Instance.roundedForce * Config.Instance.forceInPlayer*1000;*/
            
            GameController.Instance.Player_Position.GetComponentsInChildren<Rigidbody>().ToList().ForEach(item =>
            {
                item.AddForce(-direction.normalized*15*GameController.Instance.roundedForce,ForceMode.Impulse);
            });
            #endregion
        }

        #region Xoay đầu lên trên cho thanh gươm

        if (GameController.Instance.isCheckDrag)
        {
            //GameController.Instance.isCheckDragJoystick = false;
            GameController.Instance.Weapon.transform.rotation = Quaternion.Euler(-90, -90, 0);
            //Debug.Log(123);

        }

        #endregion

        #region Reset mũi tên về 0

        handle.anchoredPosition = Vector2.zero;
        input = Vector2.zero;

        #endregion
    }

//hiện thị vòng nhấn lên màn hình
    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }

        return Vector2.zero;
    }
}

public enum AxisOptions
{
    Both,
    Horizontal,
    Vertical
}