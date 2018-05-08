using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Node : MonoBehaviour {

    public bool isTop = false;
    public bool isBottom = false;

    private Bubble bubble = null;

    void OnTriggerStay(Collider col)
    {
        if (bubble != null) return;

		if (col.gameObject.GetComponent<Bubble>().isDestroyed) return;

        if (col.gameObject.layer == LayerMask.NameToLayer("Bubbles"))
        {
            if (col.gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
            {
                return;
            }

            if ((gameObject.transform.position - col.transform.position).magnitude < 0.24f)
            {
				Consume(col.gameObject.GetComponent<Bubble>());
            }
        }
    }

	public void Consume(Bubble b) {
		b.gameObject.transform.position = transform.position;

		bubble = b;
		bubble.SnapToGrid(this);
	}
}
