using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Config : MonoBehaviour
{
    public static Config Instance { get; private set; }
    #region Khi nhấn hiển thị mũi tên
    [HeaderTextColor(0.3f,0.9f,0.7f,headerText = "Khi nhấn hiển thị mũi tên")]
        [SerializeField] public float distance = 2f;
        public float deadzone = 5;
    #endregion
    
    #region Lực bắn
    [HeaderTextColor(0.3f,0.9f,0.7f,headerText = "Lực bắn")]
        public float forceMagnitude;
        [HideInInspector]public float forceMagnitudeLeft;
    #endregion
    
    #region Lực tác dụng lên người dùng
    [HeaderTextColor(0.3f,0.9f,0.7f,headerText = "Lực tác dụng lên người dùng")]
    [Range(0,50)]public float forceInPlayer;
    #endregion
    
    #region Thời gian dừng
    [HeaderTextColor(0.3f,0.9f,0.7f,headerText = "Thời gian làm chậm mọi hoạt động")]
        [Range(0.1f,1f)][SerializeField] public float TimeSlow =.2f;
        [Range(0.01f,0.1f)][SerializeField] public float TimefixedDelta =.02f;
    #endregion
    //danh sách tag khi kiếm va vào , xác nhận va vào đâu của enemy
    [HideInInspector] public List<string> Taglist_DieEnemy = new List<string> { "Die_Other", "Die_Head" };

    #region Tốc độ đưa tay vào lòng ngực

    [Range(0.1f,1f)]public float moveSpeed;
    

    #endregion

    #region Check thanh gươm có trên mặt đất không
    [Space(10)]
    [HeaderTextColor(0.3f,0.9f,0.7f,headerText = "Check thanh gươm có trên mặt đất không")]
    [Range(0.1f,3f)]public float raycastDistance;
    
    #endregion

    #region Tốc độ của Player khi đến Weapon

    [HideInInspector] public int moveSpeedPlayer = 500;
    

    #endregion

    #region Tên Weapon hiện tại đang dùng

    public string NameWeaponCurrent;
    public string NameSkinsCurrent;
    

    #endregion

    #region PlayerPrefs

    public string Prefs_Coin = "Coin_";
    public string Prefs_PercentSkin = "Prefs_PercentSkin_";
    public string Prefs_Music = "Music_";
    public string Prefs_Music2 = "Music_2";
    public string Prefs_LevelName = "Level_";
    [HideInInspector]public string Prefs_WeaponPlayer ="WeaponPlayer";
    [HideInInspector]public string Prefs_SkinPlayer ="SkinPlayer";
    #endregion

    #region Thời gian phóng to thu nhỏ của UI
    [Space]
    [Space]
    [Space]
    [HeaderTextColor(0.3f,0.9f,0.7f,headerText = "Thời gian phóng to thu nhỏ của UI")]
    public float scaleDuration_SmallToBig; // Thời gian phóng to
    public float scaleDuration_BigToSmall; // Thời gian phóng nhỏ
    #endregion

    [Range(1f,5f)]public float DameOfBomb;
    #region Khoảng cách player có thể phi đến được
    [Space(10)]
    [HeaderTextColor(0.3f,0.9f,0.7f,headerText = "Khoảng cách phi của player")]
    public int DistanceSpaceOfPlayer;

    #endregion
    
    #region Thời gian làm chậm
    [Space(10)]
    [HeaderTextColor(0.3f,0.9f,0.6f,headerText = "Thời gian làm chậm của weapon khi chạm vào enemy")]
    public float timeSlowMotion;
    [HideInInspector]public float timeDurationSlowMotion;
    [HideInInspector]public bool isCheckTimeSlowMotion;
    #endregion
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}