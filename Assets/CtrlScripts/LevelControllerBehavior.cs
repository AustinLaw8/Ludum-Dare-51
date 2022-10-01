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
    public List<EnemyBehavior> livingEnemies;
    bool _levelActive; 
    private Weapon _currentWeapon; 

    private float startTime;
    private int paused; // toggle for pause menu
    void Awake()
    {
        LevelControllerBehavior.levelController = this;
        _playerBehavior = player.GetComponent<PlayerBehavior>();
        _levelActive = false;
        startTime = -1;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        seconds10 = 10
        if timer < 0                    if timer > seconds10
            do whater action                do w/e action
            timer = seconds10               timer = 0
        timer -= Time.deltaTime          timer += Time.deltaTime
        */
        
        LevelStart(); // for now, level starts at game start
    }

    // Update is called once per frame
    void Update()
    {
        
        // Input

        //  Pause
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // cue pause menu
            paused *= -1;
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

        //  Weapon fire
        //if (Input.mouse)

    }

    // called when game starts. Does setup/refresh
    void LevelStart()
    {
        RefreshEnemies();
        player.transform.position = new Vector3(0f, 0f, 0f);
        _playerBehavior.RefreshHealth();
        _levelActive = true;
        startTime = 0f;
        paused = -1; // not paused
    }

    // Deletes all enemies and empties list
    void RefreshEnemies()
    {
        while (livingEnemies.Count > 0)
        {
            Destroy(livingEnemies[0].gameObject);
            livingEnemies.RemoveAt(0);
        }
    }
}


