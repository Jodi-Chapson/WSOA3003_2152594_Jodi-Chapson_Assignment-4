using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Reference for the state machine/combat system from Brackey's "Turn-Based Combat in Unity" https://www.youtube.com/watch?v=_1pz_ohupPs&t
public enum BattleState { OUTCOMBAT, START, PLAYERTURN, ENEMYTURN, END }

public class StateMachine : MonoBehaviour
{
	[Header("Player and Enemy References")]
	public GameObject player, enemy;
	public Chara_Info playerinfo, enemyinfo;
	public Transform playerPos, enemypos, camplacement;
	public Vector3 playerlastPos, enemylastPos, camlastpos;
	public CharacterMovement playermove;
	public Slider enemyhealth;
	public Image enemydamagetype;
	public Image enemyhead;

	[Header("Menus and UI")]
	public BattleMenu BM;
	public GameObject endscreen;
	public MiscScript misc;
	public GameObject coinUI;
	public Text coincounter;
	public int TotalCoins;
	public float currentCD;
	public float totalCD;
	public GameObject alt1, alt2;
	public GameObject pmenu;
	public bool paused;
	public bool ended;

	[Header("Misc")]
	public BattleState state;
	public Animator fadeanim;
	public GameObject key, support;
	public GameObject door;
	public Sprite openeddoor;
	public Support supp;
	public Sprite fire, normaldmg;
	public Sprite eyehead, firehead;
	public CamController cam;
	public GameObject DamFig;
	public int healthbonus;

	public int starthealth;
	public int finishhealth;





	public void Start()
	{
		state = BattleState.OUTCOMBAT;
		paused = false;
		
	}

	public IEnumerator BattleSetup()
	{
		yield return new WaitForSeconds(0.05f);

		fadeanim.Play("Toblack", 0, 0.0f);

		playermove.playeranim.SetInteger("State", 0);

		BM.ToggleONBATTLEHUD();
		if(enemyinfo.type == 1)
		{
			enemydamagetype.GetComponent<Image>().sprite = normaldmg;
			enemyhead.GetComponent<Image>().sprite = eyehead;
			

		}
		else if (enemyinfo.type == 2)
		{
			enemydamagetype.GetComponent<Image>().sprite = fire;
			enemyhead.GetComponent<Image>().sprite = firehead;
			
		}

		starthealth = playerinfo.currentHP;

		cam.CamFollow = false;
		playerlastPos = player.transform.position;
		enemylastPos = enemy.transform.position;
		camlastpos = cam.transform.position;
		player.transform.position = playerPos.transform.position;
		enemy.transform.position = enemypos.transform.position;

		enemyhealth.maxValue = enemy.GetComponent<Chara_Info>().maxHP;
		enemyhealth.value = enemy.GetComponent<Chara_Info>().currentHP;

		cam.transform.position = camplacement.transform.position;
		playermove.sprite.transform.eulerAngles = new Vector3(0, 0, 0);
		
		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		playermove.canmove = false;

		yield return new WaitForSeconds(0.1f);

		fadeanim.Play("Fadeout", 0, 0.0f);

		yield return new WaitForSeconds(0.5f);

		state = BattleState.PLAYERTURN;

		PlayerTurn();
	}

	public void DamagePop(int target)
	{
		if (target == 0 || target == 1 || target == 4)
		{
			GameObject dam = Instantiate(DamFig, enemypos.position, Quaternion.identity);
			dam.transform.position = new Vector3(dam.transform.position.x - 1, dam.transform.position.y + 1, dam.transform.position.z);
			if (target == 0)
			{
				int bonusdam = playerinfo.damage + 2;
				dam.GetComponentInChildren<TextMeshPro>().text = bonusdam.ToString();

				dam.GetComponentInChildren<TextMeshPro>().color = Color.blue;
			}
			else if (target == 1)
		    {
				int bonusdam = (playerinfo.damage + 2) + (int) (playerinfo.damage + 2)/2;
				dam.GetComponentInChildren<TextMeshPro>().text = bonusdam.ToString();
				dam.GetComponentInChildren<TextMeshPro>().color = Color.blue;
			}
			else if (target == 4)
			{
				int bonusdam = playerinfo.damage ;
				dam.GetComponentInChildren<TextMeshPro>().text = bonusdam.ToString();

				
			}

			Destroy(dam, 3);
			

		}
		
		else if (target == 2)
		{
			GameObject dam = Instantiate(DamFig, playerPos.position, Quaternion.identity);
			dam.transform.position = new Vector3(dam.transform.position.x + 1, dam.transform.position.y + 2, dam.transform.position.z);
			dam.GetComponentInChildren<TextMeshPro>().text = enemyinfo.damage.ToString();

			if (enemyinfo.type == 2)
			{
				dam.GetComponentInChildren<TextMeshPro>().color = Color.red;
			}

			Destroy(dam, 2);

		}
		else if (target == 3)
		{
			GameObject heal = Instantiate(DamFig, playerinfo.gameObject.transform.position, Quaternion.identity);

			heal.GetComponentInChildren<TextMeshPro>().text = (healthbonus).ToString();
			heal.GetComponentInChildren<TextMeshPro>().color = Color.green;
		}

		
	}

