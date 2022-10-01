using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HERE: got rid of using System.Math, it's Mathf instead and you already have access

public abstract class EnemyBehavior : MonoBehaviour 
{
    protected static PlayerBehavior _playerBehavior; 

    // Enemy fields
    public float attack, range, speed, hp, maxHp, cooldown; // cd for attacking

    // get dist from this enemy to player object
    public float dist2Player()
    {
        float px = _playerBehavior.transform.position.x;
        float py = _playerBehavior.transform.position.y;
        float x = transform.position.x;
        float y = transform.position.y;
        return Mathf.Sqrt(Mathf.Pow((px - x), 2) + Mathf.Pow((py - y), 2));
    }

    public void move(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    // helper to normalize to unit vector CHECK CODE
    private Vector3 unit(Vector3 vect)
    {
        float mag = Mathf.Sqrt(Mathf.Pow(vect.x, 2) + Mathf.Pow(vect.y, 2) + Mathf.Pow(vect.z, 2));
        return vect/mag;
    }


    // CHECK CODE! move a set distance towards the player
    public void move2Player()
    {
        Vector3 pPos = _playerBehavior.transform.position;
        Vector3 pos = transform.position;
        float px = pPos.x; // player x, y
        float py = pPos.y;
        float x = pos.x; // enemy x, y
        float y = pos.y;
        float z = pos.z;
        float k = speed * Time.deltaTime;
        float dx = unit(pPos - pos).x; //Mathf.Atan2(y - py, x - px); // angle between enemy and player in Rad
        float dy = unit(pPos - pos).y;

        //Debug.Log(dx);
        //Debug.Log(dy);

        transform.position = new Vector3(x + k*dx, y + k*dy, z); //Mathf.Atan2(destinationPos.y - originPos.y, destinationPos.x - originPos.x) * 180f / Mathf.PI;
    }

    public void DamageEnemy(float amt)
    {
        hp = Mathf.Min(maxHp, hp - amt); // overheal boundary case
    }

    public abstract void Attack();

    
    void Start()
    {
        Setup(maxHp, speed, attack, range, cooldown);
    }
    
    void Update()
    {
        if (range < dist2Player())
            move2Player(); // set default x,y multipliers
        else
            Attack();

        // update cooldown if necessary
        // cooldown 
    }

    protected void Setup(float health, float sp, float atk, float rng, float cd) // default curr health to max health
    {
        if (!_playerBehavior)
        {
            _playerBehavior = LevelControllerBehavior.levelController.playerBehavior;
        }
    
        // initialize defualt stats
        health = maxHp;
        speed = sp;
        attack = atk;
        range = rng;
        cooldown = cd;
    }

    // ============================== COMMON METHODS =====================================
    public static Vector2 Vec3ToVec2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static float Distance2D(Vector3 start, Vector3 end)
    {
        return Vector2.Distance(Vec3ToVec2(start), Vec3ToVec2(end));
    }

    public static float GetAngleBetweenPoints2D_Rads(Vector3 originPos, Vector3 destinationPos)
    {
        return Mathf.Atan2(destinationPos.y - originPos.y, destinationPos.x - originPos.x);
    }

    public static float GetAngleBetweenPoints2D_Rads(Vector2 originPos, Vector2 destinationPos)
    {
        return Mathf.Atan2(destinationPos.y - originPos.y, destinationPos.x - originPos.x);
    }

    public static float GetAngleBetweenPoints2D_Degs(Vector3 originPos, Vector3 destinationPos)
    {
        return Mathf.Atan2(destinationPos.y - originPos.y, destinationPos.x - originPos.x) * 180f / Mathf.PI;
    }

    public static float GetAngleBetweenPoints2D_Degs(Vector2 originPos, Vector2 destinationPos)
    { // 
        return Mathf.Atan2(destinationPos.y - originPos.y, destinationPos.x - originPos.x) * 180f / Mathf.PI;
    }

    public static float GetAngleToMouse_Rads(Camera camera, Vector3 originPos)
    {            
        Vector3 cursorPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return GetAngleBetweenPoints2D_Rads(originPos, cursorPos);
    }

    public static float GetAngleToMouse_Degs(Camera camera, Vector3 originPos)
    {
        return GetAngleToMouse_Rads(camera, originPos) * 180f / Mathf.PI; 
    }

    public static Vector2 GetUnitInDir2D_Rads(float rads)
    {
        return new Vector2(Mathf.Cos(rads), Mathf.Sin(rads));
    }
}
