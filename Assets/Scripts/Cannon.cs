using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	// instantiated objects
	public GameObject currBubble;
	public GameObject nextBubble;

	public float shotDelay = 3.0f;
	private float currDelay = 0.0f;

	public int shotsPerShift = 5;
	private int shots = 0;

	private bool canFire = false;

	// components
	private GameController _controller = null;
	private ParticleSystem[] _particleSystems = null;

	// children
	private Transform _current;
	private Transform _next;

	void Awake() {
		// components
		_particleSystems = GetComponentsInChildren<ParticleSystem>();
		_controller = GameObject.Find("Director").GetComponent<GameController>();

		// transforms
		_current = gameObject.transform.GetChild(0);
		_next = GameObject.Find("NextBubble").transform;

		foreach(var p in _particleSystems) {
			p.Stop();
		}

		// initialization
		currDelay = shotDelay;
	}

	void OnEnable() {
		EventManager.StartListening("Reload", Reload);
		EventManager.StartListening("GameStart", Reload);
	}

	void OnDisable() {
		EventManager.StopListening("Reload", Reload);
		EventManager.StopListening("GameStart", Reload);
	}
		
//    void Start() {
//        Reload();
//    }

    // Update is called once per frame
    void Update() {
		currDelay -= Time.deltaTime;
		if (currDelay <= 0.0f) {
			Fire();
			return;
		}

        if (Input.GetKey(KeyCode.LeftArrow)) {
            if ((transform.eulerAngles.z <= 50) || (transform.eulerAngles.z <= 360 && transform.eulerAngles.z >= 306)) {
                transform.Rotate(Vector3.forward * 2);
            }

        } else if (Input.GetKey(KeyCode.RightArrow)) {
            if ((transform.eulerAngles.z <= 55) || (transform.eulerAngles.z <= 360 && transform.eulerAngles.z >= 311)) {
                transform.Rotate(-Vector3.forward * 2);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Fire();
        }
    }

    void Fire() {
		if (!canFire) return;
		canFire = false;

		foreach(var p in _particleSystems) {
			p.Play();
		}

		currBubble.GetComponent<SphereCollider>().enabled = true;
		currBubble.GetComponent<Bubble>().ThrowBubble(transform.eulerAngles.z * Mathf.Deg2Rad);
		currBubble.transform.parent = null;
		currBubble = null;

		shots++;
		currDelay = shotDelay;
    }

    void Reload() {
		if (shots >= shotsPerShift) {
			EventManager.TriggerEvent("Shift");
			shots = 0;
		}

        while (true) {
			if (currBubble == null) {
				if (nextBubble != null) {
					currBubble = nextBubble;
					currBubble.transform.parent = _current;
					currBubble.transform.position = _current.position;
				}

				nextBubble = Instantiate(_controller.GetValidBubble(), _next.position, Quaternion.identity, _next);
            }

			if (currBubble != null && nextBubble != null) {
				currBubble.GetComponent<SphereCollider>().enabled = false;
				nextBubble.GetComponent<SphereCollider>().enabled = false;

				canFire = true;
                return;
            }
        }
    }
}
