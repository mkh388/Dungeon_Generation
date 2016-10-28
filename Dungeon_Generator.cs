using UnityEngine;
using System.Collections;

public class Dungeon_Generator : MonoBehaviour {
	public GameObject tile;
	public int sizeX, sizeY;
	public int[,] dungeon;
	public int numRooms;
	private int numRoomsCreated = 0;
	public int roomSize;
	public int roomVariability;
	public int numCorridors;
	public int corridorLength;
	public Room[] roomList;
	private bool roomsSetup, corridorsSetup, doorsSetup, decorationsSetup=false;
	private int offsetLeft, offsetRight,offsetTop,offsetBottom;
	public class Room 
	{
		public Vector2 location;
		public int width;
		public int length;
		public Vector2 entrance;
		public Vector2 exit;
	}
	public enum tileType
	{
		wall, floor, decoration, door
	};
	public GameObject[,] dungeonTiles;
	// Use this for initialization
	void Start () {
		GameObject tileHolder = new GameObject ("TileHolder");
		//Start with all tile types as walls
		dungeon = new int[sizeX,sizeY];
		dungeonTiles =new GameObject[sizeX,sizeY];
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				dungeon [i, j] = (int)tileType.wall;
				dungeonTiles[i,j] = (GameObject)Instantiate (tile, new Vector3 (i, 0, j), Quaternion.Euler(90f,0,0));
				dungeonTiles [i, j].transform.parent = tileHolder.transform;
			}
		}

		roomList = new Room[numRooms];
		StartCoroutine(SetupRooms ());
	}

	public IEnumerator SetupRooms()
	{
		int trys = 0;
		for (int i = 0; i < numRooms; i++) {
			trys += 1;
			if (trys > 100) {
				Debug.Log ("Finished");
				yield break;
			}
			yield return new WaitForSeconds (1f);
			Room room = new Room ();
			int length = roomSize + Random.Range (-roomVariability, roomVariability);
			int width = roomSize + Random.Range (-roomVariability, roomVariability);
			Vector2 location = new Vector2 (Random.Range (width, sizeX - width), Random.Range (length, sizeY - length));
			room.location = location;
			room.width = width;
			room.length = length;
			//Check for overlap
			bool isOverlap = false;
			if ((int)location.y == 0)
				offsetLeft = 0;
			else
				offsetLeft = 1;
			if ((int)location.x == 0)
				offsetBottom = 0;
			else
				offsetBottom=1;

			if ((int)location.y + length == roomSize)
				offsetRight = 0;
			else
				offsetRight = 1;

			if ((int)location.x + width == roomSize)
				offsetTop = 0;
			else
				offsetTop = 1;
			
			for (int j = (int)location.y-offsetLeft; j < (int)location.y + length+offsetRight; j++) {
				for (int k = (int)location.x-offsetTop; k < (int)location.x + width+offsetBottom; k++) {
					if (dungeon [k, j] == (int)tileType.floor) {
						isOverlap = true;
					}
				}
			}
			//Check if this room overlaps with another room, if so try again (we only try 100 times)
			if (isOverlap) {
				i -= 1;
				continue;
			}

			//If it doesn't overlap it becomes a room with the bottom left corner at (Vector2)location it has a width of (int)width and a length of (int)length
			else {
				Debug.Log ("Width is " + width + ", and length is " + length);
				for (int j = (int)location.y; j < (int)location.y + length; j++) {
					for (int k = (int)location.x; k < (int)location.x + width; k++) {
						dungeon [k, j] = (int)tileType.floor;
					}
				}
				roomList [i] = room;
			}
		}
	}



}
