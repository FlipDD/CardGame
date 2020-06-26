using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameController : MonoBehaviour
{
	// public
	[Header("Card images")]
	public List<Sprite> frontImages;

	[Header("Prefabs")]
	public GameObject cardPrefab;

	[Header("Variables")]
	public float flippingTime = 6.0f;

	// private
	private enum State { FirstFlip, SecondFlip }
	private State state;
	private ScoreCounter scoreCounter;
	private PauseMenuController pauseMenuController;
	private Card flippedCard;

    void Awake() 
	{
	 	scoreCounter = FindObjectOfType<ScoreCounter>();
		pauseMenuController = FindObjectOfType<PauseMenuController>();
	}

    void Start() 
	{
		state = State.FirstFlip;
		pauseMenuController.enabled = false;
		StartCoroutine(DealCards());
	}

	void Update () 
	{
		// Raycast to the card's colliders
		if((Input.GetMouseButtonDown(0) || Input.touchCount > 0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit.collider != null) 
				ValidateFlip(hit.collider);
		}
	}

    IEnumerator DealCards()
	{
		enabled = false;
		// Instantiate prefabs and set their sprites
		int index = 0;
		for (int i = 1; i < 24 + 1; i++)
		{
			GameObject card = Instantiate(cardPrefab, transform);
			var frontCard = card.transform.GetChild(0);
			frontCard.GetComponent<SpriteRenderer>().sprite = frontImages[index];
			card.GetComponent<Card>().id = frontImages[index].name;
			index++;
			if (index == 12)
				index = 0;
			yield return new WaitForSeconds(0.15f);
		}

		// List of children
		var children = new List<Transform>();
		foreach	(Transform child in transform)
			children.Add(child);
		
		// Shuffle and reorder (randomize)
		children.Shuffle();
		for (int i = 0; i < transform.childCount; i++)
			children[i].SetSiblingIndex(i);

		// Show cards for a few seconds and hide them again
		StartCoroutine(StartGame());
	}

	IEnumerator StartGame()
	{
		// Show the cards
		yield return new WaitForSeconds(1f);
		foreach (Transform child in transform)
			StartCoroutine(FlipCard(child));

		// Hide the cards
		yield return new WaitForSeconds(3f);
		foreach (Transform child in transform)
			StartCoroutine(FlipCard(child));

		// Enable all update methods
		yield return new WaitForSeconds(.5f);
		enabled = true;
		scoreCounter.enabled = true;
		pauseMenuController.enabled = true;
	}

	void ValidateFlip(Collider2D collider)
	{
		var cardTransform = collider.gameObject.transform;
		var card = cardTransform.GetComponent<Card>();

		// Only flip if we haven't found a pair already
		if (!card.foundPair)
		{
			
			switch (state)
			{
				case State.FirstFlip:
					flippedCard = card;
					state = State.SecondFlip;
					StartCoroutine(FlipCard(cardTransform));
					scoreCounter.AddMoveScore();
				break;

				case State.SecondFlip:
					if (flippedCard != card)
					{
						StartCoroutine(FlipCard(cardTransform));
						scoreCounter.AddMoveScore();
						state = State.FirstFlip;

						// If the ID's don't match, flip the cards back, otherwise add a pair to the score
						if (card.id != flippedCard.id)
							StartCoroutine(WaitToFlipBack(cardTransform, flippedCard.transform));
						else
							AddPair(card, flippedCard);
					}
				break;
			}	
		}
	}

	void AddPair(Card firstCard, Card secondCard)
	{
		firstCard.foundPair = true;
		secondCard.foundPair = true;
		scoreCounter.AddPairScore();
	}

	IEnumerator WaitToFlipBack(Transform firstCard, Transform secondCard)
	{
		enabled = false;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(FlipCard(firstCard));
		StartCoroutine(FlipCard(secondCard));
		enabled = true;
	}

	IEnumerator FlipCard(Transform card)
	{
		float t = 0f;
		bool uncovered = false;
		while(t < 1f) 
		{
			t += Time.deltaTime * flippingTime;
			float angle = Mathf.LerpAngle(180, 0, t);
			card.eulerAngles = new Vector3(0, angle, 0);

			if(((angle >= 90 && angle < 180) || (angle >= 270 && angle < 360) ) && !uncovered) 
			{
				uncovered = true;
				for(int i = 0; i < card.childCount; i++) 
				{
					var c = card.GetChild(i);
					c.GetComponent<SpriteRenderer>().sortingOrder *= -1;
					yield return null;
				}
			}
			yield return null;
		}
	}
}