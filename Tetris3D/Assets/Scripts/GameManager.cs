using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager instance;
    int score;
    int level;
    int layersCleared;

    bool gameIsOver;

    float fallSpeed;

    // public GameObject gameOverWindow;

    void Awake(){
        instance = this;
    }
    
    void Start(){
        // gameOverWindow.SetActive(false);
        SetScore(score);
    }

    public void SetScore(int amount){
        score += amount;
        CalculateLevel();
        UIHandler.instance.UpdateUI(score, level, layersCleared);
        //UPDATE UI

    }

    public float ReadFallSpeed(){
        return fallSpeed;
        
    }

    public void LayersCleared(int amount){

        if (amount == 1){
            SetScore(300);
        }else if (amount == 2){
            SetScore(600);
        }else if (amount == 3){
            SetScore(900);
        }else if (amount == 4){
            SetScore(1800);
        }else if (amount == 5){
            SetScore(1800*2);
        }else if (amount == 6){
            SetScore(1800*2*2);
        }else if (amount == 7){
            SetScore(1800*2*2*2);
        }else if (amount == 8){
            SetScore(1800*2*2*2*2);
        }else if (amount == 9){
            SetScore(1800*2*2*2*2*2);
        }else if (amount == 10){
            SetScore(1800*2*2*2*2*2*2);
        }


        layersCleared += amount;
        //UPDATE UI
        UIHandler.instance.UpdateUI(score, level, layersCleared);
    }
  
    //esto se puede hacer con un bucle
    void CalculateLevel(){
        if(score < 10000){
            level = 1;
            fallSpeed = 3f;
        }else if(score > 10000 && score <= 20000){
            level = 2;
            fallSpeed = 2.75f;
        }else if(score > 20000 && score <=30000){
            level = 3;
            fallSpeed = 2.5f;
        }else if(score > 30000 && score <=40000){
            level = 4;
            fallSpeed = 2.25f;
        }else if(score > 40000 && score <=50000){
            level = 5;
            fallSpeed = 2f;
        }else if(score > 50000 && score <=60000){
            level = 6;
            fallSpeed = 1.75f;
        }else if(score > 60000 && score <=70000){
            level = 7;
            fallSpeed = 1.5f;
        }else if(score > 70000 && score <=80000){
            level = 8;
            fallSpeed = 1.25f;
        }else if(score > 80000 && score <=90000){
            level = 9;
            fallSpeed = 1.1f;
        }else if(score > 90000 && score <=100000){
            level = 10;
            fallSpeed = 1f;
        }


        //UPDATE UI
    }

     // get and set methods
    public bool ReadGameIsOver(){
        return gameIsOver;
    }

    public void SetGameIsOver(){
        gameIsOver = true;
        //UPDATE UI
        UIHandler.instance.ActivateGameOverWindow();
        // gameOverWindow.SetActive(true);
    }

}
