using UnityEngine;
using System.Collections;

public class ScrollingTexture : MonoBehaviour {
	public Vector2 uvSpeed = new Vector2(0.0f, 1.0f);
	public Vector2 uvOffset = Vector2.zero;

	void LateUpdate() {
		uvOffset += (uvSpeed * Time.deltaTime);
		GetComponent<MeshRenderer>().materials[0].SetTextureOffset("_BaseMap", uvOffset);
	}
}