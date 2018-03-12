using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections;

public class MoverTests {




    Mover shot;


    [SetUp] // metodo per creare l'oggetto mover, viene invocato prima di ogni test
    public void SetUp()
    {
        shot = new GameObject().AddComponent<Mover>();//Viene creato un nuovo gameobject a cui si aggiunge lo script desiderato
        shot.tag = "Shot";
        Rigidbody2D rigibodyShot = shot.gameObject.AddComponent<Rigidbody2D>();//Sintassi che consente di aggiungere il componete all'oggetto appena creato
                                                                               //Definizione di alcune variabili necessarie al settaggio dello stato di Mover
        Ray2D ray = new Ray2D();
        Vector3 vec = new Vector3();
        //Settaggio dello stato attraverso il metodo construct
        shot.Construct(100, 4, 2, vec);

    }



    //Test che verifica il metodo bounce dello script Mover (verifica il cambiamento di velocita')
    [UnityTest]
    public IEnumerator MoverBounceSpeedTest()
    {
        
        //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start 
        yield return null;
        //invocazione del metodo bounce su shot
        shot.SendMessage("Bounce");
        //verifica dell'asserzione
        Assert.AreNotEqual(100, shot.speed);

    }

    //Test che verifica il metodo bounce dello script Mover (verifica il ramo if)
    [UnityTest]
    public IEnumerator MoverBounceIfSide()
    {
    
        var player = new GameObject().AddComponent<PlayerController>();//Viene creato un nuovo gameobject a cui si aggiunge lo script desiderato
        player.tag = "Player";
        player.gameObject.AddComponent<Rigidbody2D>();//Sintassi che consente di aggiungere il componete all'oggetto appena creato
        var points = new GameObject().AddComponent<Text>();//serve a creare un oggetto di gioco con una label, questo verra' poi passato al costruttore del player
        var gameOver = new GameObject().AddComponent<Text>();
        var restart = new GameObject().AddComponent<Button>();
        //Settaggio dello stato attraverso il metodo construct
        player.Construct(10, 0, points, gameOver, restart);

        //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        yield return null;
        shot.Construct(100, 4, 4);
        //invocazione del metodo bounce su shot
        shot.SendMessage("Bounce");
        yield return null;
        //verifica dell'asserzione
        Assert.AreEqual(1, player.GetPoints());
    }

    //Test che verifica il metodo bounce dello script Mover (verifica il ramo else)
    [UnityTest]
    public IEnumerator MoverBounceElseSide()
    {
        //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        yield return null;
        shot.Construct(100, 4, 2);
        //invocazione del metodo bounce su shot
        shot.SendMessage("Bounce");
        //verifica dell'asserzione
        Assert.AreEqual(3, shot.GetCount());
    }



    [UnityTest]
    public IEnumerator CollisionRightBackgroundX() // serve a vedere se il vettore viene cambiato regolarmente una volta avvenuta la collisione
    {
        //caso in cui collidiamo a destra, quello che deve cambiare è la x
        var background = new GameObject().AddComponent<Pause>(); //creo un oggetto che rappresenta il background
        background.tag = "Background";
        Rigidbody2D r = background.gameObject.AddComponent<Rigidbody2D>();
        var collider = r.gameObject.AddComponent<BoxCollider2D>(); // gli do un collider che passerò poi a OnTriggeredEnter2D
        background.GetComponent<BoxCollider2D>().offset = new Vector2(1, 0); // vado a definire l'offset che serve a capire in quale bordo è avvenuta la collisione (per maggiori informazioni vedere CheckCollider in Mover.cs)
        yield return null;
		var oldX = shot.GetDifference().x; // ottengo la vecchia x del vettore, la nuova dovrà essere la vecchia moltiplicato -1
        shot.SendMessage("OnTriggerEnter2D", collider);
		Assert.AreEqual(oldX, shot.GetDifference().x * -1);
    }


    [UnityTest]
    public IEnumerator CollisionRightBackgroundY() // quando il colpo si scontra col bordo destro o sinistro, la componente y non deve cambiare
    {
        var background = new GameObject().AddComponent<Pause>(); //creo un oggetto che rappresenta il background
        background.tag = "Background";
        Rigidbody2D r = background.gameObject.AddComponent<Rigidbody2D>();
        var collider = r.gameObject.AddComponent<BoxCollider2D>(); // gli do un collider che passerò poi a OnTriggeredEnter2D
        background.GetComponent<BoxCollider2D>().offset = new Vector2(1, 0); // vado a definire l'offset che serve a capire in quale bordo è avvenuta la collisione (per maggiori informazioni vedere CheckCollider in Mover.cs)
        yield return null;
        var oldY = shot.GetDifference().y; // ottengo la vecchia y del vettore, la nuova dovrà essere uguale
        shot.SendMessage("OnTriggerEnter2D", collider);
        Assert.AreEqual(oldY, shot.GetDifference().y);
    }


    [UnityTest]
    public IEnumerator CollisionUpBackgroundX() // quando il colpo si scontra col bordo superiore o inferiore, la componente x non deve cambiare
    {
        var background = new GameObject().AddComponent<Pause>(); //creo un oggetto che rappresenta il background
        background.tag = "Background";
        Rigidbody2D r = background.gameObject.AddComponent<Rigidbody2D>();
        var collider = r.gameObject.AddComponent<BoxCollider2D>(); // gli do un collider che passerò poi a OnTriggeredEnter2D
        background.GetComponent<BoxCollider2D>().offset = new Vector2(0, 1); // vado a definire l'offset che serve a capire in quale bordo è avvenuta la collisione (per maggiori informazioni vedere CheckCollider in Mover.cs)
        yield return null;
        var oldX = shot.GetDifference().x; // ottengo la vecchia x del vettore, la nuova dovrà essere uguale
        shot.SendMessage("OnTriggerEnter2D", collider);
        Assert.AreEqual(oldX, shot.GetDifference().x);
    }



    [UnityTest]
    public IEnumerator CollisionUpBackgroundY() // serve a vedere se il vettore viene cambiato regolarmente una volta avvenuta la collisione
    {
        //caso in cui collidiamo sopra, quello che deve cambiare è la y
        var background = new GameObject().AddComponent<Pause>(); //creo un oggetto che rappresenta il background
        background.tag = "Background";
        Rigidbody2D r = background.gameObject.AddComponent<Rigidbody2D>();
        var collider = r.gameObject.AddComponent<BoxCollider2D>(); // gli do un collider che passerò poi a OnTriggeredEnter2D
        background.GetComponent<BoxCollider2D>().offset = new Vector2(0, 1); // vado a definire l'offset che serve a capire in quale bordo è avvenuta la collisione (per maggiori informazioni vedere CheckCollider in Mover.cs)
        yield return null;
        var oldY = shot.GetDifference().y; // ottengo la vecchia y del vettore, la nuova dovrà essere la vecchia moltiplicato -1
        shot.SendMessage("OnTriggerEnter2D", collider);
        Assert.AreEqual(oldY, shot.GetDifference().y * -1);
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
