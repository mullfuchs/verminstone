using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public MeshFilter walls;

	public squareGrid _squareGrid;

	public MeshFilter caves;

	public MeshCollider wallCollider;

	List<Vector3> verticies;
	List<int> triangles;


	Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();

	List<List<int>> outlines = new List<List<int>>();
	HashSet<int> checkedVerticies = new HashSet<int> ();

	public void generateMesh(int[,] map, float squareSize){

		outlines.Clear ();
		checkedVerticies.Clear ();

		triangleDictionary.Clear ();

		_squareGrid = new squareGrid (map, squareSize);

		verticies = new List<Vector3>();
		triangles = new List<int>();

		for (int x = 0; x < _squareGrid.squares.GetLength (0); x++) {
			for (int y = 0; y < _squareGrid.squares.GetLength (1); y++) {
				triangulateSquare(_squareGrid.squares[x,y]);
			}
		}

		Mesh mesh = new Mesh ();
		caves.mesh = mesh;

		mesh.vertices = verticies.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		Vector2[] UVs = new Vector2[verticies.Count];
		for (int i = 0; i < verticies.Count; i++) {
			float percentX = Mathf.InverseLerp (-map.GetLength (0) / 2 + squareSize, map.GetLength (0) / 2 + squareSize, verticies [i].x);
			float percentY = Mathf.InverseLerp (-map.GetLength (0) / 2 + squareSize, map.GetLength (0) / 2 + squareSize, verticies [i].z);
			UVs [i] = new Vector2 (percentX, percentY);
		}
		mesh.uv = UVs;

		CreateWallMesh ();
	}

	void CreateWallMesh(){

		CalculateMeshOutlines ();

		List<Vector3> wallVerticies = new List<Vector3> ();
		List<int> wallTriangles = new List<int> ();
		Mesh wallMesh = new Mesh ();
		float wallHeigh = 5.0f;

		foreach (List<int> outline in outlines) {
			for (int i = 0; i < outline.Count - 1; i++) {
				int startIndex = wallVerticies.Count;
				wallVerticies.Add (verticies [outline [i]]);
				wallVerticies.Add (verticies [outline [i + 1]]);
				wallVerticies.Add (verticies [outline [i]]  - Vector3.up * wallHeigh);
				wallVerticies.Add (verticies [outline [i + 1]] - Vector3.up * wallHeigh);

				wallTriangles.Add (startIndex + 0);
				wallTriangles.Add (startIndex + 2);
				wallTriangles.Add (startIndex + 3);

				wallTriangles.Add (startIndex + 3);
				wallTriangles.Add (startIndex + 1);
				wallTriangles.Add (startIndex + 0);
			}
		}
		wallMesh.vertices = wallVerticies.ToArray ();
		wallMesh.triangles = wallTriangles.ToArray ();

		wallMesh.RecalculateNormals ();

		float scaleFactor = 1;

		Vector2[] UVs = new Vector2[ wallMesh.vertices.Length ];

		for (int i = 0; i < wallMesh.vertices.Length; i++) {
			UVs[i] =  new Vector2(wallMesh.vertices[i].x, wallMesh.vertices[i].y);
		}

		wallMesh.uv = UVs;


		walls.mesh = wallMesh;


//		if (gameObject.GetComponent<MeshCollider> () == null) {
//			MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider> ();
//		} else {
//			
//		}
		wallCollider.sharedMesh = null;
		wallCollider.sharedMesh = wallMesh;
	}

	void triangulateSquare(square _square){
		switch (_square.configuration) {
		case 0:
			break;

			// 1 points:
		case 1:
			MeshFromPoints(_square.centerLeft, _square.centerBottom, _square.bottomLeft);
			break;
		case 2:
			MeshFromPoints(_square.bottomRight, _square.centerBottom, _square.centerRight);
			break;
		case 4:
			MeshFromPoints(_square.topRight, _square.centerRight, _square.centerTop);
			break;
		case 8:
			MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerLeft);
			break;

			// 2 points:
		case 3:
			MeshFromPoints(_square.centerRight, _square.bottomRight, _square.bottomLeft, _square.centerLeft);
			break;
		case 6:
			MeshFromPoints(_square.centerTop, _square.topRight, _square.bottomRight, _square.centerBottom);
			break;
		case 9:
			MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerBottom, _square.bottomLeft);
			break;
		case 12:
			MeshFromPoints(_square.topLeft, _square.topRight, _square.centerRight, _square.centerLeft);
			break;
		case 5:
			MeshFromPoints(_square.centerTop, _square.topRight, _square.centerRight, _square.centerBottom, _square.bottomLeft, _square.centerLeft);
			break;
		case 10:
			MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerRight, _square.bottomRight, _square.centerBottom, _square.centerLeft);
			break;

			// 3 point:
		case 7:
			MeshFromPoints(_square.centerTop, _square.topRight, _square.bottomRight, _square.bottomLeft, _square.centerLeft);
			break;
		case 11:
			MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerRight, _square.bottomRight, _square.bottomLeft);
			break;
		case 13:
			MeshFromPoints(_square.topLeft, _square.topRight, _square.centerRight, _square.centerBottom, _square.bottomLeft);
			break;
		case 14:
			MeshFromPoints(_square.topLeft, _square.topRight, _square.bottomRight, _square.centerBottom, _square.centerLeft);
			break;

			// 4 point:
		case 15:
			MeshFromPoints (_square.topLeft, _square.topRight, _square.bottomRight, _square.bottomLeft);
			checkedVerticies.Add (_square.topLeft.vertexIndex);
			checkedVerticies.Add (_square.topRight.vertexIndex);
			checkedVerticies.Add (_square.bottomRight.vertexIndex);
			checkedVerticies.Add (_square.bottomLeft.vertexIndex);
			break;

		}
	}

	void MeshFromPoints(params node[] points){
		AssignVerticies (points);

		if (points.Length >= 3)
			CreateTriangle (points [0], points [1], points [2]);
		if (points.Length >= 4)
			CreateTriangle (points [0], points [2], points [3]);
		if (points.Length >= 5)
			CreateTriangle (points [0], points [3], points [4]);
		if (points.Length >= 6)
			CreateTriangle (points [0], points [4], points [5]);
		
	}

	void AssignVerticies(node[] points){
		for (int i = 0; i < points.Length; i++) {
			if (points [i].vertexIndex == -1) {
				points [i].vertexIndex = verticies.Count;
				verticies.Add (points [i].position);
			}
		}
	}

	void CreateTriangle(node a, node b, node c){
		triangles.Add (a.vertexIndex);
		triangles.Add (b.vertexIndex);
		triangles.Add (c.vertexIndex);

		Triangle triangle = new Triangle (a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriangleToDictionary (triangle.vertexIndexA, triangle);
		AddTriangleToDictionary (triangle.vertexIndexB, triangle);
		AddTriangleToDictionary (triangle.vertexIndexC, triangle);
	}

	void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle){
		if (triangleDictionary.ContainsKey(vertexIndexKey)) {
			triangleDictionary[vertexIndexKey].Add(triangle);
		}
		else{
			List<Triangle> triangleList = new List<Triangle> ();
			triangleList.Add (triangle);
			triangleDictionary.Add (vertexIndexKey, triangleList);
		}
	}

	void CalculateMeshOutlines(){
		for (int vertexIndex = 0; vertexIndex < verticies.Count; vertexIndex++) {
			if (!checkedVerticies.Contains (vertexIndex)) {
				int newOutlineVertex = GetConnectedOutlineVertex (vertexIndex);
				if (newOutlineVertex != -1) {
					checkedVerticies.Add (vertexIndex);
					List<int> newOutline = new List<int> ();
					newOutline.Add (vertexIndex);
					outlines.Add (newOutline);
					FollowOutline (newOutlineVertex, outlines.Count - 1);
					outlines [outlines.Count - 1].Add (vertexIndex);
				}
			}
		}
	}

	void FollowOutline(int vertexIndex, int outlineIndex){
		outlines [outlineIndex].Add (vertexIndex);
		checkedVerticies.Add (vertexIndex);
		int nextVertexIndex = GetConnectedOutlineVertex (vertexIndex);
		if (nextVertexIndex != -1) {
			FollowOutline (nextVertexIndex, outlineIndex);
		}
	}

	int GetConnectedOutlineVertex(int vertexIndex){
		List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];

		for (int i = 0; i < trianglesContainingVertex.Count; i++) {
			Triangle triangle = trianglesContainingVertex [i];

			for (int j = 0; j < 3; j++) {
				int vertexB = triangle[j];
				if (vertexB != vertexIndex && !checkedVerticies.Contains(vertexB)) {
					if (IsOutlineEdge (vertexIndex, vertexB)) {
						return vertexB;
					}	
				}
			}
		}
		return -1;
	}

	bool IsOutlineEdge(int vertexA, int vertexB){
		List<Triangle> trianglesContainingVertexA = triangleDictionary [vertexA];
		int shareTriangleCount = 0;

		for (int i = 0; i < trianglesContainingVertexA.Count; i++) {
			if(trianglesContainingVertexA[i].Contains(vertexB)){
				shareTriangleCount ++;
				if (shareTriangleCount > 1) {
					break;
				}
			}
		}

		return shareTriangleCount == 1;
	}
		
	struct Triangle{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;

		int[] verticies;

		public Triangle(int a, int b, int c){
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;
			verticies = new int[3];
			verticies[0] = a;
			verticies[1] = b;
			verticies[2] = c;
		}

		public int this[int i]{
			get{
				return verticies [i];
			}
		}

		public bool Contains(int vertexIndex){
			return (vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC);
		}
	}

	public class squareGrid{
		public square[,] squares;

		public squareGrid(int[,] map, float squareSize){
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);
			float mapWidth = nodeCountX * squareSize;
			float mapHeight = nodeCountY * squareSize;

			controlNode[,] controlNodes = new controlNode[nodeCountX,nodeCountY];

			for(int x = 0; x < nodeCountX; x++){
				for(int y = 0; y < nodeCountY; y++){
					Vector3 position = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, 0, -mapHeight/2 + y * squareSize + squareSize/2);
					controlNodes[x,y] = new controlNode(position, map[x,y] == 1, squareSize);
				}
			}

			squares = new square[nodeCountX - 1, nodeCountY - 1];

			for(int x = 0; x < nodeCountX - 1; x++){
				for(int y = 0; y < nodeCountY - 1; y++){
					squares[x,y] = new square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
				}
			}

		}
	}

	public class square{
		public controlNode topLeft, topRight, bottomLeft, bottomRight;
		public node centerTop, centerRight, centerBottom, centerLeft;

		public int configuration = 0;

		public square(controlNode _topLeft, controlNode _topRight, controlNode _bottomRight, controlNode _bottomLeft){
			topLeft = _topLeft;
			topRight = _topRight;
			bottomLeft = _bottomLeft;
			bottomRight = _bottomRight;

			centerTop = topLeft.right;
			centerRight = bottomRight.above;
			centerBottom = bottomLeft.right;
			centerLeft = bottomLeft.above;

			if(topLeft.active)
				configuration += 8;
			if(topRight.active)
				configuration += 4;
			if(bottomRight.active)
				configuration += 2;
			if(bottomLeft.active)
				configuration += 1;

		}
	}

	public class node{
		public Vector3 position;
		public int vertexIndex = -1;

		public node(Vector3 _pos){
			position = _pos;
		}
	}

	public class controlNode : node{
		public bool active;
		public node above, right;

		public controlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos){
			active = _active;
			above = new node(position + Vector3.forward * squareSize/2f);
			right = new node(position + Vector3.right * squareSize/2f);
		}
	}
		

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
