using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Tests {


	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode

	#if UNITY_EDITOR
	[UnityTest]
	public IEnumerator PrefabTest() {


		var ShotPrefab = Resources.Load("Prefabs/Shot") as GameObject; 
		var Gun = new GameObject().AddComponent<Shoot> ();
		Gun.Construct (1, 0, ShotPrefab);

		yield return null;
		
		var SpawnedShot = GameObject.FindWithTag("Shot");

		var SpawnedPrefab = PrefabUtility.GetPrefabParent (SpawnedShot);
		Assert.AreEqual (ShotPrefab, SpawnedPrefab);
	}
	#endif

	//Test che verifica il metodo bounce dello script Mover (verifica il cambiamento di velocita')
	[UnityTest]
	public IEnumerator SpeedUpdateTest(){
		var shot = new GameObject ().AddComponent<Mover> ();//Viene creato un nuovo gameobject a cui si aggiunge lo script desiderato
		Rigidbody2D rigibodyShot = shot.gameObject.AddComponent<Rigidbody2D> ();//Sintassi che consente di aggiungere il componete all'oggetto appena creato
		//Definizione di alcune variabili necessarie al settaggio dello stato di Mover
		Ray2D ray = new Ray2D ();
		Vector3 vec = new Vector3 ();
		//Settaggio dello stato attraverso il metodo construct
		shot.Construct (100, 4, ray, 2, vec);

		//si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
		yield return null;
		//invocazione del metodo bounce su shot
		shot.SendMessage ("Bounce");
		//verifica dell'asserzione
		Assert.AreNotEqual (100, shot.speed);			
	}

	[TearDown]
	public void TearDown(){
		foreach (var gameObject in GameObject.FindGameObjectsWithTag("Shot")) {
			Object.Destroy(gameObject);
		}
		foreach (var gameObject in GameObject.FindGameObjectsWithTag("Player")) {
			Object.Destroy(gameObject);
		}
	}
}
