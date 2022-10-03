using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HERE: got rid of using System.Math, it's Mathf instead and you already have access

public abstract class EnemyBehavior : MonoBehaviour 
{
    protected static PlayerBehavior _playerBehavior; 
    protected Animator anim;

    // Enemy fields
    public float attack, range, speed, hp, maxHp, cooldown, maxCD; // cd for attacking

    // get dist from this enemy to player object
    public float dist2Player()
    {
        return Vector3.Distance(_playerBehavior.transform.position, this.transform.position);
    }

    public float dist2Mouse()
    {
        return Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), this.transform.position);
    }

    public void move(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    public void move2Player()
    {
        // Gets unit vector in direction of player, then multiplies by speed * time to get deltaPosition to add
        anim.SetBool("attacking", false);
        transform.position += (_playerBehavior.transform.position - this.transform.position).normalized * speed * Time.deltaTime;
    }

    public void facePlayer()
    {
        gameObject.GetComponent<SpriteRenderer>().flipX = _playerBehavior.transform.position.x - this.transform.position.x < 0;
    }

    public void DamageEnemy(float amt)
    {
        hp = Mathf.Min(maxHp, hp - amt); // overheal boundary case
        if (hp <= 0) {
            Destroy(this.gameObject);
        }
        Debug.Log("hit");
        StartCoroutine(flashWhite());
    }

    public abstract void Attack();
    
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        Setup(maxHp, speed, attack, range, cooldown);
    }

    void Update()
    {
        if (LevelControllerBehavior.levelController._levelActive)
        {
            if (range < dist2Player())
                move2Player(); // set default x,y multipliers
            else
                Attack();
            facePlayer();
            // update cooldown if necessary

            if (cooldown > 0)
                cooldown -= Time.deltaTime;

            LevelControllerBehavior.SetYDependentOrderInLayer(gameObject);
        }
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
        maxCD = cd; // max timer on cooldown
        cooldown = 0; // current cooldown
    }

    private IEnumerator flashWhite() {
        float t = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float animTime = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        if (anim.GetBool("attacking")) {
            anim.Play("EnemyAttackWhite", -1, t % animTime);
        } else {
            anim.Play("EnemyFloatWhite", -1, t % animTime);
        }

        yield return new WaitForSeconds(.1f);

        t = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        animTime = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        if (anim.GetBool("attacking")) {
            anim.Play("EnemyAttack", -1, t % animTime);
        } else {
            anim.Play("MeleeEnemyMove", -1, t % animTime);
        }
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

    public static Vector3 RotateVector(Vector3 vec, float angle)
    {
        return new Vector3(
                Mathf.Cos(angle) * vec.x - Mathf.Sin(angle) * vec.y,
                Mathf.Sin(angle) * vec.x + Mathf.Cos(angle) * vec.y
        );
    }
}
