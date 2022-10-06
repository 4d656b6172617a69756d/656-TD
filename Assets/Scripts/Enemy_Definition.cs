using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Definition : MonoBehaviour, IEffectable
{
	// public Image healthBar;
	public GameObject deathEffect;
	private Transform target;
	public StatusEffectData _data;

	public float currentHealth;
	public float maxHealth = 50;

	private bool isDead = false;
	private int wavepoint = 0;
    
	private float moveSpeed = 20f;
	private float currentEffectTime = 0f;
	private float nextTickTime = 0f;

	void Start()
	{
		target = Waypoint_Logic.points[0];
		currentHealth = maxHealth;
	}

	void Update()
	{
		if (_data != null) HandleEffect();

		Vector3 direction = target.position - transform.position;
		transform.Translate(moveSpeed * Time.deltaTime * direction.normalized, Space.World);

		if (Vector3.Distance(transform.position, target.position) <= 0.4f)
		{
			if (wavepoint >= Waypoint_Logic.points.Length - 1)
			{
				Destroy(gameObject);
				return;
			}
			wavepoint++;
			target = Waypoint_Logic.points[wavepoint];
		}
	}

	public void Debuff_End()
	{
		_data = null;
		currentEffectTime = 0;
		nextTickTime = 0;
		moveSpeed = 20f;
	}

	public void Debuff_Apply(StatusEffectData _data)
	{
		Debuff_End();
		this._data = _data;
		if (_data.MovementPenalty > 0)
		{
			moveSpeed /= _data.MovementPenalty;
		}
	}

	public void HandleEffect()
	{
		if (currentHealth <= 0 && !isDead)
		{
			Die();
		}

		currentEffectTime += Time.deltaTime;

		if (currentEffectTime >= _data.Lifetime)
		{
			Debuff_End();
		}

		if (_data == null)
		{
			return;
		}

		if (_data.DOTAmount != 0 && currentEffectTime > nextTickTime)
		{
			nextTickTime += _data.TickSpeed;
            currentHealth -= _data.DOTAmount;
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		}

		
	}

	public void TakeDamage(int amount)
	{
        currentHealth -= amount;
		Debug.Log("Damage: " + amount + "Current Health: " + currentHealth);
		if (currentHealth <= 0 && !isDead)
		{
			Die();
		}
	}

	void Die()
	{
		isDead = true;
		GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);
		Destroy(gameObject);
	}

}