	public void PlayerTurn()
	{
		BM.ToggleBaseorBack();
		BM.Arrow.SetActive(true);
		BM.Arrow.transform.localPosition = new Vector2(-420, BM.Arrow.transform.localPosition.y);


	}

	public void ToggleFlee()
	{
		StartCoroutine(Flee());
	}
	public IEnumerator Flee ()
	{
		yield return new WaitForSeconds (0.2f);

		BM.Arrow.SetActive(false);
		fadeanim.Play("Toblack", 0, 0.0f);

		BM.ToggleOFFALLMenus();
		BM.ToggleOFFBATTLEHUD();

		//player.transform.position = playerlastPos;
		enemy.transform.position = enemylastPos;
		playermove.canmove = true;
		cam.CamFollow = true;
		cam.transform.position = camlastpos;

		Vector3 newpos = new Vector3((-enemylastPos.x + 2*playerlastPos.x), (-enemylastPos.y + 2*playerlastPos.y), 0);
		//playerinfo.currentHP -= 1;
		
		//a basic highschool maths formula that took me way too long to get haha
		player.transform.position = newpos;
		
		yield return new WaitForSeconds(0.1f);

		fadeanim.Play("Fadeout", 0, 0.0f);




		yield return new WaitForSeconds(0.5f);
		state = BattleState.OUTCOMBAT;



	}
	
	public void toggleattack(int type)
	{
		if (type == 1)
		{
			StartCoroutine(PlayerAttack()); 
		}
		else if (type == 2)
		{
			StartCoroutine(WaterAttack());
		}


		BM.Arrow.SetActive(false);

	}

