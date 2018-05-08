using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerareGrid : MonoBehaviour {

    public GameObject node;
    public float yoffset = 0.34f;

	public List<Node> nodes = new List<Node>();

	void Awake() {
        GenerateHexGrid();
    }

    void GenerateHexGrid() {
        Vector3 gridpositionrow1 = new Vector3(-1.3f, 1.75f, 0);
        Vector3 gridpositionrow2 = new Vector3(-1.1f, 1.41f, 0);
        float increment = 0;

        for (int k = 0; k < 6; k++) {
            GameObject temp = null;
            for (int i = 0; i < 8; i++) {
                temp = Instantiate(node, gridpositionrow1, transform.rotation);
                gridpositionrow1 += new Vector3(0.372f, 0, 0);

                if (k == 0)
                {
                    temp.GetComponent<Node>().isTop = true;
                }

				if (k == 5) {
					temp.GetComponent<Node>().isBottom = true;
				}

				// add node to list
				nodes.Add(temp.GetComponent<Node>());
            }

            for (int i = 0; i < 7; i++) {
                temp = Instantiate(node, gridpositionrow2, transform.rotation);
                gridpositionrow2 += new Vector3(0.372f, 0, 0);

                if (k == 5) {
                    temp.GetComponent<Node>().isBottom = true;
                }

				// add node to list
				nodes.Add(temp.GetComponent<Node>());
            }

            increment += yoffset;
            gridpositionrow1 = new Vector3(-1.3f, 1.75f - increment * 2, 0);
            gridpositionrow2 = new Vector3(-1.1f, 1.41f - (increment * 2), 0);
	
        }
    }
}
