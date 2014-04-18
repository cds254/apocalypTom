using UnityEngine;
using System.Collections;

public class GridManager: MonoBehaviour
{
	//following public variable is used to store the hex model prefab;
	//instantiate it by dragging the prefab on this variable using unity editor
	public GameObject Hex1;
	public GameObject Hex2;
	public GameObject Hex3;
	public GameObject BlankHex;
	//next two variables can also be instantiated using unity editor
	public int gridWidthInHexes = 10;
	public int gridHeightInHexes = 10;
	public int dropDepth = 5;
	public int preDropPenalty = 5;
	public int dropDepthPenalty = 50;
	public int postDropPenalty = 10;
	public int biomeCores = 10;
	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;
	private int currType;
	private bool[,] occArray;
	private Vector2[] coreArray;
	//Method to initialise Hexagon width and height
	void setSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		hexWidth = Hex1.renderer.bounds.size.x;
		hexHeight = Hex1.renderer.bounds.size.z;
	}

	//initialize array to keep track which tiles have already been generated
	void initArrays()
	{
		//Default value is false for each bool
		occArray = new bool[gridWidthInHexes, gridHeightInHexes];
		coreArray = new Vector2[biomeCores];
	}
	
	//Method to calculate the position of the first hexagon tile
	//The center of the hex grid is (0,0,0)
	Vector3 calcInitPos()
	{
		Vector3 initPos;
		//the initial position will be in the left upper corner
		initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0,
		                      gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
		
		return initPos;
	}
	
	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = calcInitPos();
		//Every second row is offset by half of the tile width
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;
		
		float x =  initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		return new Vector3(x, 0, z);
	}
	
	//Finally the method which initialises and positions all the tiles
	void createGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");

		//Randomly generate core tiles
		for (int i = 0; i < biomeCores; i++) 
		{
			coreArray[i] = new Vector2((float)Random.Range(0, gridWidthInHexes), 
			                             (float)Random.Range(0, gridHeightInHexes));

			int hexType = Random.Range(0, 3);
			GameObject coreHex = null;
			switch(hexType)
			{
				case 0:
					coreHex = (GameObject)Instantiate(Hex1);
					break;
				case 1:
					coreHex = (GameObject)Instantiate(Hex2);
					break;
				case 2:
					coreHex = (GameObject)Instantiate(Hex3);
					break;
				default:
					coreHex = (GameObject)Instantiate(Hex1);
					break;
			}

			coreHex.transform.position = calcWorldCoord(coreArray[i]);
			coreHex.transform.parent = hexGridGO.transform;
		}
		
		for (float y = 0; y < gridHeightInHexes; y++)
		{
			for (float x = 0; x < gridWidthInHexes; x++)
			{
				/*
				int hexType = Random.Range(0, 3);
				//GameObject assigned to Hex public variable is cloned
				GameObject hex = null;
				switch(hexType)
				{
					case 0:
						hex = (GameObject)Instantiate(Hex1);
						break;
					case 1:
						hex = (GameObject)Instantiate(Hex2);
						break;
					case 2:
						hex = (GameObject)Instantiate(Hex3);
						break;
					default:
						hex = (GameObject)Instantiate(Hex1);
						break;
				}
				*/
				bool isCore = false;
				for (int i = 0; i < biomeCores; i++) 
				{
					Vector2 coreCoord = coreArray[i];
					if((coreCoord.x == x) && (coreCoord.y == y))
					{
						isCore = true;
					}
				}

				if(!isCore)
				{
					GameObject hex = (GameObject)Instantiate(BlankHex);

					//mark position as generated

					//Current position in grid
					Vector2 gridPos = new Vector2(x, y);
					hex.transform.position = calcWorldCoord(gridPos);
					hex.transform.parent = hexGridGO.transform;
				}
			}
		}
	}
	
	//The grid should be generated on game start
	void Start()
	{
		setSizes();
		initArrays();
		createGrid();
	}
}