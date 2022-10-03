using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float originalMaxHp, baseSpeed;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private UnityEngine.UI.Image _healthBar;
    private HealthBarBehavior _healthBarBehavior;
    private float _maxHp; public float maxHp {get {return _maxHp;}}
    private float _hp; public float hp {get {return _hp;}}
    private float _attack; public float attack {get {return _attack;}}
    private float _critRate; public float critRate {get {return _critRate;}}
    private float _speedStat; public float speedStat {get {return _speedStat;}}
    
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

        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, _mainCamera.transform.position.z);
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
        // check 
        float diagMultiplier = (movementMultiplierX != 0f && movementMultiplierY != 0f) ? Mathf.Sqrt(2) / 2 : 1f;
        transform.position = new Vector3(transform.position.x + diagMultiplier * movementMultiplierX * baseSpeed * speedStat / 100 * Time.deltaTime,
                                         transform.position.y + diagMultiplier * movementMultiplierY * baseSpeed * speedStat / 100 * Time.deltaTime,
                                         transform.position.z);
    }

    private void flip() {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        _healthBarBehavior.FlipToMaintainBillboard();
    }
}
