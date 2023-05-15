using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ControllerShop : MonoBehaviour
{
    public static ControllerShop Instance { get; private set; }

    [FormerlySerializedAs("List_ItemParent")] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Các ItemParent")]
    public List<GameObject> List_ItemParent_Weapon;

    public List<GameObject> List_ItemParent_Skin;

    [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Các Header")]
    public List<GameObject> List_Header_Weapon;

    public List<GameObject> List_Header_Skin;

    #region Các item

    #region Weapon

    [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "WEAPON")] [SerializeField]
    public List<Item> List_WeaponTab1;
    public List<Item> List_WeaponTab2;

    #endregion

    #region Skin

    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "SKIN"), SerializeField]
    public List<Item> List_SkinTab1;
    public List<Item> List_SkinTab2;
    #endregion
    
    [Space(30)][HeaderTextColor(0.4f, 1f, 0.4f, headerText = "LIST WEAPON")] [SerializeField]
    public List<GameObject> List_Weapon;

    [FormerlySerializedAs("List_Skins")] [Space(30)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "LIST SKIN")] [SerializeField]
    public List<Sprite> List_SkinsForDisplay;

    #endregion

    #region Cho chạy random

    private List<Item> listRandom;

    private int randomIndex;

    //private float delay = 0.01f;
    private float currentDelay;

    [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Button nhấn để quay weapon")] [SerializeField]
    private Image Button_OpenWeapon;
    [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Button nhấn để quay Skin")] [SerializeField]
    private Image Button_OpenSkin;

    #endregion

    #region Kiểm tra xem đang ở tab weapon hay là skin

    [SerializeField] public GameObject MenuShop_Weapon;
    [SerializeField] public GameObject MenuShop_Skin;
    public bool isCheckTab;

    #endregion

    #region Kiểm tra xem tab header nếu ở tab 2, 3 thì tắt Open và +500 đi
    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Kiểm tra xem tab header nếu ở tab 2, 3 thì tắt Open và +500 đi")]
    [SerializeField] public GameObject GO_Open;
    [SerializeField] public GameObject GO_500;
    [SerializeField] public GameObject GO_Open_Kins;
    [SerializeField] public GameObject GO_500_Skin;

    #endregion

    #region Setting

    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Các cài đặt của game")]
    public GameObject GO_Setting;

    public AudioSource musicGame;
    public AudioSource TouchEnemy;
    [SerializeField] private Slider GO_Music_Slider;
    [SerializeField] private Slider GO_Music_Slider2;
    private float valueMusic;
    private float valueMusic2;
    public bool isCheckVibrate;
    public Image ChangeColorNodeSlider;
    public Image ChangeColorNodeSlider2;

    #endregion

    #region Loser Game

    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Loser Game")] [SerializeField]
    private GameObject GO_LoserGame;

    [SerializeField] private Text TextLoserGame;

    #endregion

    #region Các thông số cần thiết hiển thị lên game

    [SerializeField] public Text LevelGame;

    #endregion

    #region List weapon hiển thị ở shop

    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "List weapon hiển thị ở shop")]
    public List<GameObject> list_WeaponInShop;

    public GameObject Player_SkinInShop;
    private Transform PositionCreateWeaponInShop;
    private Transform PositionCreateSkinInShop;
    private GameObject WeaponInShop;
    private GameObject PlayerGetWeaponInShop; //Player cầm vũ khí trong shop
    private GameObject SkinInShop;

    #endregion

    #region Thay đổi skin cho player

    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Thay đổi skin cho player")] [SerializeField]
    private Material materialPlayer;

    #endregion

    #region Hiện thị headshot,othershot

    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Hiện thị headshot,othershot")]
    public GameObject GO_HeadShot;

    public GameObject GO_OtherShot;

    #endregion

    #region Hiển thị Combo double triper ....

    public List<GameObject> GO_ComboKill;

    #endregion

    #region Coin

    [Space(10)] [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Coin")]
    public Text text_Coin;

    public Text text_CoinPlus;

    #endregion

    #region Combo

    [HideInInspector] public bool isCheckCoroutineComboShot;
    [HideInInspector] public bool isCheckCoroutineOtherShot;
    [HideInInspector] public int randomCombo;

    #endregion

    #region Kiểm tra khi click chọn weapon , nếu weapon đó đã hiển thị thì không cho click nữa

    private int indexClickWeapon;

    #endregion

    #region Level


    [Space(10)]
    [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Level")]
    public Text GO_Level;
    [Space(10)]
    public GameObject GO_ImageEnemy;

    #endregion

    #region Gán chiều dài sau các bức ảnh hiển thị sống chết của enemy,Boss

    [Space(10)]
    [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Gán chiều dài sau các bức ảnh hiển thị sống chết của enemy")]
    public Image imageBackgroundEnemy;
    public GridLayoutGroup gridLayoutGroupEnemy;
    public GameObject ImageParentEnemy;
    [Space(10)]
    [HeaderTextColor(0.4f, 1f, 0.4f, headerText = "Gán chiều dài sau các bức ảnh hiển thị sống chết của BOSS")]
    public Image imageBackgroundBOSS;
    public GridLayoutGroup gridLayoutGroupBOSS;
    public GameObject ImageParentBoss;
    

    #endregion
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            musicGame.Play();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
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
        #region Bật level trong game đi

        if (!GO_Level.gameObject.activeSelf)
        {
            GO_Level.gameObject.SetActive(true);
        }
        if (!GO_ImageEnemy.activeSelf)
        {
            GO_ImageEnemy.SetActive(true);
        }
        #region Tắt image hiển thị enemy die đi

        if (!GameController.Instance.GO_EnemyDiedParent.activeSelf)
        {
            GameController.Instance.GO_EnemyDiedParent.SetActive(true);
        }

        #endregion
        #endregion
        #region Gắn level hiển thị
        GameController.Instance.level = PlayerPrefs.GetInt(Config.Instance.Prefs_LevelName, 1);
        GO_Level.text = ("Level "+(GameController.Instance.level));
        #endregion
        #region Gán giá trị cho các item

            #region Weapon

            GameController.Instance.list_WeaponsSave1 = SaveManager.LoadWeapon1();
            for (int i = 0; i < List_WeaponTab1.Count; i++)
            {
                if (GameController.Instance.list_WeaponsSave1.Count== 0)
                {
                    //Game mới chơi cho mặc định có weapon đầu
                    //i < 1 thì cho trạng thái mua item thành màu sáng
                    List_WeaponTab1[i].isStatusPay = i < 1 ? true : false;
                    List_WeaponTab1[i].Item_Index = i;
                    if (i < List_Weapon.Count)
                    {
                        List_WeaponTab1[i].Item_Name = List_Weapon[i].name;
                        if (List_WeaponTab1[i].Item_Name.Equals(Config.Instance.NameWeaponCurrent))
                        {
                            List_WeaponTab1[i].isEquipped = true;
                        }
                    }
                }
                else
                {
                    //đã có data , lấy nó ra
                    List_WeaponTab1[i].isStatusPay = GameController.Instance.list_WeaponsSave1[i].isStatusPay;
                    List_WeaponTab1[i].Item_Index = GameController.Instance.list_WeaponsSave1[i].Item_Index;
                    List_WeaponTab1[i].isEquipped = GameController.Instance.list_WeaponsSave1[i].isEquipped;
                    List_WeaponTab1[i].Item_Name = GameController.Instance.list_WeaponsSave1[i].Item_Name;
                }
            }

            GameController.Instance.list_WeaponsSave2 = SaveManager.LoadWeapon2();
            for (int i = 0; i < List_WeaponTab2.Count; i++)
            {
                if (GameController.Instance.list_WeaponsSave2.Count== 0)
                {
                    //i < 1 thì cho trạng thái mua item thành màu sáng
                    List_WeaponTab2[i].isStatusPay = false;
                    List_WeaponTab2[i].Item_Index = i+9;
                    if (i < List_WeaponTab2.Count)
                    {
                        List_WeaponTab2[i].Item_Name = List_Weapon[i+9].name;
                        if (List_WeaponTab2[i].Item_Name.Equals(Config.Instance.NameWeaponCurrent))
                        {
                            List_WeaponTab2[i].isEquipped = true;
                        }
                    }
                }
                else
                {
                    //đã có data , lấy nó ra
                    List_WeaponTab2[i].isStatusPay = GameController.Instance.list_WeaponsSave2[i].isStatusPay;
                    List_WeaponTab2[i].Item_Index = GameController.Instance.list_WeaponsSave2[i].Item_Index;
                    List_WeaponTab2[i].isEquipped = GameController.Instance.list_WeaponsSave2[i].isEquipped;
                    List_WeaponTab2[i].Item_Name = GameController.Instance.list_WeaponsSave2[i].Item_Name;
                    List_WeaponTab2[i].gameObject.transform.GetChild(2).gameObject.SetActive(false);
                }
            }

            #endregion

            #region Skin

                GameController.Instance.list_SkinsSave1 = SaveManager.LoadSkin1();
            for (int i = 0; i < List_SkinTab1.Count; i++)
            {
                if (GameController.Instance.list_SkinsSave1.Count == 0)
                {
                    List_SkinTab1[i].isStatusPay = i < 1 ? true : false;
                    List_SkinTab1[0].isEquipped = true;
                    List_SkinTab1[i].Item_Index = i;
                    if (i < List_SkinsForDisplay.Count)
                    {
                        List_SkinTab1[i].Item_Name = List_SkinsForDisplay[i].name;
                        if (List_SkinTab1[i].Item_Name.Equals(Config.Instance.NameSkinsCurrent))
                        {
                            List_SkinTab1[i].isEquipped = true;
                        }
                    }
                }
                else
                {
                    //đã có data , lấy nó ra
                    List_SkinTab1[i].isStatusPay = GameController.Instance.list_SkinsSave1[i].isStatusPay;
                    List_SkinTab1[i].Item_Index = GameController.Instance.list_SkinsSave1[i].Item_Index;
                    List_SkinTab1[i].isEquipped = GameController.Instance.list_SkinsSave1[i].isEquipped;
                    List_SkinTab1[i].Item_Name = GameController.Instance.list_SkinsSave1[i].Item_Name;
                }
            }
            GameController.Instance.list_SkinsSave2 = SaveManager.LoadSkin2();
            for (int i = 0; i < List_SkinTab2.Count ;i++)
            {
                if (GameController.Instance.list_SkinsSave2.Count == 0)
                {
                    //i < 1 thì cho trạng thái mua item thành màu sáng
                    List_SkinTab2[i].isStatusPay = false;
                    List_SkinTab2[i].Item_Index = i + 9;
                    if (i < List_SkinTab2.Count)
                    {
                        List_SkinTab2[i].Item_Name = List_SkinsForDisplay[i + 9].name;
                        if (List_SkinTab2[i].Item_Name.Equals(Config.Instance.NameSkinsCurrent))
                        {
                            List_SkinTab2[i].isEquipped = true;
                        }
                    }
                }
                else
                {
                    //đã có data , lấy nó ra
                    List_SkinTab2[i].isStatusPay = GameController.Instance.list_SkinsSave2[i].isStatusPay;
                    List_SkinTab2[i].Item_Index = GameController.Instance.list_SkinsSave2[i].Item_Index;
                    List_SkinTab2[i].isEquipped = GameController.Instance.list_SkinsSave2[i].isEquipped;
                    List_SkinTab2[i].Item_Name = GameController.Instance.list_SkinsSave2[i].Item_Name;
                    List_SkinTab2[i].gameObject.transform.GetChild(2).gameObject.SetActive(false);
                }

            }

            #endregion

            #region Hiển thị weapon mình đang dùng, chấm xanh trên ảnh
            //Weapon
            for (int i = 0; i < List_WeaponTab1.Count; i++)
            {
                if (List_WeaponTab1[i].isEquipped)
                {
                    //Debug.Log(i);
                    List_WeaponTab1[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    indexClickWeapon = List_WeaponTab1[i].Item_Index;
                }
                else
                {
                    if (List_WeaponTab1[i].isStatusPay)
                    {
                        List_WeaponTab1[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }
                List_WeaponTab1[i].gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }
            for (int i = 0; i < List_WeaponTab2.Count; i++)
            {
                if (List_WeaponTab2[i].isEquipped)
                {
                    List_WeaponTab2[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    indexClickWeapon = List_WeaponTab2[i].Item_Index;
                }
                else
                {
                    if (List_WeaponTab2[i].isStatusPay)
                    {
                        List_WeaponTab2[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }

                }
                List_WeaponTab2[i].gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }
            //SKin
            for (int i = 0; i < List_SkinTab1.Count; i++)
            {
                if (List_SkinTab1[i].isEquipped)
                {
                     List_SkinTab1[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                }
                else
                {
                    if (List_SkinTab1[i].isStatusPay)
                    {
                        List_SkinTab1[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }

                List_SkinTab1[i].gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }
            for (int i = 0; i < List_SkinTab2.Count; i++)
            {
                if (List_SkinTab2[i].isEquipped)
                {
                    List_SkinTab2[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                }
                else
                {
                    if (List_SkinTab2[i].isStatusPay)
                    {
                        List_SkinTab2[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }
                List_SkinTab2[i].gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }
            #endregion

            
        if (SceneManager.GetActiveScene().name.Equals("ARMS"))
        {
            #region Sinh ra weapon để hiển thị lên màn hình

            string nameWeapon = PlayerPrefs.GetString(Config.Instance.Prefs_WeaponPlayer, "Weapon");
            for (int i = 0; i < list_WeaponInShop.Count; i++)
            {
                if (list_WeaponInShop[i].name.Equals(nameWeapon))
                {
                    PositionCreateSkinInShop = GameObject.FindGameObjectWithTag("CreateSkinInShop").transform;
                    PositionCreateWeaponInShop = GameObject.FindGameObjectWithTag("CreateWeaponInShop").transform;
                    PlayerGetWeaponInShop = LeanPool.Spawn(Player_SkinInShop, PositionCreateWeaponInShop.position,
                        Quaternion.Euler(0, 180, 0));
                    GameObject weaponInHand =
                        GameController.FindChildByName(PlayerGetWeaponInShop.transform,
                            "mixamorig:RightHand"); //tìm cánh tay để đưa weapon vào
                    WeaponInShop = LeanPool.Spawn(list_WeaponInShop[i], PositionCreateWeaponInShop.position,
                        Quaternion.Euler(90, 0, 0), weaponInHand.transform);
                    WeaponInShop.transform.localPosition = Vector3.zero;
                    WeaponInShop.transform.localRotation = Quaternion.Euler(0, -90, -90);
                }
            }

            #endregion

            #region Sinh ra skin để hiển thị lên màn hình

            PositionCreateSkinInShop = GameObject.FindGameObjectWithTag("CreateSkinInShop").transform;
            SkinInShop = LeanPool.Spawn(Player_SkinInShop, PositionCreateSkinInShop.position,
                Quaternion.Euler(0, 180, 0));

            #endregion

            #endregion

            #region Gán màu,ảnh cho các weapon chưa được mua 1 màu khác cho item của 3 Header

            for (int i = 0; i < List_WeaponTab1.Count; i++)
            {
                //gắn icon cho các weapon
                //những weapon nào chưa được mua sẽ màu như này:Gray
                if (!List_WeaponTab1[i].isStatusPay)
                {
                    List_WeaponTab1[i].transform.GetChild(1).GetComponent<Image>().color = Color.black;
                }
            }

            for (int i = 0; i < List_WeaponTab2.Count; i++)
            {
                //gắn icon cho các weapon
                //những weapon nào chưa được mua sẽ màu như này:Gray
                if (!List_WeaponTab2[i].isStatusPay)
                {
                    List_WeaponTab2[i].transform.GetChild(1).GetComponent<Image>().color = Color.black;
                    List_WeaponTab2[i].transform.GetChild(2).gameObject.SetActive(true);
                }
            }

            #region Skins

            for (int i = 0; i < List_SkinTab1.Count; i++)
            {
                //gắn icon cho các Skin
                //những Skin nào chưa được mua sẽ màu như này:Gray
                if (!List_SkinTab1[i].isStatusPay)
                {
                    List_SkinTab1[i].transform.GetChild(1).GetComponent<Image>().color = Color.black;
                }
            }
            for (int i = 0; i < List_SkinTab2.Count; i++)
            {
                //gắn icon cho các weapon
                //những weapon nào chưa được mua sẽ màu như này:Gray
                if (!List_SkinTab2[i].isStatusPay)
                {
                    List_SkinTab2[i].transform.GetChild(1).GetComponent<Image>().color = Color.black;
                    List_SkinTab2[i].transform.GetChild(2).gameObject.SetActive(true);
                }
            }
            #endregion

            #endregion

            #region Set mặc định cho 3 button Header

            SetHeaderWeaponDefault();
            SetHeaderSkinDefault();

            #endregion
        }
    }

    private void Start()
    {
        #region Get value cho music khi mơi vào game

        musicGame.volume = PlayerPrefs.GetFloat(Config.Instance.Prefs_Music, 1);
        TouchEnemy.volume = PlayerPrefs.GetFloat(Config.Instance.Prefs_Music2, 1);
        GO_Music_Slider.value = musicGame.volume;
        GO_Music_Slider2.value = TouchEnemy.volume;
        ChangeColorNodeSlider.color = musicGame.volume == 0 ? Color.red : Color.green;
        ChangeColorNodeSlider2.color = TouchEnemy.volume == 0 ? Color.red : Color.green;

        #endregion
        
        #region Get value coin

        text_Coin.text = PlayerPrefs.GetInt(Config.Instance.Prefs_Coin, 99999).ToString();

        #endregion

        #region Get value Percent Skin

        GameController.Instance.GO_Slider_Outfit.GetComponent<Image>().fillAmount=PlayerPrefs.GetFloat(Config.Instance.Prefs_PercentSkin);

        #endregion

        //List_WeaponTab1.ForEach(item=>Debug.Log(item.Item_Index));
    }

    #region Nhấn vào setting

    #region Setting

    public void ClickSetting()
    {
        if (!GO_Setting.activeSelf)
        {
            GO_Setting.SetActive(true);
            GameController.Instance.isWinGame = true;
            if (GameController.Instance.Tutorials.activeSelf)
            {
                GameController.Instance.Tutorials.SetActive(false);
            }
        }
    }

    #endregion

    #region Music

    public void OffMusic()
    {
        valueMusic = GO_Music_Slider.value;
        musicGame.volume = valueMusic;
        PlayerPrefs.SetFloat(Config.Instance.Prefs_Music, musicGame.volume);
        ChangeColorNodeSlider.color = musicGame.volume == 0 ? Color.red : Color.green;
    }

    #endregion

    #region Music2

    public void OffMusic2()
    {
        valueMusic2 = GO_Music_Slider2.value;
        TouchEnemy.volume = valueMusic2;
        PlayerPrefs.SetFloat(Config.Instance.Prefs_Music2, TouchEnemy.volume);
        ChangeColorNodeSlider2.color = TouchEnemy.volume == 0 ? Color.red : Color.green;
    }

    #endregion

    #region Thoát Setting

    public void ExitSetting()
    {
        if (GO_Setting.activeSelf)
        {
            GO_Setting.SetActive(false);
            GameController.Instance.isWinGame = false;
            if (!GameController.Instance.Tutorials.activeSelf && GameController.Instance.level == 1)
            {
                GameController.Instance.Tutorials.SetActive(true);
            }
        }
    }

    #endregion

    #endregion

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
            #region Shopee

            if (GameController.Instance.isCheckSceneARMS)
            {
                // Lấy game object được click
                GameObject clickedObj = results[0].gameObject;
                // Truy cập component Item của game object được click nếu có
                Item item = clickedObj.GetComponent<Item>();
                Image status = clickedObj.transform.GetChild(0).GetComponent<Image>();
                if (!item.isStatusPay)
                {
                    Debug.Log("Chưa có được Weapon hoặc Skins này!!!");
                }
                if (item != null && item.isStatusPay)
                {
                    if (MenuShop_Weapon.activeSelf && List_ItemParent_Weapon[0].activeSelf)
                    {
                        #region Cho tất cả các status của weapon tab 1, 2 về màu đỏ trước

                            foreach (Item listItem in List_WeaponTab1)
                            {
                                if (listItem.isStatusPay)
                                {
                                    listItem.isEquipped = false;
                                    listItem.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                }
                            }

                            foreach (Item listItem in List_WeaponTab2)
                            {
                                if (listItem.isStatusPay)
                                {
                                    listItem.isEquipped = false;
                                    listItem.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                }
                            }
                            
                            #endregion

                            for (int i = 0; i < List_WeaponTab1.Count; i++)
                            {
                                if (item.Item_Index == List_WeaponTab1[i].Item_Index)
                                {
                                    #region Đổi màu status

                                    status.color = Color.green;

                                    #endregion

                                    #region Click vào item weapon

                                    item.isEquipped = true;
                                    Debug.Log(item.name);

                                    #endregion

                                    #region Thay thế weapon khi click

                                    LeanPool.Despawn(WeaponInShop);
                                    GameObject weaponInHand =
                                        GameController.FindChildByName(PlayerGetWeaponInShop.transform,
                                            "mixamorig:RightHand"); //tìm cánh tay để đưa weapon vào
                                    WeaponInShop = LeanPool.Spawn(list_WeaponInShop[item.Item_Index],
                                        PositionCreateWeaponInShop.position,
                                        Quaternion.Euler(90, 0, 0), weaponInHand.transform);
                                    WeaponInShop.transform.localPosition = Vector3.zero;
                                    WeaponInShop.transform.localRotation = Quaternion.Euler(0, -90, -90);
                                    #region Tìm kiếm weapon nào phù hợp với số id trên item

                                    GameController.Instance.SelectWeapon(item.Item_Index);

                                    #endregion
                            

                                    #endregion
                                    SaveManager.SaveWeapon1(List_WeaponTab1);
                                    SaveManager.SaveWeapon2(List_WeaponTab2);
                                    break;
                                }
                            }
                        
                    }
                    else if (MenuShop_Skin.activeSelf && List_ItemParent_Skin[0].activeSelf)
                    {
                        #region Cho status tất cả các list 2 bảng đều màu đỏ

                        foreach (Item listItem in List_SkinTab1)
                        {
                            if (listItem.isStatusPay)
                            {
                                listItem.isEquipped = false;
                                listItem.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                            }
                        }
                        List_SkinTab2.ForEach(itm =>
                        {
                            if (itm.isStatusPay)
                            {
                                itm.isEquipped = false;
                                itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                            }
                        });
                        #endregion
                        
                        for (int i = 0; i < List_SkinTab1.Count; i++)
                        {
                            if (item.Item_Index == List_SkinTab1[i].Item_Index)
                            {
                                #region Click vào item skin
                                item.isEquipped = true;
                                item.transform.GetChild(0).GetComponent<Image>().color = Color.green;

                                #endregion
                                #region Thay thế skins khi click

                                materialPlayer.mainTexture = List_SkinsForDisplay[item.Item_Index].texture;

                                #endregion
                                #region Tìm kiếm skin nào phù hợp với số id trên item

                                GameController.Instance.SelectSkin(item.Item_Index);
                                Config.Instance.NameSkinsCurrent = List_SkinsForDisplay[item.Item_Index].name;

                                #endregion
                                SaveManager.SaveSkin1(List_SkinTab1);
                                SaveManager.SaveSkin2(List_SkinTab2);
                                break;
                            }
                        }
                    }
                }

                if (item != null)
                {
                    if (MenuShop_Weapon.activeSelf&& List_ItemParent_Weapon[1].activeSelf)
                    {
                        if (item.isStatusPay)
                        {
                            #region Thay thế weapon khi click
                            LeanPool.Despawn(WeaponInShop);
                            GameObject weaponInHand =
                                GameController.FindChildByName(PlayerGetWeaponInShop.transform,
                                    "mixamorig:RightHand"); //tìm cánh tay để đưa weapon vào
                            WeaponInShop = LeanPool.Spawn(list_WeaponInShop[item.Item_Index],
                                PositionCreateWeaponInShop.position,
                                Quaternion.Euler(90, 0, 0), weaponInHand.transform);
                            WeaponInShop.transform.localPosition = Vector3.zero;
                            WeaponInShop.transform.localRotation = Quaternion.Euler(0, -90, -90);
                            #region Tìm kiếm weapon nào phù hợp với số id trên item

                            GameController.Instance.SelectWeapon(item.Item_Index);

                            #endregion
                            

                            #endregion

                            #region Đổi màu status

                            status.color = Color.green;

                            #endregion
                            
                            List_WeaponTab2.ForEach(itm =>
                            {
                                if (itm.Item_Index != item.Item_Index && itm.isStatusPay)
                                {
                                    itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                    itm.isEquipped = false;
                                }
                            });
                            List_WeaponTab1.ForEach(itm =>
                            {
                                if (itm.Item_Index != item.Item_Index && itm.isStatusPay)
                                {
                                    itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                    itm.isEquipped = false;
                                }
                            });
                            
                            #region Click vào item weapon hoặc skin

                            item.isEquipped = true;

                            #endregion
                            
                            
                        }

                        if (!item.isStatusPay &&
                            (List_ItemParent_Weapon[1].activeSelf || List_ItemParent_Skin[1].activeSelf))
                        {
                            #region Thay đổi màu status cho ca weapon khác
                            List_WeaponTab2.ForEach(itm =>
                            {
                                if (itm.Item_Index != item.Item_Index && itm.isStatusPay)
                                {
                                    itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                    itm.isEquipped = false;
                                }
                            });
                            List_WeaponTab1.ForEach(itm =>
                            {
                                if (itm.Item_Index != item.Item_Index && itm.isStatusPay)
                                {
                                    itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                    itm.isEquipped = false;
                                }
                            });
                            #endregion
                            item.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                            item.gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
                            item.isStatusPay = true;
                            item.isEquipped = true;
                            item.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                            #region Thay thế weapon khi click

                            LeanPool.Despawn(WeaponInShop);
                            GameObject weaponInHand =
                                GameController.FindChildByName(PlayerGetWeaponInShop.transform,
                                    "mixamorig:RightHand"); //tìm cánh tay để đưa weapon vào
                            WeaponInShop = LeanPool.Spawn(list_WeaponInShop[item.Item_Index],
                                PositionCreateWeaponInShop.position,
                                Quaternion.Euler(90, 0, 0), weaponInHand.transform);
                            WeaponInShop.transform.localPosition = Vector3.zero;
                            WeaponInShop.transform.localRotation = Quaternion.Euler(0, -90, -90);
                            #region Tìm kiếm weapon nào phù hợp với số id trên item

                            GameController.Instance.SelectWeapon(item.Item_Index);

                            #endregion
                            

                            #endregion
                        }
                        SaveManager.SaveWeapon1(List_WeaponTab1);
                        SaveManager.SaveWeapon2(List_WeaponTab2);
                    }
                    if (MenuShop_Skin.activeSelf&& List_ItemParent_Skin[1].activeSelf)
                    {
                        if (item.isStatusPay)
                        {
                            //TODO:Cho xem quảng cáo
                            #region Tìm kiếm weapon nào phù hợp với số id trên item

                            materialPlayer.mainTexture = List_SkinsForDisplay[item.Item_Index].texture;
                            GameController.Instance.SelectSkin(item.Item_Index);
                            Config.Instance.NameSkinsCurrent = List_SkinsForDisplay[item.Item_Index].name;
                            #endregion
                            #region Đổi màu status

                            status.color = Color.green;

                            #endregion

                            

                            List_SkinTab2.ForEach(itm =>
                            {
                                if (itm.Item_Index != item.Item_Index && itm.isStatusPay)
                                {
                                    itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                    itm.isEquipped = false;
                                }
                            });
                            List_SkinTab1.ForEach(itm =>
                            {
                                if (itm.Item_Index != item.Item_Index && itm.isStatusPay)
                                {
                                    itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                    itm.isEquipped = false;
                                }
                            });
                            
                            #region Click vào item weapon hoặc skin

                            item.isEquipped = true;

                            #endregion
                        }

                        if (!item.isStatusPay &&
                            (List_ItemParent_Weapon[1].activeSelf || List_ItemParent_Skin[1].activeSelf))
                        {
                            #region Cho status tất cả các list 2 bảng đều màu đỏ

                            foreach (Item listItem in List_SkinTab1)
                            {
                                if (listItem.isStatusPay)
                                {
                                    listItem.isEquipped = false;
                                    listItem.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                }
                            }
                            List_SkinTab2.ForEach(itm =>
                            {
                                if (itm.isStatusPay)
                                {
                                    itm.isEquipped = false;
                                    itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                                }
                            });
                            #endregion
                            item.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                            item.gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
                            item.isStatusPay = true;
                            item.isEquipped = true;
                            item.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                        }
                        SaveManager.SaveSkin1(List_SkinTab1);
                        SaveManager.SaveSkin2(List_SkinTab2);
                    }
                }
            }
            

            #endregion
        }
    }

    #region Bật tắt các Header của menu

    #region Weapon

    public void ClickHeader_Item1()
    {
        OnOffItemParentAndHeader(0);
        if (!GO_Open.activeSelf && !GO_500.activeSelf)
        {
            GO_Open.SetActive(true);
            GO_500.SetActive(true);
        }
    }

    public void ClickHeader_Item2()
    {
        OnOffItemParentAndHeader(1);
        if (GO_Open.activeSelf && GO_500.activeSelf)
        {
            GO_Open.SetActive(false);
            GO_500.SetActive(false);
        }
    }


    #endregion

    #region Skins

    public void ClickHeader_Item1_Skins()
    {
        OnOffItemParentAndHeader_SKINS(0);
        if (!GO_Open_Kins.activeSelf && !GO_500_Skin.activeSelf)
        {
            GO_Open_Kins.SetActive(true);
            GO_500_Skin.SetActive(true);
        }
    }

    public void ClickHeader_Item2_Skins()
    {
        OnOffItemParentAndHeader_SKINS(1);
        if (GO_Open_Kins.activeSelf && GO_500_Skin.activeSelf)
        {
            GO_Open_Kins.SetActive(false);
            GO_500_Skin.SetActive(false);
        }
    }

    #endregion

    void OnOffItemParentAndHeader(int index)
    {
        for (int i = 0; i < List_Header_Weapon.Count; i++)
        {
            if (i == index)
            {
                List_Header_Weapon[index].GetComponent<Image>().color = new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_Header_Weapon[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;

                RectTransform rectTransform = List_Header_Weapon[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -150;
                rectTransform.anchoredPosition = anchoredPosition;
            }
            else
            {
                List_Header_Weapon[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Weapon[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);
                RectTransform rectTransform = List_Header_Weapon[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -165;
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }

        for (int i = 0; i < List_ItemParent_Weapon.Count; i++)
        {
            if (i == index)
            {
                List_ItemParent_Weapon[i].SetActive(true);
            }
            else
            {
                List_ItemParent_Weapon[i].SetActive(false);
            }
        }
    }

    void OnOffItemParentAndHeader_SKINS(int index)
    {
        for (int i = 0; i < List_Header_Skin.Count; i++)
        {
            if (i == index)
            {
                List_Header_Skin[index].GetComponent<Image>().color = new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_Header_Skin[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                
                RectTransform rectTransform = List_Header_Skin[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -150;
                rectTransform.anchoredPosition = anchoredPosition;
            }
            else
            {
                List_Header_Skin[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Skin[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);
                RectTransform rectTransform = List_Header_Skin[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -165;
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }

        for (int i = 0; i < List_ItemParent_Skin.Count; i++)
        {
            if (i == index)
            {
                List_ItemParent_Skin[i].SetActive(true);
            }
            else
            {
                List_ItemParent_Skin[i].SetActive(false);
            }
        }
    }

    #endregion

    #region Khi vào menu shop , set 3 button header , button đầu mặc định bật

    void SetHeaderWeaponDefault()
    {
        for (int i = 0; i < List_Header_Weapon.Count; i++)
        {
            if (i == 0)
            {
                List_Header_Weapon[i].GetComponent<Image>().color = new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_Header_Weapon[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
            }
            else
            {
                List_Header_Weapon[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Weapon[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);
            }
        }

        for (int i = 0; i < List_ItemParent_Weapon.Count; i++)
        {
            if (i != 0)
            {
                if (List_ItemParent_Weapon[i].activeSelf)
                {
                    List_ItemParent_Weapon[i].SetActive(false);
                }
            }
            else
            {
                if (!List_ItemParent_Weapon[i].activeSelf)
                {
                    List_ItemParent_Weapon[i].SetActive(true);
                }
            }
        }
    }

    void SetHeaderSkinDefault()
    {
        for (int i = 0; i < List_Header_Skin.Count; i++)
        {
            if (i == 0)
            {
                List_Header_Weapon[i].GetComponent<Image>().color = new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_Header_Weapon[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
            }
            else
            {
                List_Header_Skin[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Skin[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);
            }
        }

        for (int i = 0; i < List_ItemParent_Skin.Count; i++)
        {
            if (i != 0)
            {
                if (List_ItemParent_Skin[i].activeSelf)
                {
                    List_ItemParent_Skin[i].SetActive(false);
                }
            }
            else
            {
                if (!List_ItemParent_Skin[i].activeSelf)
                {
                    List_ItemParent_Skin[i].SetActive(true);
                }
            }
        }
    }

    #endregion

    #region Nhấn mũi tên 2 bên để di chuyển button header

    #region Weapon
    public void Click_LeftHeader()
    {
        for (int i = 0; i < List_ItemParent_Weapon.Count; i++)
        {
            if (List_ItemParent_Weapon[i].activeSelf)
            {
                if ((i == 0 ? List_Header_Weapon.Count - 1 : i - 1) == 0 && !GO_Open.activeSelf && !GO_500.activeSelf)
                {
                    GO_Open.SetActive(true);
                    GO_500.SetActive(true);
                }
                else if ((i == 0 ? List_Header_Weapon.Count - 1 : i - 1) != 0 && GO_Open.activeSelf &&
                         GO_500.activeSelf)
                {
                    GO_Open.SetActive(false);
                    GO_500.SetActive(false);
                }

                List_Header_Weapon[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Weapon[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);

                List_Header_Weapon[i == 0 ? List_Header_Weapon.Count - 1 : i - 1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                List_Header_Weapon[i == 0 ? List_Header_Weapon.Count - 1 : i - 1].GetComponent<Image>().color =
                    new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_ItemParent_Weapon[i].SetActive(false);
                List_ItemParent_Weapon[i == 0 ? List_Header_Weapon.Count - 1 : i - 1].SetActive(true);
                RectTransform rectTransform = List_Header_Weapon[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -165;
                rectTransform.anchoredPosition = anchoredPosition;
                RectTransform rectTransform1 = List_Header_Weapon[i == 0 ? List_Header_Weapon.Count - 1 : i - 1].GetComponent<RectTransform>();
                Vector2 anchoredPosition1 = rectTransform1.anchoredPosition;
                anchoredPosition1.y = -150;
                rectTransform1.anchoredPosition = anchoredPosition1;
                break;
                

            }
        }
    }

    public void Click_RightHeader()
    {
        for (int i = 0; i < List_ItemParent_Weapon.Count; i++)
        {
            if (List_ItemParent_Weapon[i].activeSelf)
            {
                if ((i + 1 >= List_Header_Weapon.Count ? 0 : i + 1) == 0 && !GO_Open.activeSelf && !GO_500.activeSelf)
                {
                    GO_Open.SetActive(true);
                    GO_500.SetActive(true);
                }
                else if ((i + 1 >= List_Header_Weapon.Count ? 0 : i + 1) != 0 && GO_Open.activeSelf &&
                         GO_500.activeSelf)
                {
                    GO_Open.SetActive(false);
                    GO_500.SetActive(false);
                }

                List_Header_Weapon[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Weapon[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);

                List_Header_Weapon[i == 0 ? List_Header_Weapon.Count - 1 : i - 1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                List_Header_Weapon[i == 0 ? List_Header_Weapon.Count - 1 : i - 1].GetComponent<Image>().color =
                    new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_ItemParent_Weapon[i].SetActive(false);
                List_ItemParent_Weapon[i + 1 >= List_Header_Weapon.Count ? 0 : i + 1].SetActive(true);
                
                RectTransform rectTransform = List_Header_Weapon[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -165;
                rectTransform.anchoredPosition = anchoredPosition;
                RectTransform rectTransform1 = List_Header_Weapon[i == 0 ? List_Header_Weapon.Count - 1 : i - 1].GetComponent<RectTransform>();
                Vector2 anchoredPosition1 = rectTransform1.anchoredPosition;
                anchoredPosition1.y = -150;
                rectTransform1.anchoredPosition = anchoredPosition1;
                break;
            }
        }
    }
    #endregion

    #region Skin

    public void Click_LeftHeader_Skin()
    {
        for (int i = 0; i < List_ItemParent_Skin.Count; i++)
        {
            if (List_ItemParent_Skin[i].activeSelf)
            {
                if ((i == 0 ? List_Header_Skin.Count - 1 : i - 1) == 0 && !GO_Open.activeSelf && !GO_500.activeSelf)
                {
                    GO_Open.SetActive(true);
                    GO_500.SetActive(true);
                }
                else if ((i == 0 ? List_Header_Skin.Count - 1 : i - 1) != 0 && GO_Open.activeSelf &&
                         GO_500.activeSelf)
                {
                    GO_Open.SetActive(false);
                    GO_500.SetActive(false);
                }

                List_Header_Skin[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Skin[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);

                List_Header_Skin[i == 0 ? List_Header_Skin.Count - 1 : i - 1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                List_Header_Skin[i == 0 ? List_Header_Skin.Count - 1 : i - 1].GetComponent<Image>().color =
                    new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_ItemParent_Skin[i].SetActive(false);
                List_ItemParent_Skin[i == 0 ? List_Header_Skin.Count - 1 : i - 1].SetActive(true);
                RectTransform rectTransform = List_Header_Skin[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -165;
                rectTransform.anchoredPosition = anchoredPosition;
                RectTransform rectTransform1 = List_Header_Skin[i == 0 ? List_Header_Skin.Count - 1 : i - 1].GetComponent<RectTransform>();
                Vector2 anchoredPosition1 = rectTransform1.anchoredPosition;
                anchoredPosition1.y = -150;
                rectTransform1.anchoredPosition = anchoredPosition1;
                break;
                

            }
        }
    }

    public void Click_RightHeader_Weapon()
    {
        for (int i = 0; i < List_ItemParent_Skin.Count; i++)
        {
            if (List_ItemParent_Skin[i].activeSelf)
            {
                if ((i + 1 >= List_Header_Skin.Count ? 0 : i + 1) == 0 && !GO_Open.activeSelf && !GO_500.activeSelf)
                {
                    GO_Open.SetActive(true);
                    GO_500.SetActive(true);
                }
                else if ((i + 1 >= List_Header_Skin.Count ? 0 : i + 1) != 0 && GO_Open.activeSelf &&
                         GO_500.activeSelf)
                {
                    GO_Open.SetActive(false);
                    GO_500.SetActive(false);
                }

                List_Header_Skin[i].GetComponent<Image>().color = new Color(0.1803922f, 0.2862745f, 0.8000001f);
                List_Header_Skin[i].transform.GetChild(0).GetComponent<Text>().color =
                    new Color(0.482353f, 0.7137255f, 0.9921569f);

                List_Header_Skin[i == 0 ? List_Header_Skin.Count - 1 : i - 1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                List_Header_Skin[i == 0 ? List_Header_Skin.Count - 1 : i - 1].GetComponent<Image>().color =
                    new Color(0.3960785f, 0.5960785f, 0.9882354f);
                List_ItemParent_Skin[i].SetActive(false);
                List_ItemParent_Skin[i + 1 >= List_Header_Skin.Count ? 0 : i + 1].SetActive(true);
                
                RectTransform rectTransform = List_Header_Skin[i].GetComponent<RectTransform>();
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = -165;
                rectTransform.anchoredPosition = anchoredPosition;
                RectTransform rectTransform1 = List_Header_Skin[i == 0 ? List_Header_Skin.Count - 1 : i - 1].GetComponent<RectTransform>();
                Vector2 anchoredPosition1 = rectTransform1.anchoredPosition;
                anchoredPosition1.y = -150;
                rectTransform1.anchoredPosition = anchoredPosition1;
                break;
            }
        }
    }

    #endregion
    #endregion

    public void RandomWeaponBuy()
    {
        int number = Int32.Parse(text_Coin.text);
        if (number < 200)
        {
            Debug.Log("Bạn thiếu tiền :))");
        }
        else
        {
            Debug.Log("Bạn đủ tiền <3");
            List<Item> listHeader = new List<Item>();
            string nameList = null;
            Image ButtonPush = null;
            if (List_ItemParent_Weapon[0].activeInHierarchy)
            {
                listHeader = new List<Item>(List_WeaponTab1);
                nameList = "List_WeaponTab1";
                ButtonPush = Button_OpenWeapon;
            }
            else if (List_ItemParent_Weapon[1].activeInHierarchy)
            {
                listHeader = new List<Item>(List_WeaponTab2);
                ButtonPush = Button_OpenWeapon;
            }
            else if (List_ItemParent_Skin[0].activeInHierarchy)
            {
                listHeader = new List<Item>(List_SkinTab1);
                nameList = "List_SkinTab1";
                ButtonPush = Button_OpenSkin;
            }
            else if (List_ItemParent_Skin[1].activeInHierarchy)
            {
                listHeader = new List<Item>(List_SkinTab2);
                ButtonPush = Button_OpenSkin;
            }
            text_Coin.text = (number - 200).ToString();
            int countStatusPay = 0;
            for (int i = 0; i < listHeader.Count; i++)
            {
                if (!listHeader[i].isStatusPay)
                {
                    countStatusPay++;
                }
            }
            
            if (countStatusPay > 1)
            {
                StartCoroutine(RandomColorCoroutine(listHeader,ButtonPush,nameList));//check 2 danh sách , mặc định đang là list_item
            }
            else
            {
                StartCoroutine(RandomColorCoroutineWhenOneItem(listHeader,nameList));
            }
        }
    }

    public void WatchYotube()
    {
        Debug.Log("Nothing!");
    }

    public void RestartGame()
    {
        GO_LoserGame.SetActive(false);
        GameController.Instance.isWinGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator RandomColorCoroutineWhenOneItem(List<Item> list_,string nameList)
    {
        for (int i = 0; i < list_.Count; i++)
        {
            if (!list_[i].isStatusPay)
            {
                
                #region Dùng riêng từng loại 1

                if (nameList.Equals("List_WeaponTab1"))
            {
                #region Cho tất cả các status của weapon tab 1, 2 về màu đỏ trước

                foreach (Item listItem in List_WeaponTab1)
                {
                    if (listItem.isStatusPay)
                    {
                        listItem.isEquipped = false;
                        listItem.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }

                foreach (Item listItem in List_WeaponTab2)
                {
                    if (listItem.isStatusPay)
                    {
                        listItem.isEquipped = false;
                        listItem.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }
                            
                #endregion
                #region Set luôn vũ khí quay được làm vũ khí chính

                LeanPool.Despawn(WeaponInShop);
                GameObject weaponInHand =
                    GameController.FindChildByName(PlayerGetWeaponInShop.transform,
                        "mixamorig:RightHand"); //tìm cánh tay để đưa weapon vào
                WeaponInShop = LeanPool.Spawn(list_WeaponInShop[list_[i].Item_Index],
                    PositionCreateWeaponInShop.position,
                    Quaternion.Euler(90, 0, 0), weaponInHand.transform);
                WeaponInShop.transform.localPosition = Vector3.zero;
                WeaponInShop.transform.localRotation = Quaternion.Euler(0, -90, -90);
                #region Tìm kiếm weapon nào phù hợp với số id trên item

                GameController.Instance.SelectWeapon(list_[i].Item_Index);

                #endregion

                #endregion
            }
            else if (nameList.Equals("List_SkinTab1"))
            {
                #region Cho status tất cả các list 2 bảng đều màu đỏ

                foreach (Item listItem in List_SkinTab1)
                {
                    if (listItem.isStatusPay)
                    {
                        listItem.isEquipped = false;
                        listItem.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }
                List_SkinTab2.ForEach(itm =>
                {
                    if (itm.isStatusPay)
                    {
                        itm.isEquipped = false;
                        itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                });
                #endregion
                
                #region Tìm kiếm weapon nào phù hợp với số id trên item

                materialPlayer.mainTexture = List_SkinsForDisplay[list_[i].Item_Index].texture;
                GameController.Instance.SelectSkin(list_[i].Item_Index);
                Config.Instance.NameSkinsCurrent = List_SkinsForDisplay[list_[i].Item_Index].name;
                #endregion
            }

                #endregion
                
                list_[i].isStatusPay = true;
                list_[i].isEquipped = true;
                list_[i].GetComponent<Image>().color = Color.red;
                yield return new WaitForSeconds(.5f);
                list_[i].GetComponent<Image>().color = Color.white;
                list_[i].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                list_[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                SaveManager.SaveWeapon1(List_WeaponTab1);
                SaveManager.SaveWeapon2(List_WeaponTab2);
                SaveManager.SaveSkin1(List_SkinTab1);
                SaveManager.SaveSkin2(List_SkinTab2);
                break;
            }
        }
    }

    #region Khi click random chạy màu đỏ sau 1 thời gian nhất định cho dừng lại

    private IEnumerator RandomColorCoroutine(List<Item> list_,Image ButtonPush,string nameList)
    {
        ButtonPush.raycastTarget = false;
        List<Item> listRandom = new List<Item>();
        for (int i = 0; i < list_.Count; i++)
        {
            if (!list_[i].isStatusPay)
            {
                listRandom.Add(list_[i]);
            }
            else if (i == list_.Count - 1 && listRandom.Count == 0)
            {
                Debug.Log("Đã mua full weapon");
            }
        }
        #region Cho chạy random

        listRandom.ForEach(item => item.GetComponent<Image>().color = Color.white);
        int randomIndex = Random.Range(0, listRandom.Count);
        listRandom[randomIndex].GetComponent<Image>().color = Color.red;
        float delay = 0.01f;
        float delayLEFT = 0.01f;
        //float timePassed = 0f;
        float totalDuration = 5f;
        float TimeWhenClick = Time.time;
        while (TimeWhenClick > Time.time - totalDuration)
        {
            yield return new WaitForSeconds(delay);
            listRandom[randomIndex].GetComponent<Image>().color = Color.white;
            randomIndex = Random.Range(0, listRandom.Count);
            listRandom[randomIndex].GetComponent<Image>().color = Color.red;

            if (delay > delayLEFT * 10f)
            {
                delay += 0.01f;
            }
            else
            {
                delay += 0.005f;
            }
        }

        #endregion

        //cho phép nhấn khi quay xong
        yield return new WaitForSeconds(1.5f);
        listRandom[randomIndex].GetComponent<Image>().color = Color.white;
        for (int i = 0; i < list_.Count; i++)
        {
            if (listRandom[randomIndex].name.Equals(list_[i].name))
            {
                #region Dùng riêng từng loại 1

                if (nameList.Equals("List_WeaponTab1"))
            {
                #region Cho tất cả các status của weapon tab 1, 2 về màu đỏ trước

                foreach (Item listItem in List_WeaponTab1)
                {
                    if (listItem.isStatusPay)
                    {
                        listItem.isEquipped = false;
                        listItem.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }

                foreach (Item listItem in List_WeaponTab2)
                {
                    if (listItem.isStatusPay)
                    {
                        listItem.isEquipped = false;
                        listItem.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }
                            
                #endregion
                #region Set luôn vũ khí quay được làm vũ khí chính

                LeanPool.Despawn(WeaponInShop);
                GameObject weaponInHand =
                    GameController.FindChildByName(PlayerGetWeaponInShop.transform,
                        "mixamorig:RightHand"); //tìm cánh tay để đưa weapon vào
                WeaponInShop = LeanPool.Spawn(list_WeaponInShop[list_[i].Item_Index],
                    PositionCreateWeaponInShop.position,
                    Quaternion.Euler(90, 0, 0), weaponInHand.transform);
                WeaponInShop.transform.localPosition = Vector3.zero;
                WeaponInShop.transform.localRotation = Quaternion.Euler(0, -90, -90);
                #region Tìm kiếm weapon nào phù hợp với số id trên item

                GameController.Instance.SelectWeapon(list_[i].Item_Index);

                #endregion

                #endregion
            }
            else if (nameList.Equals("List_SkinTab1"))
            {
                #region Cho status tất cả các list 2 bảng đều màu đỏ

                foreach (Item listItem in List_SkinTab1)
                {
                    if (listItem.isStatusPay)
                    {
                        listItem.isEquipped = false;
                        listItem.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                }
                List_SkinTab2.ForEach(itm =>
                {
                    if (itm.isStatusPay)
                    {
                        itm.isEquipped = false;
                        itm.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    }
                });
                #endregion
                
                #region Tìm kiếm weapon nào phù hợp với số id trên item

                materialPlayer.mainTexture = List_SkinsForDisplay[list_[i].Item_Index].texture;
                GameController.Instance.SelectSkin(list_[i].Item_Index);
                Config.Instance.NameSkinsCurrent = List_SkinsForDisplay[list_[i].Item_Index].name;
                #endregion
            }

                #endregion
                Debug.Log(nameList);
                list_[i].GetComponent<Image>().color = Color.white;
                list_[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                list_[i].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                list_[i].isStatusPay = true;
                list_[i].isEquipped = true;
                
            }
        }
        //Reset colors
        ButtonPush.raycastTarget = true;
        SaveManager.SaveWeapon1(List_WeaponTab1);
        SaveManager.SaveWeapon2(List_WeaponTab2);
        SaveManager.SaveSkin1(List_SkinTab1);
        SaveManager.SaveSkin2(List_SkinTab2);
    }

    #endregion
    
    #region Trở về màn hình chính

    public void BackHome()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt(Config.Instance.Prefs_LevelName,1));
    }

    #endregion

    #region Thua Game

    public void LoserGame()
    {
        if (!GO_LoserGame.activeSelf && !GameController.Instance.isWinnerOrLoser)
        {
            StartCoroutine(DelayLoserGame());
        }
    }

    IEnumerator DelayLoserGame()
    {
        yield return new WaitForSeconds(.125f);
            #region Thay layer cho weapon khi thua

            int newLayer = LayerMask.NameToLayer("Default");
            GameController.Instance.Weapon.layer = newLayer;
            #endregion
            #region Tắt hiển thị sinh mạng của boss

            if (imageBackgroundBOSS.transform.parent.gameObject.activeSelf)
            {
                imageBackgroundBOSS.transform.parent.gameObject.SetActive(false);
            }

            #endregion
            #region Tắt level trong game đi

            if (GO_Level.gameObject.activeSelf)
            {
                GO_Level.gameObject.SetActive(false);
            }
            if (GO_ImageEnemy.activeSelf)
            {
                GO_ImageEnemy.SetActive(false);
            }

            #endregion
            #region Tắt mũi tên

            GameController.Instance.Arrow.SetActive(false);

            #endregion

            #region Bật ragdoll của player

            GameController.Instance.Player_Animator.enabled = false;
            /*List<SkinnedMeshRenderer> listMaterial =  GameController.Instance.Player_Position.transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
            listMaterial.ForEach(item => item.material = GameController.Instance.colorEnemyDie[0]);*/

            #endregion

            //Tắt image hiển thị live của boss
            GameController.Instance.Content_Image_Boss.SetActive(false);

            #region Tắt slowmotion

            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            #endregion

            #region Reset background cho scene sau

            GameController.Instance.isCheckLoadScene = true;

            #endregion

            //GameController.Instance.isWinnerOrLoser = true;
            GameController.Instance.isLoser = true;
            GO_LoserGame.SetActive(true);
            GameController.Instance.isWinGame = true;
            TextLoserGame.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void btn_CONTINUE()
    {
        if (GO_LoserGame.activeSelf)
        {
            GO_LoserGame.SetActive(false);
            GameController.Instance.isWinGame = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    #endregion

    #region HeadShot

    public void HeadShot()
    {
        if (!GO_HeadShot.activeSelf)
        {
            GO_HeadShot.SetActive(true);
            Invoke("DelayHeadshot", .2f);
        }
    }

    void DelayHeadshot()
    {
        GO_HeadShot.SetActive(false);
    }

    #endregion

    #region OtherShot

    public void OtherShot()
    {
        if (!GO_OtherShot.activeSelf)
        {
            isCheckCoroutineOtherShot = true;
            GO_OtherShot.SetActive(true);
            GO_OtherShot.GetComponent<Text>().text = $"KILL {GameController.Instance.indexEnemy}";
            StartCoroutine(DelayOthershot());
        }
    }

    public void StopCoroutineOtherShot()
    {
        StopCoroutine(DelayOthershot());
    }

    IEnumerator DelayOthershot()
    {
        yield return new WaitForSeconds(.2f);
        GO_OtherShot.SetActive(false);
        isCheckCoroutineOtherShot = false;
    }

    #endregion

    #region Combo kill

    public void ComboShot(int random)
    {
        if (!GO_ComboKill[random].activeSelf)
        {
            isCheckCoroutineComboShot = true;
            GO_ComboKill[random].SetActive(true);
            string[] comboTexts = { "Double Kill", "Triple Kill", "Quadra Kill", "Master Kill" };
            int comboCount = GameController.Instance.CountCombo;
            string comboText = (comboCount >= 2 && comboCount <= 5) ? comboTexts[comboCount - 2] : "";
            if (comboCount == 5)
            {
                int countCoin = int.Parse(text_Coin.text);
                countCoin += 20;
                GameController.Instance.MoneyPlayer += countCoin;
                text_Coin.text = countCoin.ToString();
            }

            GO_ComboKill[random].GetComponent<Text>().text = $"{comboText}";
            StartCoroutine(DelayComboshot(random));
        }
    }

    public void StopCoroutineComboShot()
    {
        StopCoroutine(DelayOthershot());
    }

    IEnumerator DelayComboshot(int randomCombo)
    {
        yield return new WaitForSeconds(.2f);
        GO_ComboKill[randomCombo].SetActive(false);
        isCheckCoroutineComboShot = false;
    }

    #endregion
}