using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{

	float previousTime;
	float fallTime = 1f;
	// Start is called before the first frame update
	void Start(){
		ButtonInputs.instance.SetActiveBlock(gameObject, this);	
		fallTime = GameManager.instance.ReadFallSpeed();
		if (!CheckValidMove()){
			GameManager.instance.SetGameIsOver();
		

		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time - previousTime > fallTime)
		{	
			transform.position += Vector3.down;
			if (!CheckValidMove()){
				//Debug.Log("No se puede mover");
				transform.position += Vector3.up;
				//DELETE LAYER IF POSSIBLE
				Playfiled.instance.DeleteLayer();

				enabled = false;
				//CREATE A NEW TETRIS BLOCK
				if (!GameManager.instance.ReadGameIsOver()){
					Playfiled.instance.SpawnNewBlock();
				}
				// Playfiled.instance.SpawnNewBlock();
				// return;
			}else{
				//UPDATE THE GRID
				Playfiled.instance.UpdateGrid(this);
			}
			previousTime = Time.time;
		}
		
		//MOVEMENT
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			// SetInput(Vector3.left);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			SetInput(Vector3.right);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			//SetInput(Vector3.forward);
			SetRotationalInput(new Vector3(90, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			// SetInput(Vector3.back);
			SetRotationalInput(new Vector3(-90, 0, 0));
		}


	}

	public void SetInput(Vector3 direction){
		transform.position += direction;
		if (!CheckValidMove()){
			transform.position -= direction;
		}else{
			Playfiled.instance.UpdateGrid(this);
		}
	} 
	
	public void SetRotationalInput(Vector3 rotation){
		transform.Rotate(rotation, Space.World);
		
		if (!CheckValidMove()){
			transform.Rotate(-rotation, Space.World);
		}else{
			Playfiled.instance.UpdateGrid(this);
		}

	}


	bool CheckValidMove(){

		foreach (Transform child in transform){
			Vector3 pos = Playfiled.instance.Round(child.position);
			if (!Playfiled.instance.CheckInsideGrid(pos) ){
				return false;
			}
		}
		foreach (Transform child in transform){
			Vector3 pos = Playfiled.instance.Round(child.position);
			Transform t = Playfiled.instance.GetTransformOnGridPos(pos);
			if (t != null && t.parent != transform){
				return false;
			}
		}
		return true;
	}

	public void SetHighSpeed(){
		fallTime = 0.1f;
	}


}
