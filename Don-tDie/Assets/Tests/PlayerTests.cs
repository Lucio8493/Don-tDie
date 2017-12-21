using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerTests {



    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode

    PlayerController player; // il giocatore

   [SetUp] // metodo per creare l'oggetto player, viene invocato prima di ogni test
    public void SetUp() 
    {
        player = new GameObject().AddComponent<PlayerController>();//Viene creato un nuovo gameobject a cui si aggiunge lo script desiderato
        player.tag = "Player";
        player.gameObject.AddComponent<Rigidbody2D>();//Sintassi che consente di aggiungere il componete all'oggetto appena creato
        var points = new GameObject().AddComponent<Text>();//serve a creare un oggetto di gioco con una label, questo verra' poi passato al costruttore del player
        var gameOver = new GameObject().AddComponent<Text>();
        var restart = new GameObject().AddComponent<Button>();
        //Settaggio dello stato attraverso il metodo construct
        player.Construct(10, 0, points, gameOver, restart);

    }
   

    //Test del metodo di aggiunta dei punti del player
    [UnityTest]
    public IEnumerator PlayerControllerCountTest()
    {
        //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        yield return null;
        //invocazione del metodo addpoint sul giocatore
        player.SendMessage("AddPoint");
        //verifica dell'asserzione
        Assert.AreEqual(1, player.GetPoints());

    }

    //Test che controlla se ad inizio gioco il player abbia zero punti
    [UnityTest]
    public IEnumerator PlayerControllerZeroPoints()
    {
        yield return null; 		//si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        Assert.AreEqual(0, player.GetPoints());
    }



    //Metodo per la pulizia dell'ambiente di lavoro, viene invocato dopo ogni test
    [TearDown]
    public void TearDown()
    {
        //ricerca tutti gli oggetti di tipo shot e li distrugge
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Shot"))
        {
            Object.Destroy(gameObject);
        }

        //ricerca tutti gli oggetti di tipo player e li distrugge
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            Object.Destroy(gameObject);
        }

        //ricerca tutti gli oggetti di tipo background e li distrugge
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Background"))
        {
            Object.Destroy(gameObject);
        }
    }
}
