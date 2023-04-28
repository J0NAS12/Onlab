using UnityEngine;
[System.Serializable]
public abstract class MazeCellEdge : MonoBehaviour {

	private MazeCell cell, otherCell;
	
	private MazeDirection direction;

	public virtual void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}

    public virtual int Type { get; }
}