using UnityEngine;
using System.Collections;

public class Letterss : MonoBehaviour {
	
    public bool utilized = false;
    public bool identified = false;
	public TextMesh letter;
	public int gridX, gridY;

    void Start() {
        GetComponent<Renderer>().materials[0].color = WordSearch1.Instance.defaultTint;
    }
    
    void Update() {
        if (WordSearch1.Instance.ready) {
            if (!utilized && WordSearch1.Instance.current == gameObject) {
                WordSearch1.Instance.selected.Add(this.gameObject);
                GetComponent<Renderer>().materials[0].color = WordSearch1.Instance.mouseoverTint;
                WordSearch1.Instance.selectedString += letter.text;
                utilized = true;
            }
        }

        if (identified) {
			if (GetComponent<Renderer>().materials[0].color != WordSearch1.Instance.identifiedTint) {
				GetComponent<Renderer>().materials[0].color = WordSearch1.Instance.identifiedTint;
			} 
			return;
        }

        if (Input.GetMouseButtonUp(0)) {
            utilized = false;
			if (GetComponent<Renderer>().materials[0].color != WordSearch1.Instance.defaultTint) {
				GetComponent<Renderer>().materials[0].color = WordSearch1.Instance.defaultTint;
			}
        }
    }
}
