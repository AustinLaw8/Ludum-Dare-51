using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class RenderOrder : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float yLevel;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameObject.Find("Player");
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    { 
        spriteRenderer.sortingLayerName = player.transform.position.y > this.transform.position.y + yLevel ? "InFront" : "Default";
    }
}
