using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 2.0f;

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

	private Animator anim;
	private float cooldown = .5f;
	private float lastSwing;

	protected override void Start()
	{
		base.Start();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	protected override void Update()
	{
		base.Update();

		if (Input.GetMouseButtonDown(0))
		{
			if (Time.time - lastSwing > cooldown)
			{
				lastSwing = Time.time;
				Swing();
			}
		}
	}

	protected override void OnCollide(Collider2D col)
	{
		if (col.tag == "Fighter")
		{
			if (col.name == "Player")
			{
				return;
			}

			Damage dmg = new Damage
			{
				damageAmount = damagePoint,
				origin = transform.position,
				pushForce = pushForce
			};

			col.SendMessage("ReceiveMessage", dmg);
		}
	}

	private void Swing()
	{
		anim.SetTrigger("Swing");
	}
}
