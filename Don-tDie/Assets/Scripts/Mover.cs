using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// nb, colpo e shot vengono usati come sinonimo e si riferiscono al prefab Shot
public class Mover : MonoBehaviour {

	public float force; // valore della forza applicata al proiettile per spostarla
    public int bounces; // i rimbalzi che il colpo deve fare prima di essere eliminato e scomparire dal gioco, se si vuole modificare il valore lo si trova in /Assets/Prefabs/Shot

	private Rigidbody2D rb;
	private Ray2D ActualDir;
	private int count; // i rimbalzi che il colpo ha gia fatto
    private Vector3 difference; // il vettore della direzione del colpo, mi servirà poi per andare a calcolare la direzione dopo il rimbalzo





    public Vector3 GetDifference() // get che serve per il testing, restitisce il vettore direzione
    {
        return difference;
    }

    public Ray2D GetActualDir()  // get che serve per il testing
    {
        return ActualDir;
    }



	//Metodi per il settaggio dello stato  nellla fase di testing
	public void Construct ( float force, int bounces, Ray2D ActualDir, int count, Vector3 difference){
		this.force = force;
		this.bounces = bounces;
		this.ActualDir = ActualDir;
		this.count = count;
		this.difference = difference;
	}

	public void Construct ( float force, int bounces, int count){
		this.force = force;
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
			//difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
			// in Sistemi difference lo crediamo con numeri casuali invece di darli in base alla direzione del mouse 
			difference =  new Vector3 (Random.Range(-1.0f, 1.0f),Random.Range(-1.0f, 1.0f),0); 
			
		}
		catch(System.NullReferenceException e){
			difference = new Vector3 (4, 4, 0);
		}

		difference.Normalize();
		ActualDir = new Ray2D (transform.position, difference);
		rb.AddForce (ActualDir.direction * force);

		count = 0;
	}


	/************  test     */
	void FixedUpdate () {

	}





	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) { // in questo if entriamo se il colpo ("shot") collide con il giocatore (la navicella)
			other.gameObject.SendMessage ("GameOver");

        }
		/* Per sistemi non abbiamo bisogno del rimbalzo tra i colpi, quindi lo commento
		else if(other.gameObject.CompareTag ("Shot")){ // in questo if entriamo se un colpo ("shot") collide con un altro colpo
			rb.velocity =new Vector2(0.0f,0.0f);
            difference = new Vector3(-1 * difference.x, -1*difference.y, difference.z); // quando un colpo collide con un un altro va nella direzione opposta a quella in cui andava prima
            ActualDir = new Ray2D(transform.position, difference);
            Bounce();


        }
		*/
		else if(other.gameObject.CompareTag("Background")){ // in questo if entriamo se un colpo ("shot") collide con uno dei quattro bordi dello schermo
            rb.velocity =new Vector2(0.0f,0.0f);
			int collider = CheckCollider (other);
            if (collider == 1 || collider == 3) // se  collider è uno o tre il colpo si è scontrato con il muro di destra o sinistra, in questo caso va invertita la componente x
            {
                difference = new Vector3(-1 * difference.x, difference.y, difference.z);
            } else // se invece collider è due oppure quattro il colpo si è scontrato con un muro superiore o inveriore, in questo caso va cambiata la componente y 
            {
                difference = new Vector3(difference.x, -1 * difference.y, difference.z);

            }

            ActualDir = new Ray2D(transform.position, difference);
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
		/* per sistemi l'aumento di velocità non ci serve, quindi lo commento
		speed = 1.2f * speed;
		*/
		rb.AddForce (ActualDir.direction * force);
		if (count >= bounces) {   // se count è uguale a "bounces" allora il colpo ha fatto abbastanza rimbalzi e può essere eliminato. (per sapere di più su bounces vedere sopra)
			//Destroy (this.gameObject); in Sistemi la distruzione dello shoot non ci serve, quindi viene eliminata
			GameObject pl = GameObject.FindGameObjectWithTag ("Player");
			pl.SendMessage ("AddPoint");
		}
		else // altrimenti il colpo non ha fatto abbastanza rimbalzi, quindi rimane in gioco e viene incrementato il suo count per tenere traccia di quante volte ha rimbalzato
			count++;
	}

}
