using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{

    public static GameManager current;

    public GameObject playerBlueprint;

    private void Start(){
        current = this;
        GameEvents.current.onBlockLanding += onBlockLanding;
    }

    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            Debug.Log("Exit");
            Application.Quit();
        }

        if (Input.GetKeyUp(KeyCode.C)) {
            Debug.Log("Camera changed");
            GameEvents.current.ChangeCamera();
        }

    }

    public bool startGame(){
        if (GameObject.FindWithTag("Player") == null) {
            GameObject player = Instantiate(playerBlueprint);
            player.SetActive(true);
            return true;
        }
        return false;
    }

    public bool gameOver() {
        return GameObject.FindWithTag("Player") == null; //Also true before starting game
    }

    public void onBlockLanding(int id) {
        Debug.Log(id);
    }

    public void onPlayerDeath() {
        Debug.Log("Game Over");
    }

}
