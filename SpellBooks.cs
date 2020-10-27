using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBooks : MonoBehaviour
{
	private static SpellBooks instance;
	public static SpellBooks MyInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<SpellBooks>();
			}
			return instance;
		}
	}



	[SerializeField]
	private Image castingBar;

	[SerializeField]
	private Spell[] spells;

	[SerializeField]
	private Text currentSpell;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private CanvasGroup canvasGroup;

	private Coroutine spellRoutine;

	private Coroutine fadeRoutine;





	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public Spell CastSpell(string spellName)

	{

		Spell spell = Array.Find(spells, x => x.MyName == spellName);

		currentSpell.text = spell.MyName;
		castingBar.fillAmount = 0;

		castingBar.color = spell.MyBarColor;
		icon.sprite = spell.MyIcon;
		spellRoutine = StartCoroutine(Progress(spell));

		fadeRoutine = StartCoroutine(FadeBar());
		return spell;
	}

	private IEnumerator Progress(Spell spell)
	{
		float timeLeft = Time.deltaTime;

		float rate = 1.0f / spell.MyCastTime;

		float progress = 0.0f;

		while (progress <= 1.0)
		{
			castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
			progress += rate * Time.deltaTime;

			timeLeft += Time.deltaTime;
			yield return null;
		}

		StopCasting();
	}

	private IEnumerator FadeBar()
	{ 
	

	float rate = 1.0f / 0.25f;

	float progress = 0.0f;

		while(progress <=1.0)
        {
			canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
			progress += rate* Time.deltaTime;
		}
	
		yield return null;
		
	}



	public void StopCasting()
	{
		if (fadeRoutine !=null)
        {
			StopCoroutine(fadeRoutine);
			canvasGroup.alpha = 0;
			fadeRoutine = null;
        }
		
		if (spellRoutine != null)
		{
			StopCoroutine(spellRoutine);
			spellRoutine = null;

		}

	}

	public Spell GetSpell(string spellName)
    {
		Spell spell = Array.Find(spells, x => x.MyName == spellName);
		return spell;
    }
}