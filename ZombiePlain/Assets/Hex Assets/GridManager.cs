using UnityEngine;
using System.Collections;

public class GridManager: MonoBehaviour
{
	//Hex prefab GameObjects
	public GameObject Hex1;	    //Plains
	public GameObject Hex2;     //Forest
	public GameObject Hex3;     //Desert
	public GameObject BlankHex; //Unthemed

	//Public variables
	public int gridWidthInHexes = 10;
	public int gridHeightInHexes = 10;
	public int biomeCores = 15;      //Determines the complexity of the ground layout
	public int minCoreSpacing = 10;  //Min spacing between cores in x and y direction
	public int spacingRetries = 50;  //Max attempts to generate more evenly spaced cores
	public int maxTypeDisparity = 2; //Max disparity amongst core types
	public int typeRetries = 50;     //Max attempts to generate more evenly numbered core types

	//Private global variables
	private float hexWidth;
	private float hexHeight;
	private int currType;
	private bool[,] occArray;    //Keeps track of which tiles have been placed
	private Vector3[] coreArray; //Keeps track of each core's coords and hex type

	//Method to initialise Hexagon width and height
	void setSizes()
	{
		//Height and width determined by Plains tile
		//Assumed that tiles are the same size
		hexWidth = Hex1.renderer.bounds.size.x;
		hexHeight = Hex1.renderer.bounds.size.z;
	}

	//Initialize arrays to keep track which tiles have already been generated
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
	
	//Method used to convert hex grid coordinates to game world coordinates
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

	/**************************************************************************************
	 * This method loops, generating core locations in coreArray, until the amount of cores
	 * denoted by the public variable biomeCores all have their locations assigned.
	 * 
	 * For each new core, a random position is generated within the bounds of the grid, as
	 * set by the public variables, gridWidthInHexes and gridHeightInHexes.
	 * 
	 * Once a location is generated, the method loops through all of the previously 
	 * generated cores to see if the new core is within the minimum spacing between core
	 * (specified by the public variable, minCoreSpacing) in both the x and y(z) directions.
	 * 
	 * If the new core is too close to another core, it is regenerated and rechecked.
	 * This process is repeated until either a location far enough from the other cores is
	 * generated or the max retry limit (denoted by the public variable, spacingRetries) is
	 * reached.
	 * ************************************************************************************/
	void generateCoreLocations()
	{
		//Loop until all cores are generated
		for (int i = 0; i < biomeCores; i++) 
		{
			//Generate a random location for this core
			coreArray [i] = new Vector3 ((float)Random.Range (0, gridWidthInHexes), 
			                             (float)Random.Range (0, gridHeightInHexes), 0f);
			
			//Make sure it's not right next to another one
			bool redo = false;
			for(int x = 0; x < i; x++)
			{
				if((Mathf.Abs(coreArray[x].x - coreArray[i].x) < minCoreSpacing) &&
				   (Mathf.Abs(coreArray[x].y - coreArray[i].y) < minCoreSpacing))
					redo = true;
			}

			//If too close, keep retrying
			int j = 0; //counter to make sure there is not an infinite loop
			while((j < spacingRetries) && redo)
			{
				coreArray [i] = new Vector3 ((float)Random.Range (0, gridWidthInHexes), 
				                             (float)Random.Range (0, gridHeightInHexes), 0f);
				
				for(int x = 0; x < i; x++)
				{
					if((Mathf.Abs(coreArray[x].x - coreArray[i].x) < minCoreSpacing) &&
					   (Mathf.Abs(coreArray[x].y - coreArray[i].y) < minCoreSpacing))
						redo = true;
					else
						redo = false;
				}
				
				j++;
			}
		}
	}

	void setCoreTypes()
	{
		//Counters for each core type
		int plainCount = 0;
		int forestCount = 0;
		int desertCount = 0;

		for (int i = 0; i < biomeCores; i++)
		{
			int hexType;
			bool redo = false;
			int j = 0;
			do
			{
				j++;
				hexType = Random.Range(0, 3);
				switch(hexType)
				{
				case 0:
					if(((plainCount - forestCount) >= maxTypeDisparity) &&
					   ((plainCount - desertCount) >= maxTypeDisparity))
						redo = true;
					break;
				case 1:
					if(((forestCount - plainCount) >= maxTypeDisparity) &&
					   ((forestCount - desertCount) >= maxTypeDisparity))
						redo = true;
					break;
				case 2:
					if(((desertCount - forestCount) >= maxTypeDisparity) &&
					   ((desertCount - plainCount) >= maxTypeDisparity))
						redo = true;
					break;
				default:
					break;
				}
				
			}while(redo && (j <= typeRetries));
			
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
			
			//Set core hex's position to its corresponding world coordinate
			coreHex.transform.position = calcWorldCoord(new Vector2(coreArray[i].x, coreArray[i].y));
			
			//Mark core tiles as occupied
			occArray[(int)coreArray[i].x, (int)coreArray[i].y] = true;
		}
	}

	//Method that initialises and positions all the tiles
	void createGrid()
	{
		//Randomly generate core tile locations
		generateCoreLocations ();

		//Set type and place each core
		setCoreTypes ();

		bool[] expCores = new bool[biomeCores]; //which cores are expired
		int expCount = 0; //count of expired cores
		//cores expire when all tiles in the current ring are already occupied
		int ring = 0; // current ring
		while (expCount < biomeCores) 
		{
			ring++; //increment ring number

			for(int i = 0; i < biomeCores; i++)
			{
				if (expCores[i])
					continue; //If this core has expired skip this iteration

				Vector3 currCore = coreArray[i];
				int hexType = (int)currCore.z;
				Vector2 currPos;
				int tilesPlaced = 0; //how many tiles were able to be placed in this ring
								 	 //cores expire when this number is 0 after the whole ring is tried
				
				//if core is in even row number top and bottom x coords are -1
				bool isEven;
				if ((currCore.y % 2) == 0)
					isEven = true;
				else
					isEven = false;

				currPos = new Vector2(currCore.x + ring, currCore.y);
				int sidePos = 1;

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
						//hex.transform.parent = hexGridGO.transform;

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
							//hex.transform.parent = hexGridGO.transform;
							
							occArray[(int)nextPos.x, (int)nextPos.y] = true;

							//Track that this tile was able to be placed
							tilesPlaced++;
						}

						currPos = nextPos;
						sidePos++;
					}
					sidePos = 0;
				}
				//if the none of the ring could be placed expire core
				if(tilesPlaced == 0)
				{
					expCores[i] = true;
					expCount++;
				}
			}
		}
		
		for (int y = 0; y < gridHeightInHexes; y++)
		{
			for (int x = 0; x < gridWidthInHexes; x++)
			{
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