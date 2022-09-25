using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour {

	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0, 100)]
	public int randomFillPercent;

	int[,] map;


	System.Random prng;

	List<Room> roomList;

	void Start () {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		prng = new System.Random (seed.GetHashCode());

		//CreateMap ();	
	}

	public void initLevel(){
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		prng = new System.Random (seed.GetHashCode());
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			//CreateMap ();
		}
	}

	public int[,] MakeFloor(int fillPercent){
		return CreateMap (fillPercent);
	}

	int[,] CreateMap(int fillPercent){
		map = new int[width, height];

		if (fillPercent <= 0) {
			RandomFillMap ();
		} else {
			RandomFillMapPercent (fillPercent);
		}

		for (int i = 0; i < 5; i++) {
			SmoothMap ();
		}

		processMap ();

		int boarderSize = 5;

		int[,] borderedMap = new int[width + boarderSize * 2, height + boarderSize * 2];

		for (int x = 0; x < borderedMap.GetLength(0); x++) {
			for (int y = 0; y < borderedMap.GetLength(1); y++) {
				if (x >= boarderSize && x < width + boarderSize && y >= boarderSize && y < height + boarderSize) {
					borderedMap [x, y] = map [x - boarderSize, y - boarderSize];
				} else {
					borderedMap [x, y] = 1;
				}
			}
		}

		//MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		//meshGen.generateMesh (borderedMap, 1f);

		return borderedMap;
	}


	List<Coord> GetRegionTiles(int startX, int startY){
		List<Coord> tiles = new List<Coord> ();

		int[,] mapFlags = new int[width, height];
		int tileType = map [startX, startY];
		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue(new Coord(startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue ();
			tiles.Add (tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if(isInMapRange(x, y) && (y == tile.tileY || x == tile.tileX)){
						if (mapFlags [x, y] == 0 && map [x, y] == tileType) {
							mapFlags [x, y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}	
			}
		}

		return tiles;
	}

	void ConnectClosestRooms(List<Room> allRooms, bool forceAccessablityFromMainRoom = false){

		List<Room> roomListA = new List<Room> ();
		List<Room> roomListB = new List<Room> ();

		if (forceAccessablityFromMainRoom) {
			foreach (Room room in allRooms) {
				if (room.isAccessableFromMainRoom) {
					roomListB.Add (room);
				} else {
					roomListA.Add (room);
				}
			}
		} else {
			roomListA = allRooms;
			roomListB = allRooms;
		}

		int bestDistance = 0;

		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		Room bestRoomA = new Room ();
		Room bestRoomB = new Room ();
		bool possibleConnectionFound = false;

		foreach (Room roomA in roomListA) {
			if (!forceAccessablityFromMainRoom) {
				possibleConnectionFound = false;
				if (roomA.connectedRooms.Count > 0) {
					continue;
				}
			}

			foreach (Room roomB in roomListB) {
				if (roomA == roomB || roomA.isConnected(roomB)) {
					continue;
				}
				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
						Coord tileA = roomA.edgeTiles [tileIndexA];
						Coord tileB = roomB.edgeTiles [tileIndexB];
						int distanceBetweenRooms = (int)(Mathf.Pow( tileA.tileX - tileB.tileX, 2) + Mathf.Pow( tileA.tileY - tileB.tileY, 2));

						if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
							bestDistance = distanceBetweenRooms;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}

			if (possibleConnectionFound && !forceAccessablityFromMainRoom) {
				CreatePassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}

		if (possibleConnectionFound && forceAccessablityFromMainRoom) {
			CreatePassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms (allRooms, true);
		}


		if (!forceAccessablityFromMainRoom) {
			ConnectClosestRooms (allRooms, true);
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB){
		Room.connectRooms (roomA, roomB);
		Debug.DrawLine (coordToWorldPoint (tileA), coordToWorldPoint (tileB), Color.green, 100);

		List<Coord> line = GetLine (tileA, tileB);
		foreach (Coord c in line) {
			drawCircle (c, 1);
		}
	}

	void drawCircle(Coord c, int radius){
		for (int x = -radius; x <= radius; x++) {
			for (int y = -radius; y <= radius; y++) {
				if (x * x + y * y <= radius * radius) {
					int realX = c.tileX + x;
					int realY = c.tileY + y;
					if(isInMapRange(realX, realY)){
						map [realX, realY] = 0;
					}
				}
			}
		}
	}

	List<Coord> GetLine(Coord from, Coord to){
		List<Coord> line = new List<Coord> ();
		int x = from.tileX;
		int y = from.tileY;

		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		bool inverted = false;

		int step = (int)Mathf.Sign (dx);
		int gradientStep = (int)Math.Sign (dy);

		int longest = Math.Abs (dx);
		int shortest = Math.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Math.Abs (dy);
			shortest = Math.Abs (dx);
		
			step = (int)Math.Sign (dy);
			gradientStep = (int)Math.Sign (dx);
		}

		int gradeAccumulation = longest / 2;
		for (int i = 0; i < longest; i++) {
			line.Add(new Coord(x, y));

			if (inverted) {
				y += step;
			} else {
				x += step;
			}

			gradeAccumulation += shortest;
			if (gradeAccumulation >= longest) {
				if (inverted) {
					x += gradientStep;
				} else {
					y += gradientStep;
				}
				gradeAccumulation -= longest;
			}


		}

		return line;
	}

	Vector3 coordToWorldPoint(Coord tile){
		return new Vector3 (-width / 2f + 0.5f + tile.tileX, 2, -height / 2f + 0.5f + tile.tileY);

	}

	List<List<Coord>> GetRegions(int tileType){
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (mapFlags [x, y] == 0 && map [x, y] == tileType) {
					List<Coord> newRegion = GetRegionTiles (x, y);
					regions.Add (newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags [tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}

	void processMap(){
		List<List<Coord>> wallRegions = GetRegions (1);
		int wallThresholdSize = 50;

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					map [tile.tileX, tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);
		int roomThresholdSize = 50;
		List<Room> survivingRooms = new List<Room> ();


		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map [tile.tileX, tile.tileY] = 1;
				}
			} else {
				survivingRooms.Add (new Room (roomRegion, map));
			}
		}
		survivingRooms.Sort ();
        //TODO: FIX CAVE GEN STUFF
		survivingRooms [0].isMainRoom = true;
		survivingRooms [0].isAccessableFromMainRoom = true;

		ConnectClosestRooms (survivingRooms);
		roomList = survivingRooms;

		//gameObject.GetComponent<PlaceObjects> ().PopulateMap (GetRandomPointsInRooms (1));
	}
		

	bool isInMapRange(int x, int y){
		return (x >= 0 && x < width && y >= 0 && y < height);
	}

	void RandomFillMapPercent(int percentFill){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (prng.Next (0, 100) < percentFill) ? 1 : 0;
				}

			}
		}
	}

	void RandomFillMap(){
//		if (useRandomSeed) {
////			seed = Time.time.ToString();
////		}
////
		//System.Random prng = new System.Random (seed.GetHashCode());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (prng.Next (0, 100) < randomFillPercent) ? 1 : 0;
				}

			}
		}
	}

	void SmoothMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighborWallTiles = GetSurroundingWallCount (x, y);

				if (neighborWallTiles > 4)
					map [x, y] = 1;
				else if (neighborWallTiles < 4)
					map [x, y] = 0;
			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY){
		int WallCount = 0;
		for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++) {
			for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++) {

				if (isInMapRange(neighborX, neighborY)) {
					if (neighborX != gridX || neighborY != gridY) {
						WallCount += map [neighborX, neighborY];
					}
				} else {
					WallCount++;
				}
			}
		}
		return WallCount;
	}

	struct Coord {
		public int tileX;
		public int tileY;

		public Coord(int x, int y){
			tileX = x;
			tileY = y;
		}
	}

	class Room : IComparable<Room>{
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Coord> insideTiles;
		public List<Room> connectedRooms;

		public int roomSize;
		public bool isAccessableFromMainRoom;
		public bool isMainRoom;
        public int itemcount = 0;

		public Room(){
			
		}

		public Room(List<Coord> roomTiles, int[,] map){ 
			tiles = roomTiles;

			roomSize = tiles.Count;
			connectedRooms = new List<Room>();
			edgeTiles = new List<Coord>();
			insideTiles = new List<Coord>();
			foreach(Coord tile in tiles){
				for(int x = tile.tileX - 1; x <= tile.tileX + 1; x++){
					for(int y = tile.tileY - 1; y <= tile.tileY + 1; y++){
						if(x == tile.tileX || y == tile.tileY){
							if(map[x,y] == 1){
								edgeTiles.Add(tile);
							}

						}
						else{
							if(map[x,y] == 1){
								insideTiles.Add(tile);
							}
						}
					}
				}
			}
		}

		public void SetAccessableFromMainRoom(){
			if (!isAccessableFromMainRoom) {
				isAccessableFromMainRoom = true;
				foreach (Room connectedRoom in connectedRooms) {
					connectedRoom.SetAccessableFromMainRoom ();
				}
			}
		}

		public static void connectRooms(Room roomA, Room roomB){
			if (roomA.isAccessableFromMainRoom) {
				roomB.SetAccessableFromMainRoom ();
			} else if(roomB.isAccessableFromMainRoom) {
				roomA.SetAccessableFromMainRoom ();
			}
			roomA.connectedRooms.Add (roomB);
			roomB.connectedRooms.Add (roomA);
		}

		public bool isConnected(Room otherRoom){
			return connectedRooms.Contains(otherRoom);
		}

		public int CompareTo(Room otherRoom){
			return otherRoom.roomSize.CompareTo (roomSize);
		}
			
	}

	public List<Vector3> GetRandomPointsInRooms(int fillPercent){
		List<Vector3> coordList = new List<Vector3> ();

		if (roomList.Count > 0) {
			foreach (Room room in roomList) {
				foreach (Coord tile in room.tiles) {
					if (prng.Next (0, 1000) < fillPercent) {
						coordList.Add( coordToWorldPoint(tile));
					}
				}
			}
		}

		return coordList;
	}

	public List<Vector3> GetSetNumberOfRandomPointsInRooms(int numberOfPoints){
		List<Vector3> coordList = new List<Vector3> ();

		while (coordList.Count < numberOfPoints) {
			Room tempRoom = roomList.ElementAt (UnityEngine.Random.Range (0, roomList.Count));
			Coord tempTile = tempRoom.tiles.ElementAt (UnityEngine.Random.Range (0, tempRoom.tiles.Count));
            //maybe remoove that tile here
            tempRoom.tiles.Remove(tempTile);
            tempRoom.itemcount += 1;

			coordList.Add (coordToWorldPoint (tempTile));
		}

		return coordList;
	}

	public List<Vector3> GetListOfPointsInRooms(int numberOfPointsToGet){
		List<Vector3> coordList = new List<Vector3> ();	

		while (coordList.Count < numberOfPointsToGet) {
			foreach (Room room in roomList) {
				foreach (Coord tile in room.tiles) {
					if (prng.Next (0, 1000) < 1) {
						coordList.Add( coordToWorldPoint(tile));
					}
				}
			}
		}

		return coordList;
	}
		

	public Vector3 GetFloorEntryPoint(){
		int randRoom = UnityEngine.Random.Range (0, roomList.Count);
		Room entryRoom = roomList [ randRoom ];
		Coord entryTile = entryRoom.insideTiles.ElementAt( (int)(entryRoom.insideTiles.Count / 2) );

		return coordToWorldPoint (entryTile);
	}

	public Vector3 GetFloorExitPoint(){
		print ("room count: " + roomList.Count);
		int randRoom = UnityEngine.Random.Range (0, roomList.Count);
		Room exitRoom = roomList [ randRoom ];
		Coord exitTile = exitRoom.insideTiles.ElementAt( (int)(exitRoom.insideTiles.Count / 2)  );
		return coordToWorldPoint (exitTile);
	}

	public Vector3 GetRandomPointInRandomRoom(){
		if (roomList.Count <= 0) {
			print ("No more empty rooms! that's a problem!");
			return Vector3.zero;
		}

		Room randRoom;
		int roomIndex = UnityEngine.Random.Range (0, roomList.Count);
		randRoom = roomList[roomIndex];


		//get random room
		Coord randomSpot = randRoom.insideTiles.ElementAt( UnityEngine.Random.Range(0, randRoom.insideTiles.Count) );
		//testing this
		randRoom.insideTiles.Remove(randomSpot);
		if (randRoom.insideTiles.Count <= 0) {
			roomList.Remove (randRoom);
		}
		//get random point in that room
		//return it
		return coordToWorldPoint(randomSpot);
	}

    public Vector3 GetSmallestExclusiveRoom()
    {
        //find a point in the smallest room and remove it from the room list;
        if (roomList.Count <= 0)
        {
            print("No more empty rooms! that's a problem!");
            return Vector3.zero;
        }


        Room smallestRoom = roomList[0];
        foreach(Room candidate in roomList)
        {
            if(candidate.itemcount < smallestRoom.itemcount)
            {
                smallestRoom = candidate;
            }
        }

        //get middle spot room
        Coord middleSpot = smallestRoom.tiles.ElementAt(smallestRoom.tiles.Count/2);
        roomList.Remove(smallestRoom);

        return coordToWorldPoint(middleSpot);
    }


	public void RenderMap(int[,] mapToRender){
		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.generateMesh (mapToRender, 1.0f);
	}

}