	public IEnumerator PlayerAttack()
	{
		BM.ToggleOFFALLMenus();
		
		
		//yield return new WaitForSeconds(0.5f);

		playermove.playeranim.Play("attack", 0, 0.0f);

		yield return new WaitForSeconds(0.8f);

		DamagePop(4);
		enemyinfo.currentHP -= playerinfo.damage;
		enemyhealth.value = enemy.GetComponent<Chara_Info>().currentHP;

		yield return new WaitForSeconds(1f);


		if (enemyinfo.currentHP <= 0)
		{
			state = BattleState.END;
			StartCoroutine(EndFight(1));

		}
		else
		{
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyAttack());
		}

	}

	public IEnumerator WaterAttack()
	{
		BM.ToggleOFFALLMenus();


		//yield return new WaitForSeconds(0.5f);
		playermove.playeranim.Play("attack", 0, 0.0f);

		yield return new WaitForSeconds(0.8f);


		if (enemyinfo.type == 1) //base enemy
		{
			enemyinfo.currentHP -= (playerinfo.damage + 2)*1;
			enemyhealth.value = enemy.GetComponent<Chara_Info>().currentHP;
			DamagePop(0);
		}
		else //elemental enemy
		{
			enemyinfo.currentHP -= (playerinfo.damage +2) + (int) (playerinfo.damage +2 )/2;
			enemyhealth.value = enemy.GetComponent<Chara_Info>().currentHP;
			DamagePop(1);
		}


		yield return new WaitForSeconds(1f);


		if (enemyinfo.currentHP <= 0)
		{
			state = BattleState.END;
			StartCoroutine(EndFight(1));

		}
		else
		{
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyAttack());
			
			
		}
	}


	public IEnumerator EnemyAttack()
	{
		BM.Arrow.transform.localPosition = new Vector2(420, BM.Arrow.transform.localPosition.y);
		BM.Arrow.SetActive(true);

		yield return new WaitForSeconds(1f);

		enemyinfo.GetComponentInChildren<Animator>().Play("attack", 0, 0.0f);
		yield return new WaitForSeconds(0.8f);
		DamagePop(2);
		playerinfo.currentHP -= enemyinfo.damage;
		playermove.playeranim.Play("flinch", 0, 0.0f);


		yield return new WaitForSeconds(1f);


		if (playerinfo.currentHP <= 0)
		{
			state = BattleState.END;
			StartCoroutine(EndFight(2));
		}
		else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}


	}



	public IEnumerator EndFight(int conclusion)
	{
		BM.Arrow.SetActive(false);

		yield return new WaitForSeconds(0.3f);

		fadeanim.Play("Toblack", 0, 0.0f);

		BM.ToggleOFFALLMenus();
		BM.ToggleOFFBATTLEHUD();

		playermove.canmove = true;
		if (conclusion == 1)
		{
			if (enemyinfo.type == 1)
			{
				//drop elemental
				
				BM.Togglespecialskill();
				misc.UpgradeWater();


				StartCoroutine(CoinsAnimated(1));
				

			}
			else if (enemyinfo.type == 2)
			{
				misc.ToggleKey();

				StartCoroutine(CoinsAnimated(2));


				//endscreen.SetActive(true);

			}

			Vector3 newpos = new Vector3((-enemylastPos.x + 2 * playerlastPos.x), (-enemylastPos.y + 2 * playerlastPos.y), 0);
			
			
			player.transform.position = newpos;

			//player.transform.position = playerlastPos;
			Destroy(enemy);
			playermove.canmove = true;
			cam.CamFollow = true;
			cam.transform.position = camlastpos;


			finishhealth = playerinfo.currentHP;


			healthbonus = (int)((starthealth - finishhealth)/2);
			if (healthbonus >= 0)
			{
				playerinfo.currentHP += healthbonus;
				
				if (playerinfo.currentHP >= playerinfo.maxHP)
			{
				playerinfo.currentHP = playerinfo.maxHP;
			}
			DamagePop(3);
			}
			
			

			
			


		}
		else if (conclusion == 2)
		{

			endscreen.SetActive(true);
			alt2.SetActive(true);
			ended = true;
		}



		

		yield return new WaitForSeconds(0.1f);

		fadeanim.Play("Fadeout", 0, 0.0f);

		yield return new WaitForSeconds(0.5f);


		state = BattleState.OUTCOMBAT;


		
		


	}

	public void UnlockDoor()
	{
		door.GetComponent<SpriteRenderer>().sprite = openeddoor;

		
	}

	public IEnumerator End()
	{
		yield return new WaitForSeconds(0.5f);
		endscreen.SetActive(true);
		alt1.SetActive(true);
		ended = true;
	}


	public IEnumerator CoinsAnimated(int type)
	{
		yield return new WaitForSeconds(0.5f);

		if (type == 1)
	
		{
			AddCoins(1);

			yield return new WaitForSeconds(0.3f);

			AddCoins(2);

			yield return new WaitForSeconds(0.3f);

			AddCoins(2);

			yield return new WaitForSeconds(0.2f);

			AddCoins(2);
		}
		else 
		{
			AddCoins(2);

			yield return new WaitForSeconds(0.3f);

			AddCoins(3);

			yield return new WaitForSeconds(0.3f);

			AddCoins(3);

			yield return new WaitForSeconds(0.2f);

			AddCoins(2);

			
		}




	}


	public void AddCoins(int add)
	{
		coinUI.SetActive(true);
		currentCD = totalCD;
		TotalCoins = add + TotalCoins;


		coincounter.text = TotalCoins.ToString();

	}

	public void Update()
	{

		if (coinUI.activeSelf == true)
		{
			currentCD -= 0.1f;

			if (currentCD <= 0)
			{
				coinUI.SetActive(false);
			}
		}


		if (Input.GetKeyDown("escape"))
		{
			if (!ended)
			{
				if (!paused)
				{
					Paused(1);
				}
				else
				{
					Paused(2);
				}
			}
		}



	}

	public void Paused(int toggle)
	{
		if (toggle == 1)
		{ pmenu.SetActive(true);
			playermove.canmove = false;
			paused = true;
		}
		else if (toggle == 2)
		{
			pmenu.SetActive(false);
			playermove.canmove = true;
			paused = false;
		}
	}





}
