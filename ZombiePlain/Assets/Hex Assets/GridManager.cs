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
	public int biomeCores = 15;
	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;
	private int currType;
	private bool[,] occArray;
	private Vector3[] coreArray;
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
		coreArray = new Vector3[biomeCores];
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
			coreArray [i] = new Vector3 ((float)Random.Range (0, gridWidthInHexes), 
			                             (float)Random.Range (0, gridHeightInHexes), 0f);

			//Make sure it's not right next to another one
			bool redo = false;
			int minSpace = 10;
			for(int x = 0; x < i; x++)
			{
				if((Mathf.Abs(coreArray[x].x - coreArray[i].x) < minSpace) &&
				   (Mathf.Abs(coreArray[x].x - coreArray[i].x) < minSpace))
					redo = true;
			}
			int j = 0; //counter to make sure there is not an infinite loop
			while((j < 50) && redo)
			{
				coreArray [i] = new Vector3 ((float)Random.Range (0, gridWidthInHexes), 
				                             (float)Random.Range (0, gridHeightInHexes), 0f);

				for(int x = 0; x < i; x++)
				{
					if((Mathf.Abs(coreArray[x].x - coreArray[i].x) < minSpace) &&
					   (Mathf.Abs(coreArray[x].x - coreArray[i].x) < minSpace))
						redo = true;
					else
						redo = false;
				}

				j++;
			}
		}

		int plainCount = 0;
		int forestCount = 0;
		int desertCount = 0;
		for (int i = 0; i < biomeCores; i++)
		{
			int hexType;
			bool redo = false;
			int offNum = 2;
			int maxRedo = 10;
			int j = 0;
			do
			{
				j++;
				hexType = Random.Range(0, 3);
				switch(hexType)
				{
				case 0:
					if(((plainCount - forestCount) >= offNum) &&
					   ((plainCount - desertCount) >= offNum))
						redo = true;
					break;
				case 1:
					if(((forestCount - plainCount) >= offNum) &&
					   ((forestCount - desertCount) >= offNum))
						redo = true;
					break;
				case 2:
					if(((desertCount - forestCount) >= offNum) &&
					   ((desertCount - plainCount) >= offNum))
						redo = true;
					break;
				default:
					break;
				}

			}while(redo && (j <= maxRedo));

			GameObject coreHex = null;
			switch(hexType)
			{
				case 0:
					coreHex = (GameObject)Instantiate(Hex1);
					coreArray[i].z = 1f;
					plainCount++;
					break;
				case 1:
					coreHex = (GameObject)Instantiate(Hex2);
					coreArray[i].z = 2f;
					forestCount++;
					break;
				case 2:
					coreHex = (GameObject)Instantiate(Hex3);
					coreArray[i].z = 3f;
					desertCount++;
					break;
				default:
					coreHex = (GameObject)Instantiate(Hex1);
					coreArray[i].z = 1f;
					plainCount++;
					break;
			}

			coreHex.transform.position = calcWorldCoord(new Vector2(coreArray[i].x, coreArray[i].y));
			coreHex.transform.parent = hexGridGO.transform;

			//Mark core tiles as occupied
			occArray[(int)coreArray[i].x, (int)coreArray[i].y] = true;
		}

		int expCount = 0; //count of expired cores
		//cores expire when all tiles in the current ring are already occupied
		for (int ring = 1; ring <= 10; ring++) 
		{
			for(int i = 0; i < biomeCores; i++)
			{
				Vector3 currCore = coreArray[i];
				int hexType = (int)currCore.z;
				Vector2 currPos;
				
				//if core is in even row number top and bottom x coords are -1
				bool isEven;
				if ((currCore.y % 2) == 0)
					isEven = true;
				else
					isEven = false;

				currPos = new Vector2(currCore.x + ring, currCore.y);
				int sidePos = 1;
				/*
				if (!((currCore.x < 0) || (currCore.y < 0) || 
				  		(currCore.x > gridWidthInHexes) || (currCore.y > gridHeightInHexes)))
				{
				*/
				if (currPos.x < gridWidthInHexes)
				{
					if (!occArray[(int)currPos.x, (int)currPos.y])
					{
						GameObject hex = null;
						switch(hexType)
						{
						case 1:
							hex = (GameObject)Instantiate(Hex1);
							break;
						case 2:
							hex = (GameObject)Instantiate(Hex2);
							break;
						case 3:
							hex = (GameObject)Instantiate(Hex3);
							break;
						default:
							hex = (GameObject)Instantiate(Hex1);
							break;
						}

						//Current position in grid
						hex.transform.position = calcWorldCoord(currPos);
						hex.transform.parent = hexGridGO.transform;

						occArray[(int)currPos.x, (int)currPos.y] = true;
					}
				}

				bool rowEven = isEven;
				//Fill in each of the six sides of the ring
				for(int j = 0; j < 6; j++)
				{
					Vector2 nextPos = currPos;
					while (sidePos < ring)
					{
						switch(j)
						{
						case 0:
							if(rowEven)
							{
								nextPos = new Vector2(currPos.x - 1, currPos.y + 1);
							}
							else
							{
								nextPos = new Vector2(currPos.x , currPos.y + 1);
							}
							rowEven = !rowEven;
							break;
						case 1:
							if(rowEven)
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x - 1, currPos.y + 1);
									rowEven = !rowEven;
								}
								else
									nextPos = new Vector2(currPos.x - 1, currPos.y);
							}
							else
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x, currPos.y + 1);
									rowEven = !rowEven;
								}
								else
									nextPos = new Vector2(currPos.x - 1, currPos.y);
							}
							break;
						case 2:
							if(rowEven)
							{
								if(sidePos == 0)
									nextPos = new Vector2(currPos.x - 1, currPos.y);
								else
								{
									nextPos = new Vector2(currPos.x - 1, currPos.y - 1);
									rowEven = !rowEven;
								}
									
							}
							else
							{
								if(sidePos == 0)
									nextPos = new Vector2(currPos.x - 1, currPos.y);
								else
								{
									nextPos = new Vector2(currPos.x, currPos.y - 1);
									rowEven = !rowEven;
								}
							}
							break;
						case 3:
							if(rowEven)
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x - 1, currPos.y - 1);
									rowEven = !rowEven;
								}
								else
								{
									nextPos = new Vector2(currPos.x, currPos.y - 1);
									rowEven = !rowEven;
								}
							}
							else
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x, currPos.y - 1);
									rowEven = !rowEven;
								}
								else
								{
									nextPos = new Vector2(currPos.x + 1, currPos.y - 1);
									rowEven = !rowEven;
								}
							}
							break;
						case 4:
							if(rowEven)
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x, currPos.y - 1);
									rowEven = !rowEven;
								}
								else
								{
									nextPos = new Vector2(currPos.x + 1, currPos.y);
								}
							}
							else
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x + 1, currPos.y - 1);
									rowEven = !rowEven;
								}
								else
								{
									nextPos = new Vector2(currPos.x + 1, currPos.y);
								}
							}
							break;
						case 5:
							if(rowEven)
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x + 1, currPos.y);
								}
								else
								{
									nextPos = new Vector2(currPos.x, currPos.y + 1);
									rowEven = !rowEven;
								}
							}
							else
							{
								if(sidePos == 0)
								{
									nextPos = new Vector2(currPos.x + 1, currPos.y);
								}
								else
								{
									nextPos = new Vector2(currPos.x + 1, currPos.y + 1);
									rowEven = !rowEven;
								}
							}
							break;
						default:
							break;	
						}

						//If next position isn't out of bound or occupied, add tile
						if (!((nextPos.x < 0) || (nextPos.y < 0) || 
						      (nextPos.x >= gridWidthInHexes) || (nextPos.y >= gridHeightInHexes)) &&
						    !occArray[(int)nextPos.x, (int)nextPos.y])
						{
							GameObject hex = null;
							switch(hexType)
							{
							case 1:
								hex = (GameObject)Instantiate(Hex1);
								break;
							case 2:
								hex = (GameObject)Instantiate(Hex2);
								break;
							case 3:
								hex = (GameObject)Instantiate(Hex3);
								break;
							default:
								hex = (GameObject)Instantiate(Hex1);
								break;
							}

							//Current position in grid
							hex.transform.position = calcWorldCoord(nextPos);
							hex.transform.parent = hexGridGO.transform;
							
							occArray[(int)nextPos.x, (int)nextPos.y] = true;
						}

						currPos = nextPos;
						sidePos++;
					}
					sidePos = 0;
				}
			}
		}
		
		for (int y = 0; y < gridHeightInHexes; y++)
		{
			for (int x = 0; x < gridWidthInHexes; x++)
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

				if(!occArray[x,y])
				{
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