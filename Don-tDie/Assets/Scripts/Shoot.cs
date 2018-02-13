using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Shoot : MonoBehaviour {
	
	public float fireRate;
	public GameObject ShotPrefab;

    private bool panic = false; // serve per controllare se è stato premuto il panic button, con la modalità "panic" sarà il giocatore a decidere quando sparare, serve soprattutto per controllare i rimbalzi

	private float nextFire;

	//Metodo per il settaggio dello stato  nellla fase di testing
	public void Construct (float fireRate, float nextFire, GameObject ShotPrefab){
		this.fireRate = fireRate;
		this.nextFire = nextFire;
		this.ShotPrefab = ShotPrefab;
	}
		

	// Update e' chiamata ad intervalli regolari di tempo, sempre rispettando il principio che viene invocata una volta per frame
	void Update () {

        if (Input.GetKeyDown("space")) // esegue l'operazione dentro l'if quando viene premuto lo spazio
        {
            panic = !panic; // paused diventa l'opposto

            //ricerca tutti gli oggetti di tipo shot e li distrugge
            foreach (var gameObject in GameObject.FindGameObjectsWithTag("Shot"))
            {
                Object.Destroy(gameObject);
            }


        }

        if (panic) // se panic è vero allora smettila di sparare dopo tot secondi e spara ogni volta che si fa click col pulsante sinistro del mouse
        {
            if (Input.GetMouseButtonDown(0)) // 0 rappresenta il pulsante sinistro del mouse, 1 il destro, 2 il centrale
            {
                #if UNITY_EDITOR
                GameObject shot = (GameObject)PrefabUtility.InstantiatePrefab(ShotPrefab);//crea il colpo, se si cancella il colpo non appare
                #elif UNITY_STANDALONE
			    GameObject shot = Instantiate(ShotPrefab);
                #endif
                shot.transform.position = transform.position;
            }
			
        } /* Dato che in sistemi non abbiamo bisogno del codice che spara ogni x secondi, lo commento
		else // altrimenti continua il gioco normalmente, ovvero sparando in base ai secondi
        {
            if (Time.time > nextFire) { 
			    nextFire = Time.time + fireRate; 
			    #if UNITY_EDITOR
			    GameObject shot =(GameObject) PrefabUtility.InstantiatePrefab (ShotPrefab);//crea il colpo, se si cancella il colpo non appare
			    #elif UNITY_STANDALONE
			    GameObject shot = Instantiate(ShotPrefab);
			    #endif
			    shot.transform.position=transform.position;
		    }
			
        } */

        

	}
}
