using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField]
	private GameObject[] bubbles;

	private List<GameObject> validBubbles = new List<GameObject>();

	private GenerareGrid _grid;

	private List<Bubble> currBubbles = new List<Bubble>();
	public Bubble.Type currTypes = Bubble.Type.None;

	public struct Stats {
		public int blue;
		public int green;
		public int purple;
		public int red;
		public int yellow;
		public int total;
	};

	public Stats stats = new Stats();

	void Awake() {
		_grid = gameObject.GetComponent<GenerareGrid>();
	}

	void Start() {
		var rnd = Random.Range(0, 5);
		var count = 0;
		for (int i = 0; i < 38; i++) {
			if (count >= 2) {
				var trnd = Random.Range(0, 5);
				if (trnd == rnd) {
					trnd = (trnd + 1) % 5;
				}
				rnd = trnd;
				count = 0;
			}
			count++;

			var temp = Instantiate(bubbles[rnd], _grid.nodes[i].transform.position, Quaternion.identity);
			Bubble b = temp.GetComponent<Bubble>();
			b.isPlaced = true;

			_grid.nodes[i].Consume(b);
		}


		EventManager.TriggerEvent("GameStart");
	}

	public GameObject GetValidBubble() {
		if (validBubbles.Count == 0) {
			return null;
		}

		return validBubbles[Random.Range(0, validBubbles.Count)];
	}

	public void BubbleAdded(Bubble b) {
		currBubbles.Add(b);
		currTypes |= b.bubbleType;

		switch (b.bubbleType) {
		case Bubble.Type.Blue: stats.blue++; if (!validBubbles.Contains(bubbles[0])) { validBubbles.Add(bubbles[0]); } break;
		case Bubble.Type.Green: stats.green++; if (!validBubbles.Contains(bubbles[1])) { validBubbles.Add(bubbles[1]); } break;
		case Bubble.Type.Purple: stats.purple++; if (!validBubbles.Contains(bubbles[2])) { validBubbles.Add(bubbles[2]); } break;
		case Bubble.Type.Red: stats.red++; if (!validBubbles.Contains(bubbles[3])) { validBubbles.Add(bubbles[3]); } break;
		case Bubble.Type.Yellow: stats.yellow++; if (!validBubbles.Contains(bubbles[4])) { validBubbles.Add(bubbles[4]); } break;
		}

		stats.total++;
	}

	public void BubbleRemoved(Bubble b) {
		currBubbles.Remove(b);

		// Send Event
		EventManager.TriggerEvent("ScoreUpdated");

		switch (b.bubbleType) {
		case Bubble.Type.Blue: stats.blue--; if (stats.blue <= 0 && validBubbles.Contains(bubbles[0])) { stats.blue = 0; validBubbles.Remove(bubbles[0]); } break;
		case Bubble.Type.Green: stats.green--; if (stats.green <= 0 && validBubbles.Contains(bubbles[1])) { stats.green = 0; validBubbles.Remove(bubbles[1]); } break;
		case Bubble.Type.Purple: stats.purple--; if (stats.purple <= 0 && validBubbles.Contains(bubbles[2])) { stats.purple = 0; validBubbles.Remove(bubbles[2]); } break;
		case Bubble.Type.Red: stats.red--; if (stats.red <= 0 && validBubbles.Contains(bubbles[3])) { stats.red = 0; validBubbles.Remove(bubbles[3]); } break;
		case Bubble.Type.Yellow: stats.yellow--; if (stats.yellow <= 0 && validBubbles.Contains(bubbles[4])) { stats.yellow = 0; validBubbles.Remove(bubbles[4]); } break;
		}

		stats.total--;

		if (stats.total <= 0) {
			EventManager.TriggerEvent("GameOver");
		}
	}

}
