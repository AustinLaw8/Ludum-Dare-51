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
    public static Vector3 PLAYER_CENTER = new Vector3(.1f,-.1f,0);
    public static float PLAYER_RADIUS = .5f;
    public static float MIN_SPAWN_DISTANCE = 12.5f;
    public static int ENEMY_COUNT_INITIAL_SPAWN = 4;
    public static float DIFFICULTY_FACTOR = 0.5f; // for now affects BOTH scaling of enmy spawn count per wave and enemy non-HP stats
    public GameObject MELEE_ENEMY_PREFAB;

    public GameObject player;
    private PlayerBehavior _playerBehavior; public PlayerBehavior playerBehavior {get {return _playerBehavior;}}
    [SerializeField] private GameObject _swapOptionsBox; private SwapOptionsBehavior _swapOptionsBehavior;
    public bool _levelActive; 
    public bool _gameOver = false;
    public Weapon currentWeapon;

    /* Weapon prefabs */
    public GameObject SWORD_PREFAB, STAR_PREFAB, DYNAMITE_PREFAB, FAN_PREFAB;
    public Dictionary<Weapon.WeaponType, GameObject> weaponPrefabs;

    // settings 
    public AudioSource menuThemeSource;
    public AudioSource battleThemeSource;
    public AudioSource sfxSource;
    public AudioClip onButtonPress;

    private float musicVol, sfxVol, masterVol, fontSize;

    private float _gameDuration, _gameDurationNextSwap;
    public float dimensionlessClampedTimeTilNextSwap {get {return Mathf.Clamp((10f -_gameDurationNextSwap + _gameDuration) / 10f, 0f, 1f);}}
    public float swapCount {get {return Mathf.Floor(_gameDuration / 10f);}}
    
    public uint playerScore;

    void Awake()
    {
        if (LevelControllerBehavior.levelController) {
            Destroy(this.gameObject);
        } else {
            LevelControllerBehavior.levelController = this;
        }
        if (player == null) player = GameObject.Find("Player");
        _playerBehavior = player.GetComponent<PlayerBehavior>();
        _swapOptionsBehavior = _swapOptionsBox.GetComponent<SwapOptionsBehavior>();
        _levelActive = false;
        _gameDuration = 0f;
        _gameDurationNextSwap = 10f;   
        weaponPrefabs = new Dictionary<Weapon.WeaponType, GameObject>() {
                { Weapon.WeaponType.SWORD, SWORD_PREFAB },
                { Weapon.WeaponType.STAR, STAR_PREFAB   },
                { Weapon.WeaponType.DYNAMITE, DYNAMITE_PREFAB},
                { Weapon.WeaponType.FAN, FAN_PREFAB}
                };
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelActive = false;
        if (menuThemeSource == null) menuThemeSource = Camera.main.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        if (battleThemeSource == null) battleThemeSource = Camera.main.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        if (sfxSource == null) sfxSource = Camera.main.transform.GetChild(3).gameObject.GetComponent<AudioSource>();
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

        if (_levelActive)
        {
            // Update timer
            _gameDuration += Time.deltaTime;
            if (_gameDuration >= _gameDurationNextSwap)
            {
                // Swap time!
                TenSecondSwap();
                _gameDurationNextSwap += 10f;
            }

            //  Movement
            //   Up/Down

            // check player cannot go out of bounds
            float rBound = 47.5f;
            float lBound = -47.0f;
            float upperBound = 27.1f;
            float lowerBound = -25.5f;

            /*
            if(transform.position.x > rBound)
                movementMultiplierX = 0;
            else if(transform.position.x < lBound)
                movementMultiplierX = 0;
            if(transform.position.y < lowerBound)
                movementMultiplierY = 0;
            else if(transform.position.y > upperBound)
                movementMultiplierY = 0;
            */

            int movementMultiplierY = 0; // 0 means no movement, 1 means specified direction (up), -1 means opposite (down)
            if (Input.GetKey(KeyCode.W) != Input.GetKey(KeyCode.S))
            {
                if(Input.GetKey(KeyCode.W) && !(_playerBehavior.transform.position.y > upperBound))
                    movementMultiplierY = Input.GetKey(KeyCode.W) ? 1 : -1;
                else if(Input.GetKey(KeyCode.S) && !(_playerBehavior.transform.position.y < lowerBound))
                    movementMultiplierY = Input.GetKey(KeyCode.W) ? 1 : -1;
            }
            //   Left/Right
            int movementMultiplierX = 0; // 0 means no movement, 1 means specified direction (up), -1 means opposite (down)
            if (Input.GetKey(KeyCode.D) != Input.GetKey(KeyCode.A))
            {
                if(Input.GetKey(KeyCode.D) && !(_playerBehavior.transform.position.x > rBound))
                    movementMultiplierX = Input.GetKey(KeyCode.D) ? 1 : -1;
                else if(Input.GetKey(KeyCode.A) && !(_playerBehavior.transform.position.x < lBound))
                    movementMultiplierX = Input.GetKey(KeyCode.D) ? 1 : -1;
                //movementMultiplierX = Input.GetKey(KeyCode.D) ? 1 : -1;
            }
            _playerBehavior.Walk(movementMultiplierX, movementMultiplierY);

            // Weapon fire: 0 is L, 1 is R
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButton(1)) // L targets nearest enemy, R targets mouse location, default to nearest
            {
                currentWeapon.Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // check doc
            }

            // Swap selection
            if (Input.GetKey(KeyCode.Q) != Input.GetKey(KeyCode.E))
            {
                _swapOptionsBehavior.SelectOption(Input.GetKey(KeyCode.Q) ? SwapOptionsBehavior.SelectionOption.LEFT : SwapOptionsBehavior.SelectionOption.RIGHT);
            }
        }
    }

    // called when game starts. Does setup/refresh
    public void LevelStart()
    {
        playerScore = 0;
        /* Swap musics */
        menuThemeSource.Stop();
        menuThemeSource.time = 0;
        battleThemeSource.Play();
        battleThemeSource = Camera.main.transform.GetChild(2).GetComponent<AudioSource>();
        battleThemeSource.PlayDelayed(3.5f);

        player.transform.position = new Vector3(0f, 0f, 0f);
        _playerBehavior.RefreshPlayerStatsAndHealth();
        _levelActive = true;
        _gameOver = false;
        _gameDuration = 0f;
        _gameDurationNextSwap = 10f;
        ChangeWeapon(Weapon.WeaponType.DYNAMITE);
        _swapOptionsBehavior.SelectOption(SwapOptionsBehavior.SelectionOption.LEFT);
        TenSecondSwap(false);
    }

    private void ChangeWeapon(Weapon.WeaponType newWeapon)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = GameObject.Instantiate(
                weaponPrefabs[newWeapon],
                _playerBehavior.transform, false)
                .GetComponent<Weapon>();
        currentWeapon.gameObject.transform.localPosition = new Vector3(PLAYER_CENTER.x + PLAYER_RADIUS, PLAYER_CENTER.y, 0f);
    }

    // swap audio to main menu music
    public void resetAudio()
    {
        battleThemeSource.Stop();
        battleThemeSource.time = 0;
        battleThemeSource = Camera.main.transform.GetChild(1).GetComponent<AudioSource>();
        menuThemeSource.Play();
    }

    public void SFX(AudioClip c, float mult=1f)
    {
        float tempVol = sfxSource.volume;
        sfxSource.volume *= mult;
        sfxSource.clip = c;
        sfxSource.Play();
        sfxSource.volume = tempVol;
    }

    public void OnButtonPress() {
        sfxSource.clip = onButtonPress;
        sfxSource.Play();
    }
    
    // Called by any gameObject with a SpriteRenderer in their Update to set their OrderInLayer to a value dependent on the lowest point of their y-position
    public static void SetYDependentOrderInLayer(GameObject callingObject)
    {
        SpriteRenderer renderer = callingObject.GetComponent<SpriteRenderer>();
        renderer.sortingOrder = Mathf.FloorToInt(renderer.bounds.min.y * -1000f);
        // for now looks at "feet" which is kind of jank for things small and "airborne" like stars. Might change in future
    }

    // Called every 10 seconds and handles every event at that time
    // Don't worry about updating next swap time, already handled in Update() above
    private void TenSecondSwap(bool claimSelectedOptions = true)
    {
        SpawnEnemyWave();
        if (claimSelectedOptions)
            ClaimSelectedOptions();
        SetNextSwapOptions();
    }

    private void SpawnEnemyWave()
    {
        int enemiesToSpawn = Mathf.FloorToInt(ENEMY_COUNT_INITIAL_SPAWN + swapCount * DIFFICULTY_FACTOR * Random.Range(0.8f, 1.2f));
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject newEnemy = GameObject.Instantiate(MELEE_ENEMY_PREFAB) as GameObject;
            newEnemy.transform.position = playerBehavior.transform.position + MIN_SPAWN_DISTANCE * Random.Range(1f, 3f) * Get2DUnitFromRadians(Random.Range(0f, 2 * Mathf.PI));
            EnemyBehavior newEnemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
            float statScaleFactor = Mathf.Pow((1f + DIFFICULTY_FACTOR / 10f), swapCount) * Random.Range(0.8f, 1.2f);
            newEnemyBehavior.maxHp *= statScaleFactor;
            newEnemyBehavior.hp = newEnemyBehavior.maxHp;
            newEnemyBehavior.speed += newEnemyBehavior.speed * DIFFICULTY_FACTOR / 20f * swapCount * Random.Range(0.8f, 1.2f);
        }
    }

    private Vector3 Get2DUnitFromRadians(float radians)
    {
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
    }

    private void SetNextSwapOptions()
    {
        // Random weapons
        int weaponCount = Weapon.weaponList.GetLength(0);
        NextSwapData.weaponL = Weapon.weaponList[Random.Range(0, weaponCount)];
        NextSwapData.weaponR = Weapon.weaponList[Random.Range(0, weaponCount)];
        while (NextSwapData.weaponL == NextSwapData.weaponR)
        {
            NextSwapData.weaponR = Weapon.weaponList[Random.Range(0, weaponCount)];
        }
        
        // Random Stat choice
        int boostableStatCount = NextSwapData.BoostableStatList.GetLength(0);
        NextSwapData.statTypeL = NextSwapData.BoostableStatList[Random.Range(0, boostableStatCount)];
        NextSwapData.statTypeR = NextSwapData.BoostableStatList[Random.Range(0, boostableStatCount)];
        while (NextSwapData.statTypeL == NextSwapData.statTypeR)
        {
            NextSwapData.statTypeR = NextSwapData.BoostableStatList[Random.Range(0, boostableStatCount)];
        }

        // Random stat magnitudes
        NextSwapData.boostMagnitudeL = Random.Range(2, 7);
        if (NextSwapData.statTypeL == NextSwapData.BoostableStat.HP)
        {
            NextSwapData.boostMagnitudeL *= 5;
        }
        NextSwapData.boostMagnitudeR = Random.Range(2, 7);
        if (NextSwapData.statTypeR == NextSwapData.BoostableStat.HP)
        {
            NextSwapData.boostMagnitudeR *= 5;
        }

        _swapOptionsBehavior.UpdateChoiceVisuals(NextSwapData.weaponL, NextSwapData.weaponR,
                                                 NextSwapData.statTypeL, NextSwapData.statTypeR,
                                                 NextSwapData.boostMagnitudeL, NextSwapData.boostMagnitudeR);
    }

    private void ClaimSelectedOptions()
    {
        Weapon.WeaponType newWeapon;
        NextSwapData.BoostableStat boostedStat;
        int boostMagnitude;

        if (_swapOptionsBehavior.leftSideSelected)
        {
            newWeapon = NextSwapData.weaponL;
            boostedStat = NextSwapData.statTypeL;
            boostMagnitude = NextSwapData.boostMagnitudeL;
        }
        else
        {
            newWeapon = NextSwapData.weaponR;
            boostedStat = NextSwapData.statTypeR;
            boostMagnitude = NextSwapData.boostMagnitudeR;
        }

        ChangeWeapon(newWeapon);

        switch (boostedStat)
        {
            case NextSwapData.BoostableStat.ATTACK:
                playerBehavior.attack += boostMagnitude;
                break;
            case NextSwapData.BoostableStat.HP:
                playerBehavior.maxHp += boostMagnitude;
                playerBehavior.hp += boostMagnitude;
                break;
            case NextSwapData.BoostableStat.CRITRATE:
                playerBehavior.critRate += boostMagnitude;
                break;
            case NextSwapData.BoostableStat.SPEED:
                playerBehavior.speedStat += boostMagnitude;
                break;
        }
    }

    public static class NextSwapData
    {
        public enum BoostableStat {ATTACK, HP, SPEED, CRITRATE};
        public static BoostableStat[] BoostableStatList = new BoostableStat[4] {BoostableStat.ATTACK, BoostableStat.HP, BoostableStat.SPEED, BoostableStat.CRITRATE};

        public static Weapon.WeaponType weaponL, weaponR;
        public static BoostableStat statTypeL, statTypeR;
        public static int boostMagnitudeL, boostMagnitudeR;
    }
}