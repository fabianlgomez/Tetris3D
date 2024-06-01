using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playfiled : MonoBehaviour
{
	// Start is called before the first frame update

	public static Playfiled instance;
	public int gridSizeX, gridSizeY, gridSizeZ;
	[Header ("Blocks")]
	public GameObject[] blocksList;
	public GameObject[] ghostList;

	[Header("Playfield Visuals")]
	public GameObject bottomPlane;
	public GameObject N, S, W, E;

	int randomIndex;
	public Transform[,,] theGrid;

	void Awake(){
		instance = this;
	}

	void Start()
	{
		theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
		CalculatePreviewBlock();
		SpawnNewBlock();
	}

	public Vector3 Round(Vector3 vec){
		return new Vector3(
					Mathf.RoundToInt(vec.x),
					Mathf.RoundToInt(vec.y),
					Mathf.RoundToInt(vec.z));
	}

	public bool CheckInsideGrid(Vector3 pos){
		//(int)pos.y >= 0 && (int)pos.y < gridSizeY &&
		return (
				(int)pos.x >= 0 && (int)pos.x < gridSizeX &&
				(int)pos.z >= 0 && (int)pos.z < gridSizeZ &&
				(int)pos.y >= 0
				);
	}

	public void UpdateGrid(TetrisBlock block){
		//DELETE POSIBLE PARENT OBJETCT 
		for (int x = 0; x < gridSizeX; x++){
			for (int z = 0; z < gridSizeZ; z++){
				for (int y = 0; y < gridSizeY; y++){
					if (theGrid[x, y, z] != null){
						if (theGrid[x, y, z].parent == block.transform){
							theGrid[x, y, z] = null;
						}
					}
				}
			}
		}
		//FILL IN ALL CHILD OBJECTS
		foreach (Transform child in block.transform){
			Vector3 pos = Round(child.position);
			if (pos.y < gridSizeY){
				theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
			}
		}
	}	

	public Transform GetTransformOnGridPos(Vector3 pos){
		if (pos.y > gridSizeY - 1){
			return null;
		}else{
			return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
		}
	}

	public void SpawnNewBlock(){
		Vector3 spawnPoint = new Vector3(
			(int) (transform.position.x + (float)gridSizeX / 2), 
			(int) transform.position.y + gridSizeY,
		 	(int) (transform.position.z + (float)gridSizeZ / 2) );
		
		// int randomIndex = UnityEngine.Random.Range(0, blocksList.Length); //cambia un poquito respecto al codigo del tutorial (video 20 )
		//SPAWN THE BLOCK
		GameObject newBlock = Instantiate(blocksList[randomIndex], spawnPoint, Quaternion.identity) as GameObject; //newBlock antes estaba como block
		//GHOST BLOCK
		GameObject newGhost = Instantiate(ghostList[randomIndex], spawnPoint, Quaternion.identity) as GameObject;
		//SET INPUTS - Se hace en TetrisBlock
		newGhost.GetComponent<GhostBlock>().SetParent(newBlock);

		CalculatePreviewBlock();
		Previewer.instance.ShowPreviewBlock(randomIndex);
	}

	public void CalculatePreviewBlock(){
		//PREVIEW BLOCK
		//GET THE PREVIEW BLOCK
		randomIndex = UnityEngine.Random.Range(0, blocksList.Length);
		Previewer.instance.ShowPreviewBlock(randomIndex);
	}
	
	
	public void DeleteLayer(){
		int layersCleared = 0;
		for (int y = gridSizeY-1; y >= 0; y--){
			//CHECK FULL LAYER
			if (CheckFullLayer(y)){
				layersCleared++;
				//DELETE LAYER - DELETE ALL BLOCKS
				DeleteLayerAt(y);
				//MOVE ALL BLOCKS DOWN
				MoveAllLayerDown(y);
			}
		}
		if (layersCleared > 0){
			GameManager.instance.LayersCleared(layersCleared);
		}
	}
	
	void MoveAllLayerDown(int y){
		for (int i = y; i < gridSizeY; i++){
			MoveOneLayerDown(i);
		}
	}

	void MoveOneLayerDown(int y){
		for (int x = 0; x < gridSizeX; x++){
			for (int z = 0; z < gridSizeZ; z++){
				if (theGrid[x, y, z] != null){
					theGrid[x, y - 1, z] = theGrid[x, y, z];
					theGrid[x, y, z] = null;
					theGrid[x, y - 1, z].position += Vector3.down;
				}
			}
		}
	}

	bool CheckFullLayer(int y){
		for (int x = 0; x < gridSizeX; x++){
			for (int z = 0; z < gridSizeZ; z++){
				if (theGrid[x, y, z] == null){
					return false;
				}
			}
		}
		return true;
	}
	
	
	void DeleteLayerAt(int y){
		for (int x = 0; x < gridSizeX; x++){
			for (int z = 0; z < gridSizeZ; z++){
				Destroy(theGrid[x, y, z].gameObject);
				theGrid[x, y, z] = null;
			}
		}
	}


	void OnDrawGizmos()
	{
		//codigo para dibujar el grid en el editor para el plano bottom
		if (bottomPlane != null)
		{
			//RESIZE BUTTOM PLANE
			Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeZ / 10);
			bottomPlane.transform.localScale = scaler;

			//REPOSITION BUTTOM PLANE**
			bottomPlane.transform.position = new Vector3(
							transform.position.x + (float)gridSizeX / 2,
							transform.position.y,
							transform.position.z + (float)gridSizeZ / 2);

			//RETILE MATERIAL 
			bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);

		}

		//codigo para dibujar el grid en el editor para el plano N
		if (N != null)
		{
			//RESIZE BUTTOM PLANE
			Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
			N.transform.localScale = scaler;

			//REPOSITION BUTTOM PLANE**
			N.transform.position = new Vector3(
							transform.position.x + (float)gridSizeX / 2,
							transform.position.y + (float)gridSizeY / 2,
							transform.position.z + gridSizeZ);

			//RETILE MATERIAL 
			N.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);

		}

		//codigo para dibujar el grid en el editor para el plano S
		if (S != null)
		{
			//RESIZE BUTTOM PLANE
			Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
			S.transform.localScale = scaler;

			//REPOSITION BUTTOM PLANE**
			S.transform.position = new Vector3(
							transform.position.x + (float)gridSizeX / 2,
							transform.position.y + (float)gridSizeY / 2,
							transform.position.z);

			//RETILE MATERIAL 
			// S.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY); el material está compartido con N

		}

		//codigo para dibujar el grid en el editor para el plano W
		if (W != null)
		{
			//RESIZE BUTTOM PLANE
			Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
			W.transform.localScale = scaler;

			//REPOSITION BUTTOM PLANE**
			W.transform.position = new Vector3(
							transform.position.x,
							transform.position.y + (float)gridSizeY / 2,
							transform.position.z + (float)gridSizeZ / 2);

			//RETILE MATERIAL 
			W.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);

		}

		//codigo para dibujar el grid en el editor para el plano E
		if (E != null)
		{
			//RESIZE BUTTOM PLANE
			Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
			E.transform.localScale = scaler;

			//REPOSITION BUTTOM PLANE**
			E.transform.position = new Vector3(
							transform.position.x + gridSizeX,
							transform.position.y + (float)gridSizeY / 2,
							transform.position.z + (float)gridSizeZ / 2);

			//RETILE MATERIAL 
			//E.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);

		}

	}









}
