using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public int totalCards;
    public Sprite[] imageDatabase;
    public GameObject cardPrefab;
    public RectTransform cardGridView;
    public GameObject restartButton;

    private List<MatchCard> cards;

    int selectedIndex = -1;
    int finishedCards = 0;

    public void Reset(){
        restartButton.SetActive(false);
        if (cards != null)
        {
            cards.ForEach(card => Destroy(card.gameObject));
        }
        cards = new List<MatchCard>();
        finishedCards = 0;

        int images = totalCards / 2;
        List<Sprite> sprites = new List<Sprite>();
        Queue<Sprite> all_sprites = new Queue<Sprite>(imageDatabase);
        for (int i = 0; i < images; i++)
        {
            Sprite s = all_sprites.Dequeue();
            sprites.Add(s);
            sprites.Add(s);
        }
        for (int i = 0; i < Random.Range(5, 20); i++)
        {
            sprites.Shuffle();
            sprites.Shuffle();
            sprites.Shuffle();
        }
        Queue<Sprite> sprite_queue = new Queue<Sprite>(sprites);

        for (int i = 0; i < totalCards; i++){
            Sprite s = sprite_queue.Dequeue();
            GameObject card = Instantiate(cardPrefab);
            MatchCard matchCard = card.GetComponent<MatchCard>();
            Image matchCardImage = card.GetComponent<Image>();

            matchCard.CardIndex = i;
            matchCardImage.sprite = s;
            matchCard.sprite = s;
            matchCard.CardIdentifier = s.name;
            matchCard.matchManager = this;
            matchCard.SetImageState(false);

            card.transform.SetParent(cardGridView);
            cards.Add(matchCard);
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector3(card.transform.localPosition.x, card.transform.localPosition.y, 0);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(cardGridView);
    }

    public void RevealCard(MatchCard card){
        card.SetImageState(true);
        card.SetTagState(false);
        if (selectedIndex==-1){
            selectedIndex = card.CardIndex;
        }else{
            MatchCard compareCard = cards.Find(c=>c.CardIndex==selectedIndex);
            ComparisonResult(card, compareCard, card.CardIdentifier==compareCard.CardIdentifier);
            selectedIndex = -1;

            if(finishedCards==totalCards){
                //finished game
                restartButton.SetActive(true);
            }
        }
    }

    public void ComparisonResult(MatchCard cardA, MatchCard cardB, bool matching)
    {
        if(matching){
            cardA.SetState(false);
            cardB.SetState(false);
            Debug.Log("Matching!");
            finishedCards += 2;
        }
        else
        {
            cardA.SetImageState(false);
            cardB.SetImageState(false);
            cardA.SetTagState(true);
            cardB.SetTagState(true);
            Debug.Log("Not matching");
        }
    }
}
