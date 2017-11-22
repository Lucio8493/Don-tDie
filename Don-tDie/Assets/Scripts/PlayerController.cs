using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public Text pointsLabel;
	public Text gameOverLabel;
	public Button restart;

	private Rigidbody2D rb;
	private float nextFire;
	private int points;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		points = 0;
		pointsLabel.text = "Punti: " + points.ToString();
		gameOverLabel.gameObject.SetActive (false);
		restart.gameObject.SetActive (false);
		Button btn = restart.GetComponent<Button> ();
		btn.onClick.AddListener (Reset);
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

	void AddPoint(){
		points++;
		pointsLabel.text = "Punti: " + points.ToString();
	}

	void GameOver(){
		GameObject[] t = GameObject.FindGameObjectsWithTag("Shot");
		for(int i=0;i<t.Length;i++)
			Destroy (t [i]);
		gameObject.SetActive (false);
		gameOverLabel.gameObject.SetActive (true);
		restart.gameObject.SetActive (true);
	}


	void Reset(){
		SceneManager.LoadScene ("Main");
		/*rb.position.Set (gameOverLabel.gameObject.transform.position.x, gameOverLabel.gameObject.transform.position.x);
		rb.velocity.Set (0.0f, 0.0f);
		points = 0;
		pointsLabel.text = "Punti: " + points.ToString();
		gameObject.SetActive (true);
		gameOverLabel.gameObject.SetActive (false);
		restart.gameObject.SetActive (false);*/
	}
}
