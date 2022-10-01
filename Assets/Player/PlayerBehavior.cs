using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float maxHp, speed;
    [SerializeField] private Camera mainCamera;
    private float _hp; public float hp {get {return _hp;}}
    private float _attack; public float attack {get {return _attack;}}
    
    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null) mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
    }

    public void RefreshHealth()
    {
        maxHp = _hp;
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
        float diagMultiplier = (movementMultiplierX != 0f && movementMultiplierY != 0f) ? Mathf.Sqrt(2) / 2 : 1f;
        transform.position = new Vector3(transform.position.x + diagMultiplier * movementMultiplierX * speed * Time.deltaTime,
                                         transform.position.y + diagMultiplier * movementMultiplierY * speed * Time.deltaTime,
                                         transform.position.z);
    }
}
