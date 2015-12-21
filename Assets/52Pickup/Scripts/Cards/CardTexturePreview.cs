using UnityEngine;
using System.Collections;

public class CardTexturePreview : MonoBehaviour {

    public int deletemeCardIndex;

    public GameObject targetObject;
    public UnityEngine.UI.Image targetUIImage;

    // Use this for initialization
    void Start () {
        //setCardTextureByIndexCopying(0);
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
        
        /*
        cardFace.GetComponent<UnityEngine.UI.Image>().material.SetTextureOffset("_MainTex", textureCenter);
        cardFace.GetComponent<UnityEngine.UI.Image>().material.SetTextureScale("_MainTex", textureCenter);
        */
    }

    public void setCardTextureByIndexCopying(int i)
    {
        CardSetManager cardSetManager = GameObject.Find("CardSetManager").GetComponent<CardSetManager>();
 
        if(i > cardSetManager.cardCount)
        {
        throw new System.Exception("HoverHandle index greater than size of card face texture");
        }

        targetUIImage.GetComponent<UnityEngine.UI.Image>().material.SetTextureScale("_MainTex", new Vector2(1/cardSetManager.CardTextureDimensions.x, 1 / cardSetManager.CardTextureDimensions.y) );

        float x = i / cardSetManager.CardTextureDimensions.x;
        float y = i / cardSetManager.CardTextureDimensions.y;

        //TODO: This is opting the card out of batching
        //http://docs.unity3d.com/ScriptReference/Mesh-uv.html
        targetUIImage.material.SetTextureOffset("_MainTex", new Vector2(x, y));
    }
}
