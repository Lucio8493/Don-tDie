using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text pointsLabel;
	public Text gameOverLabel;
	public Button restart;

	private Rigidbody2D rb;
	private int points;

	//Metodi per il settaggio dello stato della classe in fase di testing
	public void Construct(float speed, int points, Text pointsLabel, Text gameOverLabel, Button restart){
		this.speed = speed;
		this.points = points;
		this.pointsLabel = pointsLabel;
		this.gameOverLabel = gameOverLabel;
		this.restart = restart;
	}
	public void Construct(float speed, int points){
		this.speed = speed;
		this.points = points;
	}


	public int GetPoints(){
		return points;
	}

    public float GetPosX()
    {
        return transform.position.x;
    }

    public float GetPosY()
    {
        return transform.position.y;
    }


	// Metodo avviatto all'inizializzazione dell'oggetto
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		points = 0;
		pointsLabel.text = "Punti: " + points.ToString();
		gameOverLabel.gameObject.SetActive (false);
		restart.gameObject.SetActive (false);
		Button btn = restart.GetComponent<Button> ();
		btn.onClick.AddListener (Reset);
    }

    // FixedUpdate e' la funzione chiamata ogni frame che viene solitamente utilizzata per agire sugli oggetti
	// in particolare quando devono essere spostati e quindi va applicata una forza al loro corpo rigido
    void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		rb.velocity = movement*speed;

		/* Per Sistemi questa parte di codice non serve, è quella che gestisce la rotazione della navicella in base a dove si punta col mouse
		Vector3 difference;
		try{
			difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
		}
		catch(System.NullReferenceException e){
			difference = new Vector3 (4, 4, 0);
		}
		difference.Normalize();
		float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotation_z-90f);
		*/
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
	}
}
