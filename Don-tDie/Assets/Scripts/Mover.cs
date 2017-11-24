using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed;
	public int bounces;

	private Rigidbody2D rb;
	private Ray2D ActualDir;
	private int count;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();
		ActualDir = new Ray2D (transform.position, difference);
		rb.AddForce (ActualDir.direction * speed);
		count = 0;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.SendMessage ("GameOver");
		}
		else if(other.gameObject.CompareTag ("Shot")){
			rb.velocity =new Vector2(0.0f,0.0f);
			ActualDir = new Ray2D(-1*ActualDir.origin,-1*ActualDir.direction);
			speed = 1.5f * speed;
			rb.AddForce (ActualDir.direction * speed);
			count++;
		}
		else if(other.gameObject.CompareTag("Background")){
			//ActualDir.direction.x rappresenta il coseno
			//ActualDir.direction.y rappresenta il seno
			rb.velocity =new Vector2(0.0f,0.0f);
			int quarter = CheckQuarter ();
			int collider = CheckCollider (other);

			switch (quarter)
			{
			case 1:
				{
					if (collider == 1) {
						float angle = Mathf.Asin (ActualDir.direction.y);
						angle = Mathf.PI / 2 + angle;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q2
					} 
					else if (collider == 2){
						float angle = Mathf.Asin (ActualDir.direction.y);
						angle =  angle- Mathf.PI / 2 ;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q4
					} 
					else
						print("errore");//errore
					break;
				}
			case 2:
				{
					if(collider==2){
						float angle = Mathf.Asin (ActualDir.direction.y);
						angle = angle + Mathf.PI;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q3
					} 
					else if(collider==3){
						float angle = Mathf.Acos (ActualDir.direction.x);
						angle =angle - Mathf.PI/2 ;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q1
					} 
					else
						print("errore");//errore
					break;
				}
			case 3:
				{
					if(collider==3){
						float angle = Mathf.Acos (ActualDir.direction.x);
						angle = angle + Mathf.PI;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q4
					} 
					else if(collider==4){
						float angle = Mathf.Asin (ActualDir.direction.y);
						angle =  angle - Mathf.PI ;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q2
					} 
					else 
						print("errore");//errore
					break;
				}
			default://quadrante 4
				{
					if(collider==4){
						float angle = Mathf.Asin (ActualDir.direction.y);
						angle = Mathf.PI / 2 + angle;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q1
					} 
					else if(collider==1){
						float angle = Mathf.Asin (ActualDir.direction.y);
						angle = angle-Mathf.PI / 2;
						Vector2 vett = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
						ActualDir = new Ray2D (transform.position, vett);
						Bounce (other);
						//nuova direzione q3
					} 
					else
						print("errore");//errore
					break;
				}
			}
		}
	}

	//Metodo che restituisce il quadrante in cui giace la direzione del proiettile
	int CheckQuarter(){
		if (ActualDir.direction.x >= 0)
			if (ActualDir.direction.y >= 0)
				return 1;
			else
				return 4;
		else if (ActualDir.direction.y >= 0)
				return 2;
			else
				return 3;
	}

	//Metodo che restituisce il collider del Background colpito
	int CheckCollider(Collider2D other){
		if (other.offset.x > 0)
			return 1; //collider di destra
		else if (other.offset.y < 0)
			return 4; //collider inferiore
		else if (other.offset.x < 0)
			return 3; //collider di sinistra
		else if (other.offset.y > 0)
			return 2; //collider superiore
		else
			return 0; //errore
	}

	void Bounce(Collider2D other){
		speed = 1.5f * speed;
		rb.AddForce (ActualDir.direction * speed);
		if (count == bounces) {
			Destroy (this.gameObject);
			GameObject pl = GameObject.FindGameObjectWithTag ("Player");
			pl.SendMessage ("AddPoint");
		}
		else
			count++;
	}

}
