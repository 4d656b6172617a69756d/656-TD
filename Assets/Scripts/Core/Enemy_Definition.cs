using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Definition : MonoBehaviour, IEffectable
{
	public GameObject deathEffect;
	public SpawnManager SpawnManager;
	public StatusEffectData _data;
	private Transform target;

	public float currentHealth;
	public float maxHealth = 50;

	[Header("Enemy Type Bounty")]

	public int diceRollRange = 5;
	public int normalBounty = 10;
	public int specialBounty = 25;
	public int bossBounty = 50;
	public int massBounty = 5;
	public int farmBounty = 100;
	public string enemyType;
	public int totalBounty;

	public bool isDead = false;
	private int wavepoint = 0;
    
	private float moveSpeed = 20f;
	private float currentEffectTime = 0f;
	private float nextTickTime = 0f;

	private Dictionary<string, int> capturedEnemies = new Dictionary<string, int>();

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
                Player_Currency.lives--;
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
		Debug.Log("Damage: " + amount + " // Current Health: " + currentHealth);
		if (currentHealth <= 0 && !isDead)
		{
			Die();
		}
	}

	void Die()
	{
		isDead = true;

		GetBounty();
		GetMana();

		GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 3f);
		Destroy(gameObject);
	}

	void GetMana()
	{
		if (capturedEnemies.Count >= 10)
		{
			var oldestEnemy = capturedEnemies.Keys
				.First();
			capturedEnemies.Remove(oldestEnemy);
			Debug.Log("<color=red>REMOVING ENEMIES</color>");
		}
		else if (Random.Range(0, 100) < 20)
		{
			capturedEnemies.Add(enemyType, Random.Range(1,20));
			Debug.Log("<color=green>ADDING ENEMIES</color>");			
		}
		int totalWaves = capturedEnemies
			.Sum(x => x.Value);
		Player_Currency.mana += totalWaves;
		Debug.Log("Mana after kill: " + Player_Currency.mana);
	}

	void GetBounty()
    {
		// "Normal", "Mass", "Boss", "Farm", "Special"
		switch (enemyType)
        {
            case "Normal":
				totalBounty = Random.Range(normalBounty - diceRollRange, normalBounty + diceRollRange);
				break;

			case "Mass":
				totalBounty = Random.Range(massBounty - diceRollRange, massBounty + diceRollRange);
				break;

			case "Boss":
				totalBounty = Random.Range(bossBounty - diceRollRange, bossBounty + diceRollRange);
				break;

			case "Farm":
				totalBounty = Random.Range(farmBounty - diceRollRange, farmBounty + diceRollRange);
				break;

			case "Special":
				totalBounty = Random.Range(specialBounty - diceRollRange, specialBounty + diceRollRange);
				break;
        }
		if (totalBounty < 0) totalBounty += diceRollRange;
		Player_Currency.money += totalBounty;
		Debug.Log("Gold after death: " + Player_Currency.money + " // Gold received: " + totalBounty);
		
	}

}
