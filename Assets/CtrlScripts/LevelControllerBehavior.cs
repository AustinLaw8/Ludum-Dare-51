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

    public GameObject player;
    private PlayerBehavior _playerBehavior; public PlayerBehavior playerBehavior {get {return _playerBehavior;}}
    public bool _levelActive; 
    public float attackCd;

    // settings 
    private float musicVol, sfxVol, masterVol, fontSize;

    private float startTime;
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
        startTime = -1;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        attackCd = Weapon.WeaponDict[Weapon.currentWeapon].getCooldown();

        // Input

        //  Pause
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // cue pause menu
            UiCanvasBehavior.uiCanvasBehavior.pause();
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
            Weapon.Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // check doc
        }

    }

    // called when game starts. Does setup/refresh
    public void LevelStart()
    {
        player.transform.position = new Vector3(0f, 0f, 0f);
        _playerBehavior.RefreshHealth();
        _levelActive = true;
        startTime = 0f;
        Weapon.currentWeapon = Weapon.WeaponType.SWORD;
    }
}