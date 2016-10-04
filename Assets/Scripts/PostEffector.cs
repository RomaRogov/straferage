using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PostEffector : MonoBehaviour
{

    private Material mat;

	// Use this for initialization
	void Awake ()
    {
        mat = new Material(Shader.Find("Hidden/CRTLens"));
	}
	
	// Update is called once per frame
	void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
