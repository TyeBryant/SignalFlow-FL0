using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineShape : MonoBehaviour {

    public bool call = false;

    public GameObject[] shape;
    public GameObject triangle;

    public GameObject tri1;

    public float counter = 0;
    public bool active = false;
    Node nodes;

    int number = 2;

    public Vector2 positionA = new Vector2();
    public Vector2 positionB = new Vector2();

    // Use this for initialization
    void Start () {

    
	}
	
	// Update is called once per frame
	void Update () {

       

            counter += Time.deltaTime;



            if (counter > 2.0f)
            {

                counter = 0;
                Destroy(this.gameObject);
                
            }

            transform.position = Vector2.Lerp(positionA, positionB, counter / 2.0f);
        }
	
}
