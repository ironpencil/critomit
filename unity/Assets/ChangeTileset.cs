using UnityEngine;
using System.Collections;

public class ChangeTileset : MonoBehaviour {

    public Material applyToMaterial;
    public Material newMaterial;
    public string textureID = "_MainText";
    public Texture textureToApply;
    public MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {

     
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [ContextMenu("Apply Texture")]
    void ApplyTexture()
    {
        applyToMaterial.SetTexture(textureID, textureToApply);
        applyToMaterial.mainTexture = textureToApply;
        meshRenderer.material = applyToMaterial;
    }

    [ContextMenu("Apply Material")]
    void ApplyMaterial()
    {
        meshRenderer.material = newMaterial;
    }
}
