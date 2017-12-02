using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

	public Cell NeighborUpLeft;
	public Cell NeighborUpRight;
	public Cell NeighborRight;
	public Cell NeighborDownRight;
	public Cell NeighborDownLeft;
	public Cell NeighborLeft;

	public ControlStates State = ControlStates.Locked;
	public CellTypes Type = CellTypes.Grass;

	public string Info = "This cell is boring.";		//Temporary, will display better systems 

	public void Interact (int Button) {
		if (State == ControlStates.Available) {
			State = ControlStates.Owned;
			UpdateGraphics ();
		}

		if (State == ControlStates.Owned) {
			Debug.Log (Info);
		}
	}

	private void UpdateGraphics () {
		switch (State) {
		case ControlStates.Owned:
			GetComponent<Image> ().color = Color.green;
			break;
		case ControlStates.Available:
			GetComponent<Image> ().color = Color.yellow;
			break;
		case ControlStates.Locked:
			GetComponent<Image> ().color = Color.red;
			break;
		}
	}
}

public enum ControlStates {
	Owned,
	Available,
	Locked,
	Hidden
}

public enum CellTypes {
	City,
	Town,
	Road,
	Grass,
	Forest,
	Library,
	Crypt,
	VoidPortal
}