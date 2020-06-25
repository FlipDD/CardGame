using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipper : MonoBehaviour
{
	// public
	[Header("Card images")]
	public List<Sprite> frontImages;

	[Header("Prefabs")]
	public GameObject cardPrefab;

	[Header("Variables")]
	public float flippingTime = 6.0f;

	// private
	private ScoreCounter scoreCounter;
	private Card flippedCard;
	private bool wasCardFlipped;

	private enum State { NoFlip, SecondFlip }
	private State state;

    void Awake() => scoreCounter = FindObjectOfType<ScoreCounter>();

    void Start() 
	{
		state = State.NoFlip;
		StartCoroutine(DealCards());
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
			yield return new WaitForSeconds(0.015f);
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
		yield return new WaitForSeconds(0.1f);
		foreach (Transform child in transform)
			StartCoroutine(FlipCard(child, false));

		// Hide the cards
		yield return new WaitForSeconds(3f);
		foreach (Transform child in transform)
			StartCoroutine(FlipCard(child, false));

		enabled = true;
		scoreCounter.enabled = true;
	}

	void Update () 
	{
		if((Input.GetMouseButtonDown(0) || Input.touchCount > 0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit.collider != null) 
				ValidateFlip(hit.collider);
		}
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
				case State.NoFlip:
					flippedCard = card;
					state = State.SecondFlip;
					StartCoroutine(FlipCard(cardTransform, false));
				break;

				case State.SecondFlip:
					if (flippedCard != card)
					{
						StartCoroutine(FlipCard(cardTransform, false));

						// If the ID's don't match, flip the cards back, otherwise add a pair to the score
						if (card.id != flippedCard.id)
							StartCoroutine(WaitToFlipBack(cardTransform, flippedCard.transform));
						else
							AddPair(card, flippedCard);

						state = State.NoFlip;
					}
				break;

				default:
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
		StartCoroutine(FlipCard(firstCard, false));
		StartCoroutine(FlipCard(secondCard, false));
		enabled = true;
	}


	IEnumerator FlipCard(Transform card, bool uncover)
	{
		float maxAngle = uncover ? 180 : 0; 
		float minAngle = uncover ? 0 : 180;

		float t = 0f;
		bool uncovered = false;
		while(t < 1f) 
		{
			t += Time.deltaTime * flippingTime;

			float angle = Mathf.LerpAngle(minAngle, maxAngle, t);
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
		yield return 0;
	}
}

		// if (!card.foundPair)
		// {
		// 	if (flippedCard != null)
		// 	{
		// 		if (cardTransform != flippedCard.transform)
		// 		{
		// 			Debug.Log("Buhhh");
		// 			// If the name is the same 
		// 			StartCoroutine(FlipCard(cardTransform, false));

		// 			if (card.id == flippedCard.id)
		// 			{
		// 				card.foundPair = true;
		// 				flippedCard.foundPair = true;
		// 				scoreCounter.numberOfPairs++;
		// 				Debug.LogError("Found a pair");
		// 			}
		// 			else
		// 			{

		// 			}
		// 		}
		// 	}
		// 	else
		// 	{
		// 		flippedCard = card;
		// 		if (card.transform != flippedCard.transform)
		// 			StartCoroutine(FlipCard(cardTransform, false));
		// 	} 
		//}