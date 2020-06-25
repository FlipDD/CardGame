using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipper : MonoBehaviour
{
	[Header("Card images")]
	public List<Sprite> frontImages;

	[Header("Prefabs")]
	public GameObject cardPrefab;

	[Header("Variables")]
	public float flippingTime = 6.0f;

	void Start () 
	{
		StartCoroutine(DealCards());
	}

	IEnumerator DealCards()
	{
		frontImages.Shuffle();
		int index = 0;
		for (int i = 1; i < 24 + 1; i++)
		{
			GameObject card = Instantiate(cardPrefab, transform);
			var frontCard = card.transform.GetChild(0);
			frontCard.GetComponent<SpriteRenderer>().sprite = frontImages[index];
			index++;
			if (index == 12)
			{
				index = 0;
				frontImages.Shuffle();
			}
			yield return new WaitForSeconds(0.15f);
		}

		StartCoroutine(StartGame());
	}

	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(3);

		for (int i = 0; i < transform.childCount; i++)
			StartCoroutine(FlipCard(transform.GetChild(i), true));
	}

	void Update () 
	{
		if((Input.GetMouseButtonDown(0) || Input.touchCount > 0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit.collider != null) 
				StartCoroutine(FlipCard(hit.collider.gameObject.transform, false));
		}
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
