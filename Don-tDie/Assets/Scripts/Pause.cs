using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {


    public bool paused = false; // il gioco non comincia in pause, se si mette = true per far cominciare il gioco si dovrebbe premere p

    bool GetPaused()
    {
        return paused;
    }
    

    void Update()
    {
        if (Input.GetKeyDown("p")) // esegue l'operazione dentro l'if quando viene premuto il pulsante "p" da tastiera
        {
            paused = !paused; // paused diventa l'opposto
        }
        if (paused) // se paused è vero allora il gioco va fermato
        {
            Time.timeScale = 0; // così facendo tutto il gioco viene fermato 
        }
        else // altrimenti paused è falso, quindi il gioco deve ricominciare
        {
            Time.timeScale = 1; // Time ci permette di far ricominciare il gioco
        }
    }
}
