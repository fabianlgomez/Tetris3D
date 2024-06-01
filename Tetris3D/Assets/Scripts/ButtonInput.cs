using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputs : MonoBehaviour{

    public static ButtonInputs instance;

    public GameObject[] rotateCanvaces;
    public GameObject moveCanvas;

    GameObject activeBlock;
    TetrisBlock activeTetris;
    bool moveIsOn = true;

    void Awake(){
        instance = this;
    }

    void Start(){
        SetInputs();
    }

    void RepositionActiveBlock(){
        if (activeTetris != null){
            transform.position = activeBlock.transform.position;
        }
    }

    public void SetActiveBlock(GameObject block, TetrisBlock tetris){
        activeBlock = block;
        activeTetris = tetris;
    }

    void Update(){
        RepositionActiveBlock();
        
    }

    public void MoveBlock(string direction){
        if(activeBlock != null){
            if (direction == "left"){
                activeTetris.SetInput(Vector3.left);
            }
            if (direction == "right"){
                activeTetris.SetInput(Vector3.right);
            }
            if (direction == "forward"){
                activeTetris.SetInput(Vector3.forward);
            }
            if (direction == "back"){
                activeTetris.SetInput(Vector3.back);
            } 
        }
    }

    public void RotateBlock(string rotation){

        if(activeBlock != null){
            //X ROTATION
            if (rotation == "posX"){
              activeTetris.SetRotationalInput(new Vector3(90, 0, 0));
            }
            if (rotation == "negX"){
              activeTetris.SetRotationalInput(new Vector3(-90, 0, 0));
            }
            //Y ROTATION
            if (rotation == "posY"){
              activeTetris.SetRotationalInput(new Vector3(0, 90, 0));
            }
            if (rotation == "negY"){
              activeTetris.SetRotationalInput(new Vector3(0, -90, 0));
            }
            //Z ROTATION
            if (rotation == "posZ"){
              activeTetris.SetRotationalInput(new Vector3(0, 0, 90));
            }
            if (rotation == "negZ"){
              activeTetris.SetRotationalInput(new Vector3(0, 0, -90));
            }

            
        }
    }

    public void SwitchInputs(){
        moveIsOn = !moveIsOn;
        SetInputs();
        // moveCanvas.SetActive(moveIsOn);
        // foreach(GameObject canvas in rotateCanvaces){
        //     canvas.SetActive(!moveIsOn);
        // }
    }
    void SetInputs(){
        // moveIsOn = !moveIsOn;
        moveCanvas.SetActive(moveIsOn);
        foreach(GameObject canvas in rotateCanvaces){
            canvas.SetActive(!moveIsOn);
        }
    }

    public void SetHighSpeed(){
        activeTetris.SetHighSpeed();
    }

}
