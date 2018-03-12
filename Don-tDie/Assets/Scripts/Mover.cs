using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// nb, colpo e shot vengono usati come sinonimo e si riferiscono al prefab Shot
public class Mover : MonoBehaviour {

	public float speed; // la velocità iniziale del colpo, se si vuole modificare il valore lo si trova in /Assets/Prefabs/Shot
    public int bounces; // i rimbalzi che il colpo deve fare prima di essere eliminato e scomparire dal gioco, se si vuole modificare il valore lo si trova in /Assets/Prefabs/Shot

	private Rigidbody2D rb;
	private int count; // i rimbalzi che il colpo ha gia fatto
    private Vector2 difference; // il vettore della direzione del colpo, mi servirà poi per andare a calcolare la direzione dopo il rimbalzo



    public Vector2 GetDifference() // get che serve per il testing, restitisce il vettore direzione
    {
        return difference;
    }

   


	//Metodi per il settaggio dello stato  nellla fase di testing
	public void Construct ( float speed, int bounces,  int count, Vector2 difference){
		this.speed = speed;
		this.bounces = bounces;
	//	this.ActualDir = ActualDir;
		this.count = count;
		this.difference = difference;
	}

	public void Construct ( float speed, int bounces, int count){
		this.speed = speed;
		this.bounces = bounces;
		this.count = count;
	}

	public int GetCount(){
		return count;
	}

    // Metodo avviatto all'inizializzazione dell'oggetto
    void Start () {
		rb = GetComponent<Rigidbody2D> ();
		//Viene catturata l'eccezione per consentire di eseguire dei test automatici
		try{
			difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
		}
		catch(System.NullReferenceException e){
			difference = new Vector2(4, 4);
		}

		difference.Normalize();
		rb.AddForce (difference * speed);
		count = 0;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) { // in questo if entriamo se il colpo ("shot") collide con il giocatore (la navicella)
			other.gameObject.SendMessage ("GameOver");

        }
		else if(other.gameObject.CompareTag ("Shot")){ // in questo if entriamo se un colpo ("shot") collide con un altro colpo
			rb.velocity =new Vector2(0.0f,0.0f);
            difference = new Vector2(-1 * difference.x, -1*difference.y); // quando un colpo collide con un un altro va nella direzione opposta a quella in cui andava prima
            Bounce();


        }
		else if(other.gameObject.CompareTag("Background")){ // in questo if entriamo se un colpo ("shot") collide con uno dei quattro bordi dello schermo
            rb.velocity =new Vector2(0.0f,0.0f);
			int collider = CheckCollider (other);
            if (collider == 1 || collider == 3) // se  collider è uno o tre il colpo si è scontrato con il muro di destra o sinistra, in questo caso va invertita la componente x
            {
                difference = new Vector2(-1 * difference.x, difference.y);
            } else // se invece collider è due oppure quattro il colpo si è scontrato con un muro superiore o inveriore, in questo caso va cambiata la componente y 
            {
                difference = new Vector2(difference.x, -1 * difference.y);

            }

            Bounce();
            
        }
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

	void Bounce(){
		speed = 1.2f * speed;
		rb.AddForce (difference * speed);

		if (count >= bounces) {   // se count è uguale a "bounces" allora il colpo ha fatto abbastanza rimbalzi e può essere eliminato. (per sapere di più su bounces vedere sopra)
			Destroy (this.gameObject);
			GameObject pl = GameObject.FindGameObjectWithTag ("Player");
			pl.SendMessage ("AddPoint");
		}
		else // altrimenti il colpo non ha fatto abbastanza rimbalzi, quindi rimane in gioco e viene incrementato il suo count per tenere traccia di quante volte ha rimbalzato
			count++;
	}

}
