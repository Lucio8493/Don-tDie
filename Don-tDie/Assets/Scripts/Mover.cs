using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// nb, colpo e shot vengono usati come sinonimo e si riferiscono al prefab Shot
public class Mover : MonoBehaviour {

	public float speed; // la velocità iniziale del colpo, se si vuole modificare il valore lo si trova in /Assets/Prefabs/Shot
    public int bounces; // i rimbalzi che il colpo deve fare prima di essere eliminato e scomparire dal gioco, se si vuole modificare il valore lo si trova in /Assets/Prefabs/Shot

	private Rigidbody2D rb;
	private Ray2D ActualDir;
	private int count; // i rimbalzi che il colpo ha gia fatto
    private Vector3 difference; // il vettore della direzione del colpo, mi servirà poi per andare a calcolare la direzione dopo il rimbalzo


	//Metodo per il settaggio dello stato  nellla fase di testing
	public void Construct ( float speed, int bounces, Ray2D ActualDir, int count, Vector3 difference){
		this.speed = speed;
		this.bounces = bounces;
		this.ActualDir = ActualDir;
		this.count = count;
		this.difference = difference;
	}

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody2D> ();
		//Viene catturata l'eccezione per consentire di eseguire dei test automatici
		try{
			difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
		}
		catch(System.NullReferenceException e){
			difference = new Vector3 (4, 4, 0);
		}
		difference.Normalize();
		ActualDir = new Ray2D (transform.position, difference);
		rb.AddForce (ActualDir.direction * speed);
		count = 0;


	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) { // in questo if entriamo se il colpo ("shot") collide con il giocatore (la navicella)
			other.gameObject.SendMessage ("GameOver");

        }
		else if(other.gameObject.CompareTag ("Shot")){ // in questo if entriamo se un colpo ("shot") collide con un altro colpo
			rb.velocity =new Vector2(0.0f,0.0f);
            difference = new Vector3(-1 * difference.x, -1*difference.y, difference.z); // quando un colpo collide con un un altro va nella direzione opposta a quella in cui andava prima
            ActualDir = new Ray2D(transform.position, difference);
            Bounce();


        }
		else if(other.gameObject.CompareTag("Background")){ // in questo if entriamo se un colpo ("shot") collide con uno dei quattro bordi dello schermo
            //ActualDir.direction.x rappresenta il coseno
            //ActualDir.direction.y rappresenta il seno

            rb.velocity =new Vector2(0.0f,0.0f);
			int collider = CheckCollider (other);
            switch (collider)
            {
                case 1: //in questo caso siamo nel bordo di destra, quello che deve cambiare è la componente x del vettore
                    {
                        difference = new Vector3(-1 * difference.x, difference.y, difference.z);
                        ActualDir = new Ray2D(transform.position, difference);
                        Bounce();
                        break;
                    }
                case 2: //in questo caso il colpo collide col bordo superiore, quello che deve cambiare è la componente y del vettore
                    {
                        difference = new Vector3(difference.x, -1*difference.y, difference.z);
                        ActualDir = new Ray2D(transform.position, difference);
                        Bounce();
                        break;
                    }
                case 3: //in questo caso il colpo collide col bordo di sinistra, quello che deve cambiare è la componente x del vettore
                    {
                        difference = new Vector3(-1 * difference.x, difference.y, difference.z);
                        ActualDir = new Ray2D(transform.position, difference);
                        Bounce();
                        break;
                    }
                case 4: //in questo caso il colpo collide col bordo inferiore, quello che deve cambiare è la componente y del vettore
                    {
                        difference = new Vector3(difference.x, -1 * difference.y, difference.z);
                        ActualDir = new Ray2D(transform.position, difference);
                        Bounce();
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
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
		speed = 1.1f * speed;
		rb.AddForce (ActualDir.direction * speed);
		if (count == bounces) {   // se count è uguale a "bounces" allora il colpo ha fatto abbastanza rimbalzi e può essere eliminato. (per sapere di più su bounces vedere sopra)
			Destroy (this.gameObject);
			GameObject pl = GameObject.FindGameObjectWithTag ("Player");
			pl.SendMessage ("AddPoint");
		}
		else // altrimenti il colpo non ha fatto abbastanza rimbalzi, quindi rimane in gioco e viene incrementato il suo count per tenere traccia di quante volte ha rimbalzato
			count++;
	}

}
