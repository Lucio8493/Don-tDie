using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (Time.time > nextFire) { 
			nextFire = Time.time + fireRate; 
			GameObject shot =(GameObject) PrefabUtility.InstantiatePrefab (ShotPrefab);//crea il colpo, se si cancella il colpo non appare ma
			shot.transform.position=transform.position;
		}

	}
}
