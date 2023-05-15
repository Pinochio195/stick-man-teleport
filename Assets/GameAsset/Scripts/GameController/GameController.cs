using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Lean.Pool;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AsyncOperation = System.ComponentModel.AsyncOperation;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    #region Xoay Weapon khi buông tay

    [HideInInspector] public float rotateSpeed = 450f; // tốc độ xoay
    [HideInInspector] public float decreaseRate = .5f; // tốc độ giảm dần lực

    [HideInInspector] public float currentRotateSpeed; // lực xoay hiện tại
    //public RotationSettings rotationSettings;

    #endregion

    #region Component player

    [HeaderTextColor(0.2f, 1, 1, headerText = "Player")]
    public Animator Player_Animator;

    public GameObject Player;
    public Transform Player_Position;

    #endregion

    #region Component Target

    [HeaderTextColor(0.2f, 1, 1, headerText = "Weapon")]
    public Rigidbody Weapon_Rigidbody;

    public GameObject Weapon;

    #endregion

    #region Value của Joystick

    public VariableJoystick variableJoystick;

    #endregion

    #region Khi nhấn hiển thị mũi tên

    [HeaderTextColor(0.2f, 1, 1, headerText = "Mũi tên hiển thị khi nhấn")] [SerializeField]
    public GameObject Arrow;

    //isCheckDrag check xem có đang drag không để khi nhả ra nếu không drag thì nhả ra cho thanh gươm lên thẳng luôn.
    public bool isCheckDrag;

    #endregion

    #region Cho nhân vật bay lên 1 tý và thanh gươm thẳng lên

    [HeaderTextColor(0.2f, 1, 1, headerText = "Cho nhân vật ở dưới hoặc tường không sát vào quá")] [SerializeField]
    public GameObject leg_Left;

    [SerializeField] public GameObject leg_Right;

    #endregion

    #region Số lượng Enemy trong 1 map

    public List<GameObject> listEnemy;

    #endregion

    #region Hiện thi enemy chết

    [SerializeField] public GameObject GO_EnemyDiedParent;
    [SerializeField] public GameObject GO_imageEnemyDied;
    public List<GameObject> list_GO_imageEnemeDied;

    #endregion

    #region Check thanh gươm ở đâu so với tường để dịch ra

    [SerializeField] public float Radius_CheckGroundOrWall;
    [HideInInspector] public Collider[] colliders_Ground;
    [HideInInspector] public GameObject collider_Ground;

    #endregion

    #region Check weapon có ở dưới mặt đất, cạnh tường hay không

    private bool isCheckWeapon;
    private bool isCheckWeapon_NearWall;

    #endregion

    #region Tắt object ngoài các màn level

    [SerializeField] public List<GameObject> List_TurnOn_InLevelGame;
    [SerializeField] public List<GameObject> List_TurnOff_InLevelGame;
    [SerializeField] public List<GameObject> List_TurnOff_InPLAYGame;
    [SerializeField] public List<GameObject> List_TurnOnInShop;
    [SerializeField] public List<GameObject> List_TurnOffInShop;

    #endregion

    #region Hiển thị các UI khi win game

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Hiển thị các UI khi win game")] [SerializeField]
    private GameObject GO_WinGame;

    [SerializeField] public GameObject GO_Slider_Outfit;
    public Image Slider_Outfit;
    public Text Percent_Outfit;
    public bool isWinGame;
    public Text text_Complete;

    #endregion

    #region Level của người chơi

    public int level;

    #endregion

    #region Thay màu cho enemy khi bắn trúng

    [SerializeField] public List<Material> colorEnemyLive;
    [SerializeField] public List<Material> colorEnemyDie;

    #endregion

    #region Kiểm tra weapon chạm vào đâu chưa , rồi thì giảm velocity

    [HideInInspector] public Vector3 DirectionOfJoystick;
    [HideInInspector] public int roundedForce;
    [HideInInspector] public bool isCheckDragJoystick;

    #endregion

    #region Kiểm tra xem thắng hay thua , tránh hiện cả 2 thông báo lên

    [HideInInspector] public bool isWinnerOrLoser;

    #endregion

    #region Đếm số lượng enemy kill được

    [HideInInspector] public int indexEnemy;

    #endregion

    #region Tổng số lượng enemy khi bắt đầu màn chơi

    [HideInInspector] public int countEnemy;

    #endregion

    #region Check xem combo của người chơi

    [HideInInspector] public int CountCombo;
    [HideInInspector] public int CountComboBefore;

    #endregion

    #region Kiểm tra đang ở scene nào

    public bool isCheckSceneARMS;

    #endregion

    #region Effect khi bắn trung enemy

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Effect bắn trung enemy")]
    public GameObject effectPrefabs;

    #endregion

    #region Kiểm tra có chạm vào enemy nào không để giữ nguyên lực của weapon

    [HideInInspector] public bool isCheckTouchEnemyForForce;

    #endregion

    #region Spam Monster

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Thời gian Spam Monster")]
    public float timeDuration;

    public GameObject GO_monster;
    public bool isCheckMonster;
    public Animator animatorMonster;

    #endregion

    #region Game Object Monster

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "GameObject Monster")]
    public GameObject monster;

    #endregion

    #region Check người chơi thua chưa để zoom cam

    public bool isLoser;

    #endregion

    #region Chìa khóa ăn được

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Chìa khóa đã lấy được")]
    public List<GameObject> list_Key;

    public GameObject GO_Key; //ATTENTION:Chưa có nên tạm thời thay đổi màu thôi
    public GameObject GO_Key_Empty;
    public int Key_Count;
    public GameObject GO_ParentBox;
    public GameObject GO_OpenBoxKey;
    public GameObject GO_NoThanks_Key;
    public GameObject GO_ParentKey;

    #endregion

    #region Hiệu ứng chuyển cảnh

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Hiệu ứng chuyển cảnh")] [SerializeField]
    public Animator transitionScene;

    #endregion

    #region Particle,AudioSource khi bom của boss va phải đâu đó

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Particle,AudioSource của boss")] [HideInInspector]
    public GameObject particleObject;

    [HideInInspector] public ParticleSystem particle;
    public GameObject audioSource;
    public GameObject audioSourceBoomBox;
    [SerializeField] public GameObject particlePrefab;

    public MeshRenderer meshRenderer; //chỉ cho con boss ném quả tên lửa
    public SkinnedMeshRenderer skinnedMeshRenderer; //cho mấy quả đầu lâu
    public GameObject effectRenderer; //cho mấy quả đầu lâu
    public Collider _collider;

    #endregion

    #region Cho Scene mới vào load đúng 1 lần thôi

    public bool isCheckLoadScene;

    #endregion

    #region Đổi hướng bắn cho Enemy Fire

    public Transform directionEnemy;

    #endregion

    #region Check enemy cầm khiên , nếu kill thì cho khiên văng đi nhẹ

    public bool isCheckEnemyDefend;

    #endregion

    #region Hiện thi Sinh mạng Boss

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Hiện thi Sinh mạng Boss")] [SerializeField]
    private GameObject Go_LiveBoss;

    public List<GameObject> List_PrefabsBossImageLive;

    #endregion

    #region Danh sách này để test boss

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Danh sách này để test boss")]
    //[SerializeField] private List<GameObject> listSpam;//Nhét bừa các object biểu hiện cho sinh mạng của boss
    public GameObject BOSS;

    [HideInInspector]
    public bool isCheckLiveBoss; //Check nơi sinh ra thứ 2 của BOss , nếu Boss có 4 mạng thì 3 điểm sinh ra 

    [HideInInspector] public bool isCheckLiveBoss2;
    [HideInInspector] public bool isCheckLiveBoss3;
    public bool isCheckSceneHaveBoss;
    public GameObject Prefabs_BossImageLive;
    public GameObject Content_Image_Boss;

    #endregion

    #region Nơi sinh ra của Boss,xử lí logic cho Boss

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Nơi sinh ra của Boss,xử lí logic cho Boss")] [SerializeField]
    private List<GameObject> BOSSGame;

    public Transform Position1;
    public Transform Position2;
    public Transform Position3;
    public bool isCheckGetPositionBoss;

    #endregion

    #region Số màn đang chơi

    public int lastNumber;

    #endregion

    #region Sprite Key để thay

    public Sprite key;
    public Sprite keyBlack;

    #endregion

    #region Enemy khi chạm vào

    [HideInInspector] public GameObject EnemyTouch;

    #endregion

    #region Material khi boss hồi sinh

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Material khi boss hồi sinh")]
    public Material BossSpam;

    public Material Boss_Material;

    #endregion

    #region Nhấp nháy khi boss vừa hồi sinh

    public GameObject GO_BOSS; //lưu boss được sinh ra
    public GameObject trailBOSS;
    public GameObject trails;
    private bool isCheckTrailEnemy;
    public bool isCheckColorSpamBoss;
    public bool isCheckChangeColorSpamBoss;
    public float timeDurationBossSpam = 3f;
    public float timePlusSpam;
    public float timePerBossSpam;
    public float timeBossSpam;
    public bool isCheckTrailStart1;
    public bool isCheckTrailStart2;
    public bool isCheckTrailStart3;

    #endregion

    #region Số lần chọn hộp quà của key

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Số lần chọn hộp quà của key")] [SerializeField]
    private Text text_keyCount;

    #endregion

    #region Đếm tổng tiền

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Đếm tổng tiền")]
    public int MoneyPlayer;

    public Text text_MoneyPlayer;

    #endregion

    #region Thay hộp quà khi click

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Thay hộp quà khi click")] [SerializeField]
    private List<Sprite> list_Sprite_Coin;

    #endregion

    #region List Save

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "List Save")]
    public List<Item> list_WeaponsSave1;

    public List<Item> list_WeaponsSave2;
    public List<Item> list_SkinsSave1;
    public List<Item> list_SkinsSave2;

    #endregion

    #region Effect khi chạm vào coin

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Effect khi chạm vào coin")]
    public GameObject CoinEffect;

    #endregion

    #region List enemy nhấp nháy

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "List enemy nhấp nháy")]
    public List<GameObject> list_EnemyBlink;

    #endregion

    #region Load Scene

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Load Scene")]
    public GameObject LoadingScreeen;

    public Image LoadingBarFill;

    #endregion

    #region Tutorials

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Tutorials")]
    public GameObject Tutorials;

    #endregion

    #region List music boom cần xóa

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "List music boom cần xóa")]
    public List<GameObject> list_musicBoom;

    #endregion

    #region Prefabs Win,Lose Game

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Prefabs Win,Lose Game")]
    public Material PrefabsWin;

    private int IndexGetSkinFor100Percent;
    private int CountCheckTabSkin;
    public Material PrefabsLose;

    #endregion

    #region Make Money

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Make Money")]
    public GameObject GO_MoneyPlayer;

    public GameObject MoneyMove;
    public GameObject CoinSpam;
    List<GameObject> listcoin ;
    #endregion

    #region PopUpSkin
    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "PopUpSkin")]
    public GameObject PopUpSkin;

    #endregion

    #region Lưu các coin đã ăn và không cho hiện lên nữa

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Lưu các coin đã ăn và không cho hiện lên nữa")]
    public List<int> list_Coin;

    #endregion
    
    #region X5 Money

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "X5 Money")]
    public GameObject GO_MoneyPlayer_X5;
    public GameObject MoneyMove_X5;
    List<GameObject> listcoin_X5;
    
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

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("ARMS"))
        {
            if (scene.name.Equals("Level_1"))
            {
                if (!Tutorials.activeSelf)
                {
                    Tutorials.SetActive(true);
                }
            }
            else
            {
                Tutorials.SetActive(false);
            }

            #region

            #region Kiểm tra coin nào đã ăn được thì xóa nếu người chơi restart game
            for (int i = 0; i < list_Coin.Count; i++)
            {
                int coinIndex = list_Coin[i];
                CoinRotate coinManager = ManagerCoin.Instance.listManager.Find(item => item.Index == coinIndex);
                if (coinManager != null)
                {
                    // Xóa phần tử khỏi danh sách ManagerCoin.Instance.listManager
                    ManagerCoin.Instance.listManager.Remove(coinManager);
                    LeanPool.Despawn(coinManager.gameObject);
                }
            }
            #endregion




            #endregion
            
            #region Cho biến bool về false để lại lấy position sinh boss

            isCheckGetPositionBoss = false;

            #endregion

            #region Tắt mũi tên đi

            if (Arrow.activeSelf)
            {
                Arrow.SetActive(false);
            }

            #endregion

            #region Reset lại tiền

            MoneyPlayer = 0;

            #endregion

            #region Reset các biến mặc định

            isCheckColorSpamBoss = false;
            isCheckSceneARMS = false;
            isLoser = false;

            #endregion

            #region đưa combo về 0 để màn mới tính combo tiếp

            CountCombo = 0;
            CountComboBefore = 0;

            #endregion


            if (!scene.name.Equals("TestScene"))
            {
                // Lấy kí tự cuối cùng
                string sceneName = SceneManager.GetActiveScene().name;
                string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
                lastNumber = int.Parse(numberString);

                #region Check boss trong scene bằng tên scene

                if (lastNumber % 5 == 0)
                {
                    isCheckSceneHaveBoss = true;
                    if (!Content_Image_Boss.activeSelf)
                    {
                        Content_Image_Boss.SetActive(true);
                    }

                    int number;
                    if (lastNumber >= 20)
                    {
                        number = 4;
                    }
                    else
                    {
                        number = 3;
                    }

                    for (int i = 0; i < Content_Image_Boss.transform.childCount; i++)
                    {
                        if (Content_Image_Boss.transform.childCount > 1)
                        {
                            LeanPool.Despawn(Content_Image_Boss.transform.GetChild(i).gameObject);
                        }
                    }

                    List_PrefabsBossImageLive.Clear();
                    for (int i = 0; i < number; i++)
                    {
                        GameObject a = LeanPool.Spawn(Prefabs_BossImageLive, Vector3.zero, Quaternion.identity,
                            Content_Image_Boss.transform);
                        List_PrefabsBossImageLive.Add(a);
                    }
                }
                else
                {
                    isCheckSceneHaveBoss = false;
                }

                #endregion

                isCheckDragJoystick = false;
                indexEnemy = 0;
                isWinnerOrLoser = false;

                #region Tắt những thứ không phải của màn chơi

                for (int i = 0; i < List_TurnOn_InLevelGame.Count; i++)
                {
                    if (!List_TurnOn_InLevelGame[i].activeSelf)
                    {
                        List_TurnOn_InLevelGame[i].SetActive(true);
                    }
                }

                for (int i = 0; i < List_TurnOff_InLevelGame.Count; i++)
                {
                    if (List_TurnOff_InLevelGame[i].activeSelf)
                    {
                        List_TurnOff_InLevelGame[i].SetActive(false);
                    }
                }

                #endregion

                #region Các GameObject cần khi loadScene

                // Tìm kiếm các đối tượng trong scene mới
                GameObject playerObject = GameObject.Find("Player");
                GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

                #region Bật các buttons khi sang màn chơi khác

                List_TurnOn_InLevelGame.ForEach(item =>
                    {
                        if (!item.activeSelf)
                        {
                            item.SetActive(true);
                        }
                    }
                );

                #endregion

                #region Gán Weapon trước đó đã chọn hoặc Weapon mặc định (Khi vào trong game)

                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(Config.Instance.Prefs_WeaponPlayer)))
                {
                    Config.Instance.NameWeaponCurrent = PlayerPrefs.GetString(Config.Instance.Prefs_WeaponPlayer);
                }
                else
                {
                    Config.Instance.NameWeaponCurrent = ControllerShop.Instance.List_Weapon[0].name;
                }

                #endregion

                #region Gán Skin trước đó đã chọn hoặc Weapon mặc định (Khi vào trong game)

                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(Config.Instance.Prefs_SkinPlayer)))
                {
                    Config.Instance.NameSkinsCurrent = PlayerPrefs.GetString(Config.Instance.Prefs_SkinPlayer);
                }
                else
                {
                    Config.Instance.NameSkinsCurrent = ControllerShop.Instance.List_SkinsForDisplay[0].name;
                }

                #endregion

                for (int i = 0; i < ControllerShop.Instance.List_Weapon.Count; i++)
                {
                    if (ControllerShop.Instance.List_Weapon[i].name.Equals(Config.Instance.NameWeaponCurrent))
                    {
                        Weapon = LeanPool.Spawn(ControllerShop.Instance.List_Weapon[i], playerObject.transform.position,
                            Quaternion.Euler(90, 90, 0));
                        Weapon_Rigidbody = Weapon.GetComponent<Rigidbody>();
                    }
                }

                if (playerObject != null)
                {
                    Player_Position = playerObject.transform;
                    Player = Player_Position.GetChild(0).gameObject;
                    Player_Animator = Player.GetComponent<Animator>();

                    Player_Position.GetComponent<Rigidbody>().isKinematic = true;
                }

                if (!isCheckSceneHaveBoss)
                {
                    if (enemyObjects != null && enemyObjects.Length > 0)
                    {
                        listEnemy = new List<GameObject>(enemyObjects);
                    }
                }
                else
                {
                    //listEnemy = new List<GameObject>(listSpam);
                    if (lastNumber >= 20)
                    {
                        listEnemy = Enumerable.Repeat(new GameObject(), 4).ToList();
                    }
                    else
                    {
                        listEnemy = Enumerable.Repeat(new GameObject(), 3).ToList();
                    }
                }

                #endregion
            }

            if (!isCheckSceneHaveBoss)
            {
                #region Bật background cho ảnh hiển thị sinh mạng bot

                if (!ControllerShop.Instance.ImageParentEnemy.gameObject.activeSelf)
                {
                    ControllerShop.Instance.ImageParentEnemy.gameObject.SetActive(true);
                }
                if (!ControllerShop.Instance.imageBackgroundEnemy.gameObject.activeSelf)
                {
                    ControllerShop.Instance.imageBackgroundEnemy.gameObject.SetActive(true);
                }

                if (!ControllerShop.Instance.gridLayoutGroupEnemy.gameObject.activeSelf)
                {
                    ControllerShop.Instance.gridLayoutGroupEnemy.gameObject.SetActive(true);
                }

                if (!ControllerShop.Instance.GO_Level.gameObject.activeSelf)
                {
                    ControllerShop.Instance.GO_Level.gameObject.SetActive(true);
                }

                #endregion

                #region Tắt cho ảnh khi ở level có boss

                if (Go_LiveBoss.activeSelf)
                {
                    Go_LiveBoss.SetActive(false);
                }
                if (ControllerShop.Instance.ImageParentBoss.gameObject.activeSelf)
                {
                    ControllerShop.Instance.ImageParentEnemy.gameObject.SetActive(false);
                }
                #endregion

                #region Số lượng ảnh hiển thị enemy còn sống

                for (int i = 0; i < GO_EnemyDiedParent.transform.childCount; i++)
                {
                    if (GO_EnemyDiedParent.transform.childCount > 1)
                    {
                        LeanPool.Despawn(GO_EnemyDiedParent.transform.GetChild(i).gameObject);
                    }
                }

                list_GO_imageEnemeDied.Clear();
                for (int i = 0; i < listEnemy.Count; i++)
                {
                    GameObject A = LeanPool.Spawn(GO_imageEnemyDied, Vector3.zero, Quaternion.identity,
                        GO_EnemyDiedParent.transform);
                    if (A != null)
                    {
                        list_GO_imageEnemeDied.Add(A);
                    }
                }

                #endregion

                #region Gán kích thước ảnh background cho image enemy (kill die)

                float imageWidth =
                    (ControllerShop.Instance.gridLayoutGroupEnemy.cellSize.x +
                     ControllerShop.Instance.gridLayoutGroupEnemy.spacing.x) *
                    ControllerShop.Instance.gridLayoutGroupEnemy.constraintCount -
                    ControllerShop.Instance.gridLayoutGroupEnemy.spacing.x;
                float imageHeight =
                    (ControllerShop.Instance.gridLayoutGroupEnemy.cellSize.y +
                     ControllerShop.Instance.gridLayoutGroupEnemy.spacing.y) *
                    Mathf.CeilToInt((float)ControllerShop.Instance.gridLayoutGroupEnemy.transform.childCount /
                                    ControllerShop.Instance.gridLayoutGroupEnemy.constraintCount) -
                    ControllerShop.Instance.gridLayoutGroupEnemy.spacing.y;
                RectTransform rectTransform = ControllerShop.Instance.imageBackgroundEnemy.rectTransform;
                rectTransform.sizeDelta = new Vector2(imageHeight + 110, imageWidth + 10);

                #endregion
            }
            else
            {
                #region Tắt background cho ảnh hiển thị sinh mạng bot
                if (ControllerShop.Instance.ImageParentEnemy.gameObject.activeSelf)
                {
                    ControllerShop.Instance.ImageParentEnemy.gameObject.SetActive(false);
                }
                
                if (ControllerShop.Instance.imageBackgroundEnemy.gameObject.activeSelf)
                {
                    ControllerShop.Instance.imageBackgroundEnemy.gameObject.SetActive(false);
                }

                if (ControllerShop.Instance.gridLayoutGroupEnemy.gameObject.activeSelf)
                {
                    ControllerShop.Instance.gridLayoutGroupEnemy.gameObject.SetActive(false);
                }

                #endregion

                #region Bật background cho ảnh hiển thị sinh mạng BOSS
                if (!ControllerShop.Instance.ImageParentBoss.gameObject.activeSelf)
                {
                    ControllerShop.Instance.ImageParentBoss.gameObject.SetActive(true);
                }
                if (!ControllerShop.Instance.imageBackgroundBOSS.gameObject.activeSelf)
                {
                    ControllerShop.Instance.imageBackgroundBOSS.gameObject.SetActive(true);
                }

                #endregion

                #region Tắt image hiển thị enemy die đi

                if (GO_EnemyDiedParent.activeSelf)
                {
                    GO_EnemyDiedParent.SetActive(false);
                    GO_EnemyDiedParent.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
                }

                #endregion

                #region Bật cho ảnh khi ở level có boss

                if (!Go_LiveBoss.activeSelf)
                {
                    Go_LiveBoss.SetActive(true);
                }

                #endregion

                #region Gán kích thước ảnh background cho image BOSS (kill die)

                float imageWidth =
                    (ControllerShop.Instance.gridLayoutGroupBOSS.cellSize.x +
                     ControllerShop.Instance.gridLayoutGroupBOSS.spacing.x) *
                    ControllerShop.Instance.gridLayoutGroupBOSS.constraintCount -
                    ControllerShop.Instance.gridLayoutGroupBOSS.spacing.x;
                float imageHeight =
                    (ControllerShop.Instance.gridLayoutGroupBOSS.cellSize.y +
                     ControllerShop.Instance.gridLayoutGroupBOSS.spacing.y) *
                    Mathf.CeilToInt((float)ControllerShop.Instance.gridLayoutGroupBOSS.transform.childCount /
                                    ControllerShop.Instance.gridLayoutGroupBOSS.constraintCount) -
                    ControllerShop.Instance.gridLayoutGroupBOSS.spacing.y;
                RectTransform rectTransform = ControllerShop.Instance.imageBackgroundBOSS.rectTransform;
                rectTransform.sizeDelta = new Vector2(imageHeight + 110, imageWidth + 10);

                #endregion
            }
        }
        else
        {
            isCheckSceneARMS = true;
            List_TurnOnInShop.ForEach(item => item.SetActive(true));
        }
    }

    #region Check Weapon nào sau khi người chơi chọn weapon.

    public void SelectWeapon(int ID_Weapon)
    {
        Config.Instance.NameWeaponCurrent = ControllerShop.Instance.List_Weapon[ID_Weapon].name;
        PlayerPrefs.SetString(Config.Instance.Prefs_WeaponPlayer, Config.Instance.NameWeaponCurrent);
    }

    #endregion

    #region Check Skin nào sau khi người chơi chọn weapon.

    public void SelectSkin(int ID_Weapon)
    {
        Config.Instance.NameSkinsCurrent = ControllerShop.Instance.List_SkinsForDisplay[ID_Weapon].name;
        PlayerPrefs.SetString(Config.Instance.Prefs_SkinPlayer, Config.Instance.NameSkinsCurrent);
    }

    #endregion

    #region Tìm ra phần tử có tên là gì trong 1 gameobject cha trên cùng

    public static GameObject FindChildByName(Transform parent, string childName)
    {
        if (parent.name == childName)
        {
            return parent.gameObject;
        }

        foreach (Transform child in parent)
        {
            GameObject result = FindChildByName(child, childName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    #endregion

    void Update()
    {
        #region Xóa list music mà bom tạo ra

        if (list_musicBoom.Count > 0)
        {
            for (int i = 0; i < list_musicBoom.Count; i++)
            {
                if (list_musicBoom[i] != null)
                {
                    LeanPool.Despawn(list_musicBoom[i], 1);
                }
                else
                {
                    list_musicBoom.Remove(list_musicBoom[i]);
                }
            }
        }

        #endregion

        if (isCheckColorSpamBoss)
        {
            //Debug.Log(123);
            //isCheckColorSpamBoss = false;
            BlinkWhite(GO_BOSS.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>());
        }

        #region MyRegion

        if (isCheckTrailEnemy)
        {
            if (isCheckTrailStart1)
            {
                trails.transform.position =
                    Vector3.Lerp(trails.transform.position, Position1.position, Time.deltaTime * 35f);
                if (Vector3.Distance(trails.transform.position, Position1.position) <= .5f)
                {
                    isCheckTrailEnemy = false;
                    isCheckTrailStart1 = false;
                }
            }
            else if (isCheckTrailStart2)
            {
                trails.transform.position =
                    Vector3.Lerp(trails.transform.position, Position2.position, Time.deltaTime * 35f);
                if (Vector3.Distance(trails.transform.position, Position2.position) <= .5f)
                {
                    isCheckTrailEnemy = false;
                    isCheckTrailStart2 = false;
                }
            }
            else if (isCheckTrailStart3)
            {
                trails.transform.position =
                    Vector3.Lerp(trails.transform.position, Position3.position, Time.deltaTime * 35f);
                if (Vector3.Distance(trails.transform.position, Position3.position) <= .5f)
                {
                    isCheckTrailEnemy = false;
                    isCheckTrailStart3 = false;
                }
            }
        }

        #endregion

        if (isCheckSceneHaveBoss)
        {
            if (isCheckLiveBoss)
            {
                isCheckTrailStart1 = true;
                isCheckTrailEnemy = true;
                string sceneName = SceneManager.GetActiveScene().name;
                string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
                int lastNumber = int.Parse(numberString);
                int indexLevel = lastNumber == 5 ? 0 : (lastNumber - 5) / 5;
                //GO_BOSS = LeanPool.Spawn(BOSSGame[indexLevel], Position1.position, Quaternion.identity);

                #region bật các scripts cho boss

                if (!GO_BOSS.GetComponent<Animator>().enabled)
                {
                    GO_BOSS.GetComponent<Animator>().enabled = true;
                }

                if (GO_BOSS.GetComponent<BotController>() != null)
                {
                    GO_BOSS.GetComponent<BotController>().enabled = true;
                }
                else if (GO_BOSS.GetComponent<Enemy_FireController>() != null)
                {
                    GO_BOSS.GetComponent<Enemy_FireController>().enabled = true;
                }
                else if (GO_BOSS.GetComponent<BossController>() != null)
                {
                    GO_BOSS.GetComponent<BossController>().enabled = true;
                }

                if (GO_BOSS.GetComponent<Bot_Leve10>() != null)
                {
                    GO_BOSS.GetComponent<Bot_Leve10>().enabled = true;
                }

                if (GO_BOSS.GetComponent<BotController_ALL>().life == 0)
                {
                    GO_BOSS.GetComponent<BotController_ALL>().life = 1;
                }

                #endregion

                GO_BOSS.transform.position = Position1.position;


                GO_BOSS.transform.rotation = Quaternion.Euler(0, 180, 0);
                Boss_Material = GO_BOSS.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
                isCheckColorSpamBoss = true;
                isCheckLiveBoss = false;
            }
            else if (isCheckLiveBoss2)
            {
                isCheckTrailStart2 = true;
                isCheckTrailEnemy = true;
                isCheckLiveBoss2 = false;
                string sceneName = SceneManager.GetActiveScene().name;
                string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
                int lastNumber = int.Parse(numberString);
                int indexLevel = lastNumber == 5 ? 0 : (lastNumber - 5) / 5;
                //GO_BOSS =LeanPool.Spawn(BOSSGame[indexLevel], Position2.position, Quaternion.identity);
                GO_BOSS.transform.position = Position2.position;

                GO_BOSS.transform.rotation = Quaternion.Euler(0, 180, 0);

                GO_BOSS.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = Boss_Material;

                #region bật các scripts cho boss

                if (!GO_BOSS.GetComponent<Animator>().enabled)
                {
                    GO_BOSS.GetComponent<Animator>().enabled = true;
                }

                if (GO_BOSS.GetComponent<BotController>() != null)
                {
                    GO_BOSS.GetComponent<BotController>().enabled = true;
                }
                else if (GO_BOSS.GetComponent<Enemy_FireController>() != null)
                {
                    GO_BOSS.GetComponent<Enemy_FireController>().enabled = true;
                }
                else if (GO_BOSS.GetComponent<BossController>() != null)
                {
                    GO_BOSS.GetComponent<BossController>().enabled = true;
                }

                if (GO_BOSS.GetComponent<Bot_Leve10>() != null)
                {
                    GO_BOSS.GetComponent<Bot_Leve10>().enabled = true;
                }

                if (GO_BOSS.GetComponent<BotController_ALL>().life == 0)
                {
                    GO_BOSS.GetComponent<BotController_ALL>().life = 1;
                }

                #endregion

                isCheckColorSpamBoss = true;
            }
            else if (isCheckLiveBoss3)
            {
                isCheckTrailStart3 = true;
                isCheckTrailEnemy = true;
                isCheckLiveBoss3 = false;
                string sceneName = SceneManager.GetActiveScene().name;
                string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
                int lastNumber = int.Parse(numberString);
                int indexLevel = lastNumber == 5 ? 0 : (lastNumber - 5) / 5;
                //GO_BOSS =LeanPool.Spawn(BOSSGame[indexLevel], Position3.position, Quaternion.identity);
                GO_BOSS.transform.position = Position3.position;

                GO_BOSS.transform.rotation = Quaternion.Euler(0, 180, 0);

                GO_BOSS.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = Boss_Material;

                #region bật các scripts cho boss

                if (!GO_BOSS.GetComponent<Animator>().enabled)
                {
                    GO_BOSS.GetComponent<Animator>().enabled = true;
                }

                if (GO_BOSS.GetComponent<BotController>() != null)
                {
                    GO_BOSS.GetComponent<BotController>().enabled = true;
                }
                else if (GO_BOSS.GetComponent<Enemy_FireController>() != null)
                {
                    GO_BOSS.GetComponent<Enemy_FireController>().enabled = true;
                }
                else if (GO_BOSS.GetComponent<BossController>() != null)
                {
                    GO_BOSS.GetComponent<BossController>().enabled = true;
                }

                if (GO_BOSS.GetComponent<Bot_Leve10>() != null)
                {
                    GO_BOSS.GetComponent<Bot_Leve10>().enabled = true;
                }

                if (GO_BOSS.GetComponent<BotController_ALL>().life == 0)
                {
                    GO_BOSS.GetComponent<BotController_ALL>().life = 1;
                }

                #endregion

                isCheckColorSpamBoss = true;
            }
        }


        if (!isCheckSceneARMS && !isWinGame)
        {
            #region Kiểm tra xem có nhấn va UI nào không , nếu không thì return

#if UNITY_EDITOR || UNITY_STANDALONE
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
                if (selectedObj != null)
                {
                    return;
                }
            }
#else
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
                    if (selectedObj != null)
                    {
                        return;
                    }
                }
            }
#endif

            #endregion

            if (Input.GetMouseButtonDown(0))
            {
                #region Tắt Kinematic

                Player_Position.GetComponent<Rigidbody>().isKinematic = false;

                #endregion

                #region Tắt ragdoll

                Player_Animator.enabled = true;
                Player_Animator.Play("New State");
                Player_Animator.SetBool("ChapTay", true);

                #endregion

                #region Check nếu CountComboBefore nhấn nhiều hơn CountCombo 1 hoặc nhiều hơn <=> lúc đó hết combo, cho nó về 0

                if (CountComboBefore > CountCombo)
                {
                    CountCombo = 0;
                    CountComboBefore = 0;
                }

                CountComboBefore++;

                #endregion

                #region kiểm tra khi nhấn thì mất các buttons để chơi game

                List_TurnOff_InPLAYGame.ForEach(item =>
                    {
                        if (item.activeSelf)
                        {
                            item.SetActive(false);
                        }
                    }
                );

                #endregion

                #region Dịch chuyển thanh gươm ra 1 tí để player gắn vô

                // Tạo Raycast phía trên
                RaycastHit hit;
                isCheckWeapon = Physics.Raycast(Weapon.transform.position, Vector3.down, out hit,
                    Config.Instance.raycastDistance);
                if (isCheckWeapon)
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        //Debug.Log(123);
                        // Xử lý khi va chạm với ground
                        float safeDistance = 1.1f; // Khoảng cách an toàn
                        Weapon.transform.position = new Vector3(
                            Weapon.transform.position.x,
                            Weapon.transform.position.y + safeDistance, Weapon.transform.position.z);
                        Weapon.transform.rotation = Quaternion.Euler(-90, -90, 0);
                    }
                }

                // Tạo Raycast phía dưới
                RaycastHit hitUp;
                isCheckWeapon = Physics.Raycast(Weapon.transform.position, Vector3.up, out hitUp,
                    Config.Instance.raycastDistance);
                if (isCheckWeapon)
                {
                    if (hitUp.collider.CompareTag("Ground"))
                    {
                        //Debug.Log(123);
                        // Xử lý khi va chạm với ground
                        float safeDistance = 1.1f; // Khoảng cách an toàn
                        Weapon.transform.position = new Vector3(
                            Weapon.transform.position.x,
                            Weapon.transform.position.y - safeDistance, Weapon.transform.position.z);
                        //Weapon.transform.rotation = Quaternion.Euler(-90, -90, 0);
                    }
                }

                #region Kiểm tra bên trái và bên phải để dịch sang 1 bên

                RaycastHit hit_Wall;
                bool isCheckWeapon_NearWall = Physics.Raycast(Weapon.transform.position, Vector3.right, out hit_Wall,
                                                  Config.Instance.raycastDistance)
                                              || Physics.Raycast(Weapon.transform.position, Vector3.left, out hit_Wall,
                                                  Config.Instance.raycastDistance);
                if (isCheckWeapon_NearWall)
                {
                    if (hit_Wall.collider.CompareTag("WallLeft"))
                    {
                        // Xử lý khi va chạm với ground
                        float safeDistance = .5f; // Khoảng cách an toàn
                        Weapon.transform.position = new Vector3(
                            Weapon.transform.position.x + safeDistance,
                            Weapon.transform.position.y, Weapon.transform.position.z);
                        Weapon.transform.rotation = Quaternion.Euler(-90, -90, 0);
                    }
                    else if (hit_Wall.collider.CompareTag("WallRight"))
                    {
                        // Xử lý khi va chạm với ground
                        float safeDistance = .5f; // Khoảng cách an toàn
                        Weapon.transform.position = new Vector3(
                            Weapon.transform.position.x - safeDistance,
                            Weapon.transform.position.y, Weapon.transform.position.z);
                        Weapon.transform.rotation = Quaternion.Euler(-90, -90, 0);
                    }
                }

                #endregion

                #endregion

                //Player_Position.GetChild(1).gameObject.SetActive(true);
            }

            if (Input.GetMouseButton(0))
            {
                float distance = Vector3.Distance(Weapon.transform.position, Player_Position.transform.position);
                RaycastHit hit;
                isCheckWeapon = Physics.Raycast(Weapon.transform.position, Vector3.down, out hit,
                    Config.Instance.raycastDistance);
                Debug.DrawLine(Weapon.transform.position, new Vector3(Weapon.transform.position.x,
                    Weapon.transform.position.y - Config.Instance.raycastDistance,Weapon.transform.position.z),UnityEngine.Color.red);
                if (isCheckWeapon && distance < .4f)
                {
                    Weapon.transform.position = Player_Position.transform.position;
                }
                else
                {
                    Player_Position.transform.position = Weapon.transform.position;
                }

                #region Làm chậm thời gian khi nhấn

                if (Vector3.Distance(Player_Position.transform.position, Weapon.transform.position) == 0)
                {
                    Time.timeScale = .1f; //Config.Instance.TimeSlow;
                    Time.fixedDeltaTime = Time.timeScale * .02f;
                }

                #endregion
            }

            else if (Input.GetMouseButtonUp(0))
            {
                #region Tắt isKinematic đi nếu có bật

                if (Player_Position.GetComponent<Rigidbody>().isKinematic)
                {
                    Player_Position.GetComponent<Rigidbody>().isKinematic = false;
                }

                #endregion

                //Tắt mũi tên đi
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.timeScale * .02f;
                if (!isCheckDrag)
                {
                    isCheckDragJoystick = true;
                }

                Arrow.SetActive(false);

                #region Bật ragdoll

                //Player_Animator.SetBool("ChapTay",false);
                Player_Animator.enabled = false;

                #endregion

                //Player_Position.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
    }

    public void WinGame()
    {
        Slider_Outfit = GO_Slider_Outfit.GetComponent<Image>();
        if (Slider_Outfit.fillAmount >= .66f)
        {
            Slider_Outfit.fillAmount = 1; //Chỉnh phần trăm cho slider
            Slider_Outfit.fillAmount = 0;
            #region Get value Percent Skin

            PlayerPrefs.SetFloat(Config.Instance.Prefs_PercentSkin,Slider_Outfit.fillAmount);

            #endregion
            #region Set skin chưa mua cho model

            bool isCheck = false;
            for (int i = 0; i < ControllerShop.Instance.List_SkinTab1.Count; i++)
            {
                if (!ControllerShop.Instance.List_SkinTab1[i].isStatusPay)
                {
                    PrefabsWin.mainTexture = ControllerShop.Instance.List_SkinsForDisplay[i].texture;
                    IndexGetSkinFor100Percent = i;
                    CountCheckTabSkin = 1;
                    isCheck = true;
                    Debug.Log(IndexGetSkinFor100Percent);
                    break;
                }
                Debug.Log(1);
            }

            for (int i = 0; i < ControllerShop.Instance.List_SkinTab2.Count; i++)
            {
                if (!ControllerShop.Instance.List_SkinTab2[i].isStatusPay && !isCheck)
                {
                    PrefabsWin.mainTexture = ControllerShop.Instance.List_SkinsForDisplay[i + 9].texture;
                    IndexGetSkinFor100Percent = i;
                    CountCheckTabSkin = 2;
                Debug.Log(3);
                    break;
                }
            }

            #endregion
            PopUpSkin.SetActive(true);
            #region Tắt hiển thị sinh mạng của boss

            if (ControllerShop.Instance.imageBackgroundBOSS.transform.parent.gameObject.activeSelf)
            {
                ControllerShop.Instance.imageBackgroundBOSS.transform.parent.gameObject.SetActive(false);
            }

            #endregion

            #region Tắt level trong game đi

            if (ControllerShop.Instance.GO_Level.gameObject.activeSelf)
            {
                ControllerShop.Instance.GO_Level.gameObject.SetActive(false);
            }

            if (ControllerShop.Instance.GO_ImageEnemy.activeSelf)
            {
                ControllerShop.Instance.GO_ImageEnemy.SetActive(false);
            }

            #endregion

            #region Tắt icon key đi

            if (GO_ParentKey.activeSelf)
            {
                GO_ParentKey.SetActive(false);
            }

            #endregion

            #region không cho người chơi thao tác khi win game

            isWinGame = true;

            #endregion
        }
        else
        {
            StartCoroutine(DelayWinGame());
            
        }
    }

    public void WinGameForPopUpSkin()
    {
     
        #region Reset background cho scene sau

        isCheckLoadScene = true;

        #endregion

        #region Cho biến bool về false để lại lấy position sinh boss

        isCheckGetPositionBoss = false;

        #endregion

        if (GO_ParentBox.activeSelf)
        {
            GO_ParentBox.SetActive(false);
        }
        level += 1;
        PlayerPrefs.SetInt(Config.Instance.Prefs_LevelName,
            level);
        Debug.Log(level);
        if (Go_LiveBoss.activeSelf)
        {
            Go_LiveBoss.SetActive(false);
        }

        if (GO_NoThanks_Key.activeSelf)
        {
            GO_NoThanks_Key.SetActive(false);
        }

        //SceneManager.LoadScene("Level_" + level);
        transitionScene.SetTrigger("Fade_Out"); //Chạy animation Fade_Out trước khi load scene

        if (isCheckSceneHaveBoss)
        {
            // Lấy kí tự cuối cùng
            string sceneName = SceneManager.GetActiveScene().name;
            string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
            int lastNumber = int.Parse(numberString);

            #region Check boss trong scene bằng tên scene

            if (lastNumber >= 20)
            {
                for (int a = 0; a < 4; a++)
                {
                    LeanPool.Despawn(List_PrefabsBossImageLive[0]);
                    List_PrefabsBossImageLive.RemoveAt(0);
                }
            }
            else
            {
                for (int a = 0; a < 3; a++)
                {
                    LeanPool.Despawn(List_PrefabsBossImageLive[0]);
                    List_PrefabsBossImageLive.RemoveAt(0);
                }
            }

            #endregion

           
        }
    }
    
    IEnumerator DelayWinGame()
    {
        yield return new WaitForSeconds(2f);
        if (!GO_WinGame.activeSelf)
        {
            #region Tắt hiển thị sinh mạng của boss

            if (ControllerShop.Instance.imageBackgroundBOSS.transform.parent.gameObject.activeSelf)
            {
                ControllerShop.Instance.imageBackgroundBOSS.transform.parent.gameObject.SetActive(false);
            }

            #endregion

            #region Tắt level trong game đi

            if (ControllerShop.Instance.GO_Level.gameObject.activeSelf)
            {
                ControllerShop.Instance.GO_Level.gameObject.SetActive(false);
            }

            if (ControllerShop.Instance.GO_ImageEnemy.activeSelf)
            {
                ControllerShop.Instance.GO_ImageEnemy.SetActive(false);
            }

            #endregion

            #region Tắt icon key đi

            if (GO_ParentKey.activeSelf)
            {
                GO_ParentKey.SetActive(false);
            }

            #endregion

            #region không cho người chơi thao tác khi win game

            isWinGame = true;

            #endregion

            Arrow.SetActive(false);
            GO_WinGame.SetActive(true); //bật pop up lên
            Slider_Outfit = GO_Slider_Outfit.GetComponent<Image>();
            Slider_Outfit.fillAmount =
                Slider_Outfit.fillAmount >= 1 ? 0.33f : Slider_Outfit.fillAmount += 0.33f; //Chỉnh phần trăm cho slider
            PlayerPrefs.SetFloat(Config.Instance.Prefs_PercentSkin,Slider_Outfit.fillAmount);
            #region Hiển thị level và số lượng enemy đã tiêu diệt

            float percent = Slider_Outfit.fillAmount * 100;
            text_Complete.text = $"LEVEL {(level)} COMPLETE";
            level += 1;
            Percent_Outfit.text = $"{percent} %";
            ControllerShop.Instance.text_CoinPlus.text = $"+ {5 * indexEnemy}";

            #endregion


            #region Gán text coin plus

            text_MoneyPlayer.text = !isCheckSceneHaveBoss ? $"+ {5 * indexEnemy + MoneyPlayer}" : $"+ {15}";
            

            #endregion
            
            
        }
    }

    IEnumerator DelaySlider()
    {
        yield return new WaitForSeconds(.5f);
        Slider_Outfit = GO_Slider_Outfit.GetComponent<Image>();
        Slider_Outfit.fillAmount =
            Slider_Outfit.fillAmount >= 1 ? 0.2f : Slider_Outfit.fillAmount += 0.2f*Time.deltaTime; //Chỉnh phần trăm cho slider
    }
    public void btn_NEXT_Game()
    {
        list_Coin.Clear();
        listcoin = new List<GameObject>();
        for (int i = 0; i < 5 * indexEnemy; i++)
        {
            listcoin.Add(LeanPool.Spawn(CoinSpam, MoneyMove.transform.position, Quaternion.identity,
                MoneyMove.transform));
        }
        
        float delayMove = 0.1f; // Khoảng thời gian delay giữa các đồng coin
        float delayScale = 0.02f; // Khoảng thời gian delay giữa các đồng coin
        listcoin.ForEach(item => item.transform.localScale = Vector3.zero);
        for (int i = 0; i < listcoin.Count; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle.normalized * 50;
            listcoin[i].transform.localPosition = randomPosition;
        }

        listcoin.ForEach(item=>item.transform.DOScale(Vector3.one, .3f).OnComplete(() =>
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < listcoin.Count; i++)
            {
                listcoin[i].transform.DOKill();
                sequence.Join(listcoin[i].transform.DOMove(GO_MoneyPlayer.transform.position, (listcoin.Count > 7 ? .25f:.45f)).SetDelay(delayMove ).OnComplete(()=>{
                    #region Get value coin

                    int coinvalue = int.Parse(ControllerShop.Instance.text_Coin.text);
                    coinvalue += 1;
                    Debug.Log(text_MoneyPlayer.text);
                    ControllerShop.Instance.text_Coin.text = coinvalue.ToString();
                    PlayerPrefs.SetInt(Config.Instance.Prefs_Coin, coinvalue);

                    #endregion
                }));
            }

            sequence.OnComplete(() =>
            {
                for (int i = 0; i < listcoin.Count; i++)
                {
                    LeanPool.Despawn(listcoin[i]);

                    #region Reset background cho scene sau

                    isCheckLoadScene = true;

                    #endregion

                    #region Cho biến bool về false để lại lấy position sinh boss

                    isCheckGetPositionBoss = false;

                    #endregion

                    #region Kiểm tra nếu key đã lấy được là 3 thì reset về

                    if (Key_Count == 3)
                    {
                        GO_WinGame.SetActive(false);
                        GO_ParentBox.SetActive(true);
                        if (GO_NoThanks_Key.activeSelf)
                        {
                            GO_NoThanks_Key.SetActive(false);
                        }
                    }
                    else
                    {
                        if (GO_ParentBox.activeSelf)
                        {
                            GO_ParentBox.SetActive(false);
                        }

                        PlayerPrefs.SetInt(Config.Instance.Prefs_LevelName,
                            level);
                        if (Go_LiveBoss.activeSelf)
                        {
                            Go_LiveBoss.SetActive(false);
                        }

                        if (GO_NoThanks_Key.activeSelf)
                        {
                            GO_NoThanks_Key.SetActive(false);
                        }

                        //SceneManager.LoadScene("Level_" + level);
                        transitionScene.SetTrigger("Fade_Out"); //Chạy animation Fade_Out trước khi load scene
                    }

                    #endregion

                    #region Cho hình ảnh biểu hiện mạng sống của Boss về 0 , xóa hết hình ảnh.

                    #region Sinh hình ảnh biểu hiện live của Boss

                    if (isCheckSceneHaveBoss)
                    {
                        // Lấy kí tự cuối cùng
                        string sceneName = SceneManager.GetActiveScene().name;
                        string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
                        int lastNumber = int.Parse(numberString);

                        #region Check boss trong scene bằng tên scene

                        if (lastNumber >= 20)
                        {
                            for (int a = 0; a < 4; a++)
                            {
                                LeanPool.Despawn(List_PrefabsBossImageLive[0]);
                                List_PrefabsBossImageLive.RemoveAt(0);
                            }
                        }
                        else
                        {
                            for (int a = 0; a < 3; a++)
                            {
                                LeanPool.Despawn(List_PrefabsBossImageLive[0]);
                                List_PrefabsBossImageLive.RemoveAt(0);
                            }
                        }

                        #endregion

                        #endregion

                        #endregion
                    }
                }
            });
        }));
  
    }
    public void btn_X5Coin()
    {
        list_Coin.Clear();
        listcoin_X5 = new List<GameObject>();
        for (int i = 0; i < 5 * indexEnemy; i++)
        {
            listcoin_X5.Add(LeanPool.Spawn(CoinSpam, MoneyMove_X5.transform.position, Quaternion.identity,
                MoneyMove_X5.transform));
        }
        
        float delayMove = 0.1f; // Khoảng thời gian delay giữa các đồng coin
        float delayScale = 0.02f; // Khoảng thời gian delay giữa các đồng coin
        listcoin_X5.ForEach(item => item.transform.localScale = Vector3.zero);
        for (int i = 0; i < listcoin_X5.Count; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle.normalized * 50;
            listcoin_X5[i].transform.localPosition = randomPosition;
        }

        listcoin_X5.ForEach(item=>item.transform.DOScale(Vector3.one, .3f).OnComplete(() =>
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < listcoin_X5.Count; i++)
            {
                listcoin_X5[i].transform.DOKill();
                sequence.Join(listcoin_X5[i].transform.DOMove(GO_MoneyPlayer_X5.transform.position, (listcoin_X5.Count > 7 ? .25f:.45f)).SetDelay(delayMove ).OnComplete(()=>{
                    #region Get value coin

                    int coinvalue = int.Parse(ControllerShop.Instance.text_Coin.text);
                    coinvalue += 5;
                    Debug.Log(text_MoneyPlayer.text);
                    ControllerShop.Instance.text_Coin.text = coinvalue.ToString();
                    PlayerPrefs.SetInt(Config.Instance.Prefs_Coin, coinvalue);

                    #endregion
                }));
            }

            sequence.OnComplete(() =>
            {
                for (int i = 0; i < listcoin_X5.Count; i++)
                {
                    LeanPool.Despawn(listcoin_X5[i]);

                    #region Reset background cho scene sau

                    isCheckLoadScene = true;

                    #endregion

                    #region Cho biến bool về false để lại lấy position sinh boss

                    isCheckGetPositionBoss = false;

                    #endregion

                    #region Kiểm tra nếu key đã lấy được là 3 thì reset về

                    if (Key_Count == 3)
                    {
                        GO_WinGame.SetActive(false);
                        GO_ParentBox.SetActive(true);
                        if (GO_NoThanks_Key.activeSelf)
                        {
                            GO_NoThanks_Key.SetActive(false);
                        }
                    }
                    else
                    {
                        if (GO_ParentBox.activeSelf)
                        {
                            GO_ParentBox.SetActive(false);
                        }

                        PlayerPrefs.SetInt(Config.Instance.Prefs_LevelName,
                            level);
                        if (Go_LiveBoss.activeSelf)
                        {
                            Go_LiveBoss.SetActive(false);
                        }

                        if (GO_NoThanks_Key.activeSelf)
                        {
                            GO_NoThanks_Key.SetActive(false);
                        }

                        //SceneManager.LoadScene("Level_" + level);
                        transitionScene.SetTrigger("Fade_Out"); //Chạy animation Fade_Out trước khi load scene
                    }

                    #endregion

                    #region Cho hình ảnh biểu hiện mạng sống của Boss về 0 , xóa hết hình ảnh.

                    #region Sinh hình ảnh biểu hiện live của Boss

                    if (isCheckSceneHaveBoss)
                    {
                        // Lấy kí tự cuối cùng
                        string sceneName = SceneManager.GetActiveScene().name;
                        string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
                        int lastNumber = int.Parse(numberString);

                        #region Check boss trong scene bằng tên scene

                        if (lastNumber >= 20)
                        {
                            for (int a = 0; a < 4; a++)
                            {
                                LeanPool.Despawn(List_PrefabsBossImageLive[0]);
                                List_PrefabsBossImageLive.RemoveAt(0);
                            }
                        }
                        else
                        {
                            for (int a = 0; a < 3; a++)
                            {
                                LeanPool.Despawn(List_PrefabsBossImageLive[0]);
                                List_PrefabsBossImageLive.RemoveAt(0);
                            }
                        }

                        #endregion
                    }

                    #endregion

                    #endregion
                }
            });
        }));
  
    }

    public void GetNewFit()
    {
        //PrefabsWin.mainTexture
        //Debug.Log(CountCheckTabSkin);
        if (CountCheckTabSkin == 1)
        {
            for (int i = 0; i < ControllerShop.Instance.List_SkinTab1.Count; i++)
            {
                if (i == CountCheckTabSkin)
                {
                    Debug.Log(i);
                    #region Click vào item skin
                    ControllerShop.Instance.List_SkinTab1[i].isStatusPay = true;

                    #endregion
                    /*#region Thay thế skins khi click

                    materialPlayer.mainTexture = List_SkinsForDisplay[item.Item_Index].texture;

                    #endregion*/
                    /*#region Tìm kiếm skin nào phù hợp với số id trên item

                    SelectSkin(CountCheckTabSkin);
                    Config.Instance.NameSkinsCurrent = ControllerShop.Instance.List_SkinsForDisplay[CountCheckTabSkin].name;

                    #endregion*/
                    SaveManager.SaveSkin1(ControllerShop.Instance.List_SkinTab1);
                    SaveManager.SaveSkin2(ControllerShop.Instance.List_SkinTab2);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < ControllerShop.Instance.List_SkinTab2.Count; i++)
            {
                if (i == CountCheckTabSkin)
                {
                    #region Click vào item skin
                    ControllerShop.Instance.List_SkinTab1[i].isStatusPay = true;

                    #endregion
                    /*#region Thay thế skins khi click

                    materialPlayer.mainTexture = List_SkinsForDisplay[item.Item_Index].texture;

                    #endregion*/
                    #region Tìm kiếm skin nào phù hợp với số id trên item

                    /*SelectSkin(CountCheckTabSkin);
                    Config.Instance.NameSkinsCurrent = ControllerShop.Instance.List_SkinsForDisplay[CountCheckTabSkin].name;
                    */

                    #endregion
                    SaveManager.SaveSkin1(ControllerShop.Instance.List_SkinTab1);
                    SaveManager.SaveSkin2(ControllerShop.Instance.List_SkinTab2);
                }
            }
        }
    }
    void MakeMoney()
    {
    }

    public void ArmsScene()
    {
        StartCoroutine(LoadArmsScene());
    }


    public void SkinsScene()
    {
        StartCoroutine(LoadSkinsScene());
    }

    private IEnumerator LoadArmsScene()
    {
        yield return SceneManager.LoadSceneAsync("ARMS");

        #region Tắt những thành phần trong scene chơi game

        for (int i = 0; i < List_TurnOnInShop.Count; i++)
        {
            List_TurnOnInShop[i].SetActive(true);
        }

        for (int i = 0; i < List_TurnOffInShop.Count; i++)
        {
            List_TurnOffInShop[i].SetActive(false);
        }

        #endregion

        ControllerShop.Instance.MenuShop_Weapon.SetActive(true);
        ControllerShop.Instance.MenuShop_Skin.SetActive(false);
    }


    private IEnumerator LoadSkinsScene()
    {
        yield return SceneManager.LoadSceneAsync("ARMS");

        #region Tắt những thành phần trong scene chơi game

        for (int i = 0; i < List_TurnOnInShop.Count; i++)
        {
            List_TurnOnInShop[i].SetActive(true);
        }

        for (int i = 0; i < List_TurnOffInShop.Count; i++)
        {
            List_TurnOffInShop[i].SetActive(false);
        }

        #endregion

        ControllerShop.Instance.MenuShop_Weapon.SetActive(false);
        ControllerShop.Instance.MenuShop_Skin.SetActive(true);
    }

    public void ClickItem()
    {
        #region Click vào item trên màn hình

        // Tạo một đối tượng PointerEventData với EventSystem hiện tại
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        // Gán tọa độ của chuột là tọa độ trên màn hình của button
        pointerData.position = Input.mousePosition;

        // Tạo một RaycastResult để lưu kết quả phát hiện va chạm của Raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Thực hiện Raycast để xác định button được click
        EventSystem.current.RaycastAll(pointerData, results);

        #endregion

        // Nếu kết quả có ít nhất 1 phần tử
        if (results.Count > 0)
        {
            // Lấy game object được click
            GameObject clickedObj = results[0].gameObject;
            // Truy cập component Item của game object được click nếu có
            OpenBox item = clickedObj.GetComponent<OpenBox>();
            Debug.Log(item.randomGold);

            #region Đổi màu khi click

            if (Key_Count > 0)
            {
                Key_Count--;
                text_keyCount.text = "x " + Key_Count.ToString();
                int number = Int32.Parse(ControllerShop.Instance.text_Coin.text);
                ControllerShop.Instance.text_Coin.text = (number + item.randomGold).ToString();
                Image changeColor = clickedObj.GetComponent<Image>();
                changeColor.sprite = list_Sprite_Coin[item.randomGold < 500 ? 0 : (item.randomGold < 800 ? 1 : 2)];
                if (item.randomGold >= 800)
                {
                    clickedObj.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 1f);
                }

                changeColor.SetNativeSize();
                if (Key_Count == 0)
                {
                    for (int i = 0; i < list_Key.Count; i++)
                    {
                        if (list_Key[i].GetComponent<Image>().sprite != keyBlack)
                        {
                            list_Key[i].GetComponent<Image>().sprite = keyBlack;
                            Key_Count = 0;
                        }
                    }

                    if (GO_ParentBox.transform.GetChild(0).GetComponent<GraphicRaycaster>().enabled)
                    {
                        GO_ParentBox.transform.GetChild(0).GetComponent<GraphicRaycaster>().enabled = false;
                    }

                    StartCoroutine(DelayNoThanks());
                }
            }
            else
            {
                Key_Count = 0;
            }

            #endregion
        }
    }

    IEnumerator DelayNoThanks()
    {
        yield return new WaitForSeconds(1f);
        if (!GO_NoThanks_Key.activeSelf)
        {
            GO_NoThanks_Key.SetActive(true);
        }
    }

    private void BlinkWhite(SkinnedMeshRenderer meshRenderer)
    {
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Weapon"), LayerMask.NameToLayer("Enemy"), true);
        Weapon.GetComponent<Collider>().isTrigger = false;
        if (timeBossSpam >= timePerBossSpam)
        {
            timePerBossSpam = timeBossSpam + timePlusSpam;
            isCheckChangeColorSpamBoss = !isCheckChangeColorSpamBoss;
            if (isCheckChangeColorSpamBoss)
            {
                meshRenderer.material = BossSpam;
            }
            else
            {
                meshRenderer.material = Boss_Material;
            }
        }
        else
        {
            timeBossSpam += Time.deltaTime;
        }

        if (timeBossSpam >= timeDurationBossSpam)
        {
            isCheckColorSpamBoss = false;
            timeBossSpam = 0;
            timePerBossSpam = 0;
            meshRenderer.material = Boss_Material;
            Weapon.GetComponent<Collider>().isTrigger = true;
            // Ignore collision between Weapon layer and Enemy layer
            //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Weapon"), LayerMask.NameToLayer("Enemy"), false);
        }
    }
}