namespace Day7CamelCards;

#pragma warning disable CA1036 // Override methods on comparable types
public class CardHand : IComparable<CardHand>
#pragma warning restore CA1036 // Override methods on comparable types
{
    private const char JokerCard = 'J';

    private static readonly string AvailableCardsOrderedFromWeakestToStrongestForNoJokerRuleHand = "23456789T" + JokerCard + "QKA";
    private static readonly string AvailableCardsOrderedFromWeakestToStrongestForJokerRuleHand = JokerCard + "23456789TQKA";

    private CardHandType? type;

    public CardHand(string cards, int bid, bool respectJokerRule)
    {
        this.Cards = cards;
        this.Bid = bid;
        this.RespectJokerRule = respectJokerRule;
    }

    public string Cards { get; init; } = null!;

    public int Bid { get; init; }

    public bool RespectJokerRule { get; init; }

    public int CompareTo(CardHand? other)
    {
        if (other is null)
        {
            return 1;
        }

        var type = this.DetermineType();
        var otherType = other.DetermineType();

        var typesComparison = type.CompareTo(otherType);
        if (typesComparison != 0)
        {
            return typesComparison;
        }

        return this.CompareByCardStrengths(other);
    }

    private static CardHandType DetermineTypeByCardOccurrences(Dictionary<char, int> cardOccurrencesMap, bool respectJokerCard)
    {
        if (cardOccurrencesMap.Count == 1)
        {
            return CardHandType.FiveOfAKind;
        }

        if (cardOccurrencesMap.Count == 2)
        {
            if (respectJokerCard && cardOccurrencesMap.ContainsKey(JokerCard))
            {
                return CardHandType.FiveOfAKind;
            }

            return cardOccurrencesMap.Values.Any(count => count == 4)
                ? CardHandType.FourOfAKind
                : CardHandType.FullHouse;
        }

        if (cardOccurrencesMap.Count == 3)
        {
            var handTypeWithJokerSubstitutions = respectJokerCard
                ? DetermineHandTypeWithJokerSubstitutions(cardOccurrencesMap)
                : null;

            var handTypeWithoutJokerSubstitutions = cardOccurrencesMap.Values.Any(count => count == 3)
                ? CardHandType.ThreeOfAKind
                : CardHandType.TwoPair;

            if (handTypeWithJokerSubstitutions > handTypeWithoutJokerSubstitutions)
            {
                return handTypeWithJokerSubstitutions.Value;
            }

            return handTypeWithoutJokerSubstitutions;
        }

        if (cardOccurrencesMap.Count == 4)
        {
            var handTypeWithJokerSubstitutions = respectJokerCard
                ? DetermineHandTypeWithJokerSubstitutions(cardOccurrencesMap)
                : null;

            if (handTypeWithJokerSubstitutions > CardHandType.OnePair)
            {
                return handTypeWithJokerSubstitutions.Value;
            }

            return CardHandType.OnePair;
        }

        return respectJokerCard && cardOccurrencesMap.ContainsKey(JokerCard)
            ? CardHandType.OnePair
            : CardHandType.HighCard;
    }

    private static CardHandType? DetermineHandTypeWithJokerSubstitutions(Dictionary<char, int> cardOccurrencesMap)
    {
        CardHandType? handType = null;

        cardOccurrencesMap.TryGetValue(JokerCard, out var jokerCardOccurrences);
        if (jokerCardOccurrences > 0)
        {
            foreach (var card in cardOccurrencesMap.Keys)
            {
                if (card == JokerCard)
                {
                    continue;
                }

                var newCardOccurrencesMap = cardOccurrencesMap.ToDictionary();
                newCardOccurrencesMap.Remove(JokerCard);
                newCardOccurrencesMap[card] += jokerCardOccurrences;

                var newHandType = DetermineTypeByCardOccurrences(newCardOccurrencesMap, respectJokerCard: false);
                if (handType is null || newHandType > handType)
                {
                    handType = newHandType;
                }
            }
        }

        return handType;
    }

    private CardHandType DetermineType()
    {
        if (this.type.HasValue)
        {
            return this.type.Value;
        }

        var cardOccurrencesMap = new Dictionary<char, int>(capacity: 5);
        foreach (var card in this.Cards)
        {
            cardOccurrencesMap.TryGetValue(card, out var cardOccurrences);
            cardOccurrencesMap[card] = cardOccurrences + 1;
        }

        this.type = DetermineTypeByCardOccurrences(cardOccurrencesMap, this.RespectJokerRule);
        return this.type.Value;
    }

    private int CompareByCardStrengths(CardHand other)
    {
        var availableCardsOrderedFromWeakestToStrongest = this.RespectJokerRule
            ? AvailableCardsOrderedFromWeakestToStrongestForJokerRuleHand
            : AvailableCardsOrderedFromWeakestToStrongestForNoJokerRuleHand;

        for (var i = 0; i < this.Cards.Length; i++)
        {
            var card = this.Cards[i];
            var otherCard = other.Cards[i];

            var cardStrength = availableCardsOrderedFromWeakestToStrongest.IndexOf(card);
            var otherCardStrength = availableCardsOrderedFromWeakestToStrongest.IndexOf(otherCard);

            var cardStrengthsComparison = cardStrength.CompareTo(otherCardStrength);
            if (cardStrengthsComparison != 0)
            {
                return cardStrengthsComparison;
            }
        }

        return 0;
    }
}
