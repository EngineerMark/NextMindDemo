using NextMind.NeuroTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCard : MonoBehaviour
{
    public int CardIndex;
    public Sprite sprite;
    public string CardIdentifier;
    public MatchManager matchManager;

    public NeuroTag ntag;
    public Image image;

    public void Awake(){
        ntag = GetComponent<NeuroTag>();
        image = GetComponent<Image>();

        ntag.onTriggered.AddListener(() => matchManager.RevealCard(this));
    }

    public void SetState(bool s)
    {
        ntag.enabled = s;
        image.enabled = s;
    }

    public void SetTagState(bool s)
    {
        ntag.enabled = s;
    }

    public void SetImageState(bool s){
        image.sprite = s ? sprite : null;
    }
}
