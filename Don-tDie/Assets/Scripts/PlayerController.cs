using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

	private Rigidbody2D rb;
	private float nextFire;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		rb.velocity = movement*speed;

		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();
		float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotation_z-90f);
	}

	void Update(){
		if (Time.time > nextFire) { 

			nextFire = Time.time + fireRate; 
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation); 

		}

	}

}
