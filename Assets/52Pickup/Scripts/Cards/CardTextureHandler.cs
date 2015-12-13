using UnityEngine;
using System.Collections;

public class CardTextureHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //setCardTextureByIndex(0);
    }

    public void setCardTextureByIndex(int i)
    {
        CardSetManager cardSetManager = GameObject.Find("CardSetManager").GetComponent<CardSetManager>();

        if(i > cardSetManager.cardCount)
        {
            throw new System.Exception("HoverHandle index greater than size of card face texture");
        }

        float x = (float)i / cardSetManager.CardTextureDimensions.x;
        float y = (float)i / cardSetManager.CardTextureDimensions.y;

        //TODO: This is opting the card out of batching
        //http://docs.unity3d.com/ScriptReference/Mesh-uv.html
        transform.FindChild("CardFace").GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(x, y));
    }
}
