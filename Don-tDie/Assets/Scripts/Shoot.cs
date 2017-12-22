using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Shoot : MonoBehaviour {
	
	public float fireRate;
	public GameObject ShotPrefab;

	private float nextFire;

	//Metodo per il settaggio dello stato  nellla fase di testing
	public void Construct (float fireRate, float nextFire, GameObject ShotPrefab){
		this.fireRate = fireRate;
		this.nextFire = nextFire;
		this.ShotPrefab = ShotPrefab;
	}
		

	// Update e' chiamata ad intervalli regolari di tempo, sempre rispettando il principio che viene invocata una volta per frame
	void Update () {
		if (Time.time > nextFire) { 
			nextFire = Time.time + fireRate; 
			#if UNITY_EDITOR
			GameObject shot =(GameObject) PrefabUtility.InstantiatePrefab (ShotPrefab);//crea il colpo, se si cancella il colpo non appare
			#elif UNITY_STANDALONE
			GameObject shot = Instantiate(ShotPrefab);
			#endif
			shot.transform.position=transform.position;
		}

	}
}
