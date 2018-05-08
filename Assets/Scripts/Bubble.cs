using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bubble : MonoBehaviour {

	[Flags]
	public enum Type {
		None = 0x0,
		Blue = 0x1,
		Green = 0x2,
		Purple = 0x4,
		Red = 0x8,
		Yellow = 0x16
	}

	// variables
	public Type bubbleType;
	public bool isDestroyed = false;
	public bool isPlaced = false;
	public bool isGameOver = false;
	public GameObject explosion = null;

	// components
	private GameController _controller = null;
	private Node node = null;


	// Use this for initialization
	void Awake () {
		_controller = GameObject.Find("Director").GetComponent<GameController>();
	}

	// NOTE: this is ugly but whatever
	void Update() {
		if (node != null) {
            if (isHanging()) {
				PrepareDestruction();
            }
        }
	}

    void OnCollisionEnter(Collision col) {
		if (isDestroyed || isPlaced) {
			return;
		}

        if (col.gameObject.layer == LayerMask.NameToLayer("Boundary")) {
            if (col.gameObject.tag == "Ceilling") {
                // adjust physics
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Bubbles")) {
            // adjust physics
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

	void OnTriggerStay(Collider col) {
		if (col.gameObject.layer == LayerMask.NameToLayer("Boundary")) {
			if (col.gameObject.tag == "Death" && isPlaced) {
				EventManager.TriggerEvent("GameOver");
			}
		}
	}

    List<Bubble> GetNeighbors() {
        int mask = (1 << 9);
        Collider[] arr = Physics.OverlapSphere(transform.position, 0.25f, mask);

        List<Bubble> ret = new List<Bubble>();
        for (int i = 0; i < arr.Length; i++) {
            ret.Add(arr[i].gameObject.GetComponent<Bubble>());
        }

        return ret;
    }

    Stack<Bubble> CheckForCluster() {
        List<Bubble> neighbors = GetNeighbors();
        Stack<Bubble> visited = new Stack<Bubble>();
        Stack<Bubble> mainStack = new Stack<Bubble>();

        for (int i = 0; i < neighbors.Count; ++i) {
            if (neighbors[i].bubbleType == this.bubbleType) {
                mainStack.Push(neighbors[i]);
            }
        }

        while (mainStack.Count > 0) {
            Bubble b = mainStack.Pop();
            visited.Push(b);
            neighbors = b.GetNeighbors();
            for (int i = 0; i < neighbors.Count; ++i) {
                if (neighbors[i].bubbleType == b.bubbleType) {
                    if (!mainStack.Contains(neighbors[i]) && !visited.Contains(neighbors[i])) {
                        mainStack.Push(neighbors[i]);
                    }
                }
            }
        }

        return visited;
    }

    bool isHanging() {
        List<Bubble> neighbors = GetNeighbors();
        Stack<Bubble> visited = new Stack<Bubble>();
        Stack<Bubble> mainStack = new Stack<Bubble>();

        for (int i = 0; i < neighbors.Count; ++i) {
            if (neighbors[i].node && neighbors[i].node.isTop) {
                return false;
            }

            mainStack.Push(neighbors[i]);
        }

        while (mainStack.Count > 0) {
            Bubble b = mainStack.Pop();

            if (b.node && b.node.isTop) {
                return false;
            }

            visited.Push(b);
            neighbors = b.GetNeighbors();

            for (int i = 0; i < neighbors.Count; ++i) {
                if (!mainStack.Contains(neighbors[i]) && !visited.Contains(neighbors[i])) {
                    mainStack.Push(neighbors[i]);
                }
            }
        }

        return true;
    }

	void PrepareDestruction() {
		if (isDestroyed) return;

		var rigidbody = gameObject.GetComponent<Rigidbody>();
		rigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX);
		rigidbody.useGravity = true;

		isDestroyed = true;
		isPlaced = false;

		_controller.BubbleRemoved(this);
	
		Destroy(gameObject, 1.0f);
	}

	void DestroyWithFury() {
		if (isDestroyed) return;

		if (explosion != null) {
			var fx = Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(fx, 1.0f);
		}

		isDestroyed = true;
		isPlaced = false;

		_controller.BubbleRemoved(this);

		Destroy(gameObject);
	}

	public void SnapToGrid(Node node) {
        this.node = node;
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

		_controller.BubbleAdded(this);

		if (isPlaced) return;
		isPlaced = true;

	    // were on the grid, so check for neighbors
	    Stack<Bubble> cluster = CheckForCluster();

		if (cluster.Count >= 3) {
			DestroyCluster(cluster);
	    }

		// Send Event
		EventManager.TriggerEvent("Reload");
    }

    public void ThrowBubble(float angleRad) {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector2(-Mathf.Sin(angleRad), Mathf.Cos(angleRad)) * 6f;
    }

    void DestroyCluster(Stack<Bubble> cluster) {
		while (cluster.Count > 0) {
			var bubble = cluster.Pop();

			if (explosion != null) {
				bubble.DestroyWithFury();
			} else {
				bubble.PrepareDestruction();
			}
		}
    }
}
