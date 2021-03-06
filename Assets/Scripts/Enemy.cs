using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    public int xpValue = 1;

    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPos;

	public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

	protected override void Start()
	{
		base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPos = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
	}

	private void FixedUpdate()
	{
		if (Vector3.Distance(playerTransform.position, startingPos) < chaseLength)
		{
            if(Vector3.Distance(playerTransform.position, startingPos) < triggerLength)
			{
				chasing = true;
			}

			if (chasing)
			{
				if (!collidingWithPlayer)
				{
					UpdateMotor((playerTransform.position - transform.position).normalized);
				}
			}
			else
			{
				UpdateMotor(startingPos - transform.position);
			}
		}
		else
		{
			UpdateMotor(startingPos - transform.position);
			chasing = false;
		}

		collidingWithPlayer = false;
		boxCollider.OverlapCollider(filter, hits);

		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i] == null)
			{
				continue;
			}
			if (hits[i].tag == "Fighter" && hits[i].name == "Player")
			{
				collidingWithPlayer = true;
			}

			hits[i] = null;
		}

	}

	protected override void Die()
	{
		Destroy(gameObject);
		GameManager.instance.experience += xpValue;
		GameManager.instance.ShowText("+ " + xpValue + " xp", 25, Color.magenta, transform.position, Vector3.up * 100, .5f);
	}
}
