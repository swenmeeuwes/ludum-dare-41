using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class AvailableCardItem
{
    public Card Card;
    [Range(0, 100)] public int Propability;
}

public class CardManager : MonoSingletonEventDispatcher<CardManager>
{
    public static readonly string ExecutionDone = "CardManager.ExecutionDone";

    [SerializeField] private Player _player;
    [SerializeField] private DropTarget[] _availableTargets;
    [SerializeField] private Transform[] _availableTargetsHints;
    [SerializeField] private DropTarget _handDropTarget;
    [SerializeField] private LayoutGroup _layoutGroup;
    [SerializeField] private AvailableCardItem[] _availableCards;
    [SerializeField] private int _handSize = 5;
    
    private CardVisitor _executeCardVisitor;

    protected override void Awake()
    {
        base.Awake();

        DefineSingleton(this);
    }

    private void Start()
    {
        if (_player == null)
            _player = FindObjectOfType<Player>();

        _executeCardVisitor = new ExecuteCardVisitor(this, _player);
        AddEventListeners();
        
        PopulateHand();
    }

    protected override void OnDestroy()
    {
        RemoveEventListeners();

        base.OnDestroy();        
    }

    public Card PutRandomCardInHand()
    {
        var randomCardPrefab = GetRandomCard();
        var cardGameObject = Instantiate(randomCardPrefab.gameObject);
        var card = cardGameObject.GetComponent<Card>();
        if (card == null)
            return null;

        card.CurrentDropTarget = _handDropTarget;
        card.CurrentDropTarget.Register(card);
        card.PossibleTargets = _availableTargetsHints;
        card.LayoutGroup = _layoutGroup;
        card.Cost = Random.Range(1, 2);

        card.Randomize();

        card.transform.SetParent(_layoutGroup.transform, false);

        return card;
    }

    public void ShuffleCards()
    {
        ClearHand();
        PopulateHand();
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

            yield return new WaitUntil(() => !_player.Movement.IsMoving);

            executeIndex++;
        }
        
        Dispatch(new EventObject
        {
            Sender = this,
            Type = ExecutionDone
        });
    }

    private Card GetRandomCard()
    {
        var probabilityCountDown = Random.Range(0, 100);
        for (var i = 0; i < _availableCards.Length; i++)
        {
            probabilityCountDown -= _availableCards[i].Propability;
            if (probabilityCountDown < 0)
                return _availableCards[i].Card;
        }

        return _availableCards[_availableCards.Length - 1].Card;
    }

    private void ClearHand()
    {
        var cards = _handDropTarget.Draggables.Select(draggable => draggable.GetComponent<Card>()).Where(item => item != null).ToArray();
        for (var i = cards.Count() - 1; i >= 0; i--)
        {
            cards[i].Destroy();
        }
    }

    private void PopulateHand()
    {
        for (var i = 0; i < _handSize; i++)
        {
            PutRandomCardInHand();
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
