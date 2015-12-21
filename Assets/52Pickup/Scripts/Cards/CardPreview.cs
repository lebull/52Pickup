using UnityEngine;
using System.Collections;

public class CardPreview : MonoBehaviour {

    public void previewCard(GameObject cardObject)
    {
        CardDeck cardDeck = cardObject.GetComponent<CardDeck>();
        if (!cardDeck.inverted && cardDeck.cards.Count == 1)
        {
            GetComponent<CardTexturePreview>().enabled = true;
            GetComponent<CardTexturePreview>().setCardTextureByIndexCopying(cardDeck.cards[0]);
            transform.position = cardObject.transform.position + Vector3.up * transform.lossyScale.z / 2;
        }
        else
        {
            GetComponent<CardTexturePreview>().enabled = false;
        }

    }
}
