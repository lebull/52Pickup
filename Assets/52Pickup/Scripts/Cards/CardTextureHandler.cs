using UnityEngine;
using System.Collections;

public class CardTextureHandler : MonoBehaviour {

    public int deletemeCardIndex;

	// Use this for initialization
	void Start () {
        //setCardTextureByIndex(0);
    }

    public void setCardTextureByIndex(int cardIndex)
    {
        deletemeCardIndex = cardIndex;

        CardSetManager cardSetManager = FindObjectOfType<CardSetManager>();

        if(cardIndex > cardSetManager.cardCount)
        {
            throw new System.Exception("HoverHandle index greater than size of card face texture");
        }

        GameObject cardFace = transform.FindChild("CardFace").gameObject;

        
        Vector2 textureCenter = new Vector2(
            0.5f + (int)(cardIndex % cardSetManager.CardTextureDimensions.x),
            0.5f + (int)(cardIndex / cardSetManager.CardTextureDimensions.x)
        );

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = cardFace.GetComponent<MeshFilter>().mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(
                textureCenter.x + (vertices[i].x),
                textureCenter.y + (vertices[i].y)
                );
        }
        cardFace.GetComponent<MeshFilter>().mesh.uv = uvs;
    }
}
