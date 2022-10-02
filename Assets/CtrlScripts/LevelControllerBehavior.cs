using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerBehavior : MonoBehaviour
{
    // Static LevelController for global access to current levelController
    //  This is because I don't want to deal with writing access to the levelController for everything
    //  but I still want things like Update to trigger.
    //  I know there's a class that handles this but I forgot what it's called, this should suffice for now
    //  This will also allow all other classes to access common data that is stored in the LevelControllerBehavior
    public static LevelControllerBehavior levelController;
    private static Vector3 PLAYER_CENTER = new Vector3(.1f,-.1f,0);
    private static float PLAYER_RADIUS = .4f;

    public GameObject player;
    private PlayerBehavior _playerBehavior; public PlayerBehavior playerBehavior {get {return _playerBehavior;}}
    public bool _levelActive; 
    public Weapon currentWeapon;

    /* Weapon prefabs */
    public GameObject SWORD_PREFAB;
    public Dictionary<Weapon.WeaponType, GameObject> weaponPrefabs;

    // settings 
    private float musicVol, sfxVol, masterVol, fontSize;

    void Awake()
    {
        if (LevelControllerBehavior.levelController) {
            Destroy(this.gameObject);
        } else {
            LevelControllerBehavior.levelController = this;
        }
        if (player == null) player = GameObject.Find("Player");
        _playerBehavior = player.GetComponent<PlayerBehavior>();
        _levelActive = false;
        weaponPrefabs = new Dictionary<Weapon.WeaponType, GameObject>() {
                { Weapon.WeaponType.SWORD, SWORD_PREFAB }};
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Input

        //  Pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            // cue pause menu
            if (_levelActive)
            {
                UiCanvasBehavior.uiCanvasBehavior.pause();
            }
            else if (UiCanvasBehavior.uiCanvasBehavior.paused)
            {
                UiCanvasBehavior.uiCanvasBehavior.ButtonSettingsBack();
            }
        }

        //  Movement
        //   Up/Down
        int movementMultiplierY = 0; // 0 means no movement, 1 means specified direction (up), -1 means opposite (down)
        if (Input.GetKey(KeyCode.W) != Input.GetKey(KeyCode.S))
        {
            movementMultiplierY = Input.GetKey(KeyCode.W) ? 1 : -1;
        }
        //   Left/Right
        int movementMultiplierX = 0; // 0 means no movement, 1 means specified direction (up), -1 means opposite (down)
        if (Input.GetKey(KeyCode.D) != Input.GetKey(KeyCode.A))
        {
            movementMultiplierX = Input.GetKey(KeyCode.D) ? 1 : -1;
        }
        _playerBehavior.Walk(movementMultiplierX, movementMultiplierY);

        // Weapon fire: 0 is L, 1 is R
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) // L targets nearest enemy, R targets mouse location, default to nearest
        {
            currentWeapon.Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // check doc
        }
    }

    // called when game starts. Does setup/refresh
    public void LevelStart()
    {
        player.transform.position = new Vector3(0f, 0f, 0f);
        _playerBehavior.RefreshHealth();
        _levelActive = true;
        currentWeapon = GameObject.Instantiate(
                weaponPrefabs[Weapon.WeaponType.SWORD],
                weaponPrefabs[Weapon.WeaponType.SWORD].GetComponent<Weapon>().offset + new Vector3(PLAYER_CENTER.x + PLAYER_RADIUS, PLAYER_CENTER.y, 0f),
                Quaternion.identity,
                _playerBehavior.transform)
                .GetComponent<Weapon>();
    }

    // Called by any gameObject with a SpriteRenderer in their Update to set their OrderInLayer to a value dependent on the lowest point of their y-position
    public static void SetYDependentOrderInLayer(GameObject callingObject)
    {
        SpriteRenderer renderer = callingObject.GetComponent<SpriteRenderer>();
        //renderer.bounds.
    }
}