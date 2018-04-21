using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoSingleton<CardManager>
{
    [SerializeField] private Player _player;
    [SerializeField] private DropTarget[] _availableTargets;
    [SerializeField] private Transform[] _availableTargetsHints;
    [SerializeField] private DropTarget _handDropTarget;
    [SerializeField] private LayoutGroup _layoutGroup;
    [SerializeField] private Card[] _availableCards;
    
    private CardVisitor _executeCardVisitor;

    private void Awake()
    {
        DefineSingleton(this);
    }

    private void Start()
    {
        if (_player == null)
            _player = FindObjectOfType<Player>();

        _executeCardVisitor = new ExecuteCardVisitor(this, _player);
        AddEventListeners();

        // TEST
        for (var i = 0; i < 5; i++)
        {
            PutRandomCardInHand();
        }
    }

    protected override void OnDestroy()
    {
        RemoveEventListeners();

        base.OnDestroy();        
    }

    public Card PutRandomCardInHand()
    {
        var randomCardPrefab = _availableCards[Random.Range(0, _availableCards.Length)];
        var cardGameObject = Instantiate(randomCardPrefab.gameObject);
        var card = cardGameObject.GetComponent<Card>();
        if (card == null)
            return null;

        card.CurrentDropTarget = _handDropTarget;
        card.PossibleTargets = _availableTargetsHints;
        card.LayoutGroup = _layoutGroup;
        card.Cost = Random.Range(1, 2);

        card.Randomize();

        card.transform.SetParent(_layoutGroup.transform, false);

        return card;
    }

    public void ExecuteCards(Card[] cards)
    {
        StartCoroutine(ExecuteCardsCoroutine(cards));
    }

    private IEnumerator ExecuteCardsCoroutine(Card[] cards)
    {
        var executeIndex = 0;
        while (cards.Length > executeIndex)
        {            
            cards[executeIndex].Accept(_executeCardVisitor);

            yield return new WaitUntil(() => !_player.Movement.IsBusy);

            executeIndex++;
        }
    }

    //[Obsolete]
    //public T RetrieveCard<T>() where T : Card
    //{
    //    var cardPrototype = (T)_availableCards.First(item => item.GetType() == typeof(T));
    //    var cardGameObject = Instantiate(cardPrototype.gameObject);
    //    var card = cardGameObject.GetComponent<T>();
    //    if (card == null)
    //        return null;

    //    card.PossibleTargets = _availableTargets.Select(target => target.transform).ToArray();
    //    card.LayoutGroup = _layoutGroup;

    //    card.Randomize();

    //    card.transform.SetParent(_layoutGroup.transform, false);

    //    return card;
    //}    

    private void AddEventListeners()
    {
        foreach (var availableTarget in _availableTargets)
        {
            availableTarget.AddEventListener(DropTarget.Dropped, OnDropTargetDropped);
        }
    }

    private void RemoveEventListeners()
    {
        foreach (var availableTarget in _availableTargets)
        {
            if (availableTarget != null)
                availableTarget.RemoveEventListener(DropTarget.Dropped, OnDropTargetDropped);
        }
    }

    // When the drop target receives a drop (card)
    private void OnDropTargetDropped(EventObject eventObject)
    {
        //var droppedGameObject = (GameObject)eventObject.Data;
        //var card = droppedGameObject.GetComponent<Card>();
        //if (card != null)
        //    card.Accept(_executeCardVisitor);

        // todo: update stamina?
    }
}
