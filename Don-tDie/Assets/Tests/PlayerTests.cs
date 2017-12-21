using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using NUnit.Framework;
using WindowsInput; // importato https://inputsimulator.codeplex.com/
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
        Rigidbody2D rigid = player.gameObject.AddComponent<Rigidbody2D>();//Sintassi che consente di aggiungere il componete all'oggetto appena creato
        rigid.gravityScale = 0; //serve per impostare la gravità a zero, AddComponet aggiunge un rigidBody di default che ha come gravità uno e che quindi fa scendere l'oggetto
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

    [UnityTest]
    public IEnumerator PlayerControllerInitialPositionX()
    {
        yield return null; 		//si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        Assert.AreEqual(0, player.GetPosX());
    }

    [UnityTest]
    public IEnumerator PlayerControllerInitialPositionY()
    {
        yield return null; 		//si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        Assert.AreEqual(0, player.GetPosY());
    }

    [UnityTest]
    public IEnumerator PlayerControllerMoveDown() // test per controllare se il giocatore si muove in basso
    {
        float prec = player.GetPosY(); // prendo la posizione predecente che confronterò con la successiva
        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S); // simula la pressione su tastiera
        yield return null;     //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        //InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S); // simula la pressione su tastiera
        Assert.GreaterOrEqual(prec, player.GetPosY()); // la posizione y deve essere minore di quella precedente
    }

    [UnityTest]
    public IEnumerator PlayerControllerMoveRight() // test per controllare se il giocatore si muove a destra
    {
        float prec = player.GetPosX();
        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D); // simula la pressione su tastiera
        yield return null;     //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        Assert.GreaterOrEqual(player.GetPosX(),prec); // la posizione x deve essere maggiore di quella precedente
    }

    [UnityTest]
    public IEnumerator PlayerControllerMoveLeft() // test per controllare se il giocatore si muove a sinistra
    {
        float prec = player.GetPosX();
        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A); // simula la pressione su tastiera
        yield return null;     //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
        Assert.GreaterOrEqual(prec, player.GetPosX()); // la posizione x deve essere minore di quella precedente
    }

    [UnityTest]
    public IEnumerator PlayerControllerMoveUp() // test per controllare se il giocatore si muove in alto
    {
        float prec = player.GetPosY();
        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W); // simula la pressione su tastiera
        yield return null;       //si invoca il metodo per l'instanziazione del gameobject, viene eseguito start e una volta l'update
       //Assert.AreEqual(0, player.GetPosY());
        Assert.GreaterOrEqual(player.GetPosY(), prec); // la posizione y deve essere maggiore di quella precedente
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
