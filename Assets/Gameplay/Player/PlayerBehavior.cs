using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float originalMaxHp, baseSpeed;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private UnityEngine.UI.Image _healthBar;
    private HealthBarBehavior _healthBarBehavior;
    private float _maxHp; public float maxHp {get {return _maxHp;} set {_maxHp = value;}}
    private float _hp; public float hp {get {return _hp;} set {_hp = value;}}
    private float _attack; public float attack {get {return _attack;} set {_attack = value;}}
    private float _critRate; public float critRate {get {return _critRate;} set {_critRate = value;}}
    private float _speedStat; public float speedStat {get {return _speedStat;} set {_speedStat = value;}}
    [SerializeField] private GameObject boundaryTopRight, boundaryBottomLeft;
    [SerializeField] private GameObject _uiCanvas;

    public int scaledPixelHeight;
    
    public bool facingLeft;
    // Start is called before the first frame update
    void Start()
    {
        if (_mainCamera == null) _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _healthBarBehavior = _healthBar.GetComponent<HealthBarBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the player sprite isn't looking towards the mouse,
        facingLeft = Input.mousePosition.x < Camera.main.pixelWidth / 2;
        if (facingLeft && this.transform.localScale.x > 0 || !facingLeft && this.transform.localScale.x < 0)
        {
            flip();
        }

        // figure out units shown in y-direction by _main camera
        // change the y-position in the vector3 below by subtracting 10% of that distance you figured out above

        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f *_mainCamera.orthographicSize, _mainCamera.transform.position.z);
        LevelControllerBehavior.SetYDependentOrderInLayer(gameObject);
    }

    public void RefreshPlayerStatsAndHealth()
    {
        _maxHp = originalMaxHp;
        _speedStat = 100f;
        _attack = 100f;
        _critRate = 0f;
        RefreshHealth();
    }

    public void RefreshHealth()
    {
        _hp = maxHp;
    }

    public void DamagePlayer(float hpDamaged)
    {
        _hp = Mathf.Min(maxHp, _hp - hpDamaged);
        if (_hp <= 0f)
        {
            LevelControllerBehavior.levelController._gameOver = true;
            _uiCanvas.GetComponent<UiCanvasBehavior>().enterDeathScreen();
        }
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    // returns given vector's distance to player
    public float dist2Player(Vector3 pos)
    {
        return Vector3.Distance(this.transform.position, pos);
    }

    // Controls WASD movement
    // Input is read by LevelControllerBehavior, which calls this function
    public void Walk(int movementMultiplierX, int movementMultiplierY)
    {
        float diagMultiplier = (movementMultiplierX != 0f && movementMultiplierY != 0f) ? Mathf.Sqrt(2) / 2 : 1f;
        
        float dx = diagMultiplier * movementMultiplierX * baseSpeed * speedStat / 100 * Time.deltaTime;
        float dy = diagMultiplier * movementMultiplierY * baseSpeed * speedStat / 100 * Time.deltaTime;
        
        transform.position = new Vector3(transform.position.x + dx,
                                         transform.position.y + dy,
                                         transform.position.z);
    }

    private void flip() {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        _healthBarBehavior.FlipToMaintainBillboard();
    }
}
