namespace PlayingCards
{
    public enum Suit { Spades, Clubs, Diamonds, Hearts }

    public enum PictureCard { Jack = 1, Queen, King, Ace }

    public class Card
    {
        public string Value;
        public Suit Suit;
        public string NamedValue => Value + " of " + Suit;
        public bool IsPicture => Enum.IsDefined(typeof(PictureCard), Value);
        public bool IsNumber => !Enum.IsDefined(typeof(PictureCard), Value);
        public bool IsBlack  => (Enum.GetName(Suit) == "Spades" || Enum.GetName(Suit) == "Clubs");
        public bool IsRed => (Enum.GetName(Suit) == "Diamonds" || Enum.GetName(Suit) == "Hearts");
    }

    public class Deck
    {
        public static List<Card> Cards = new();
        public static void Enumerate()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                // Add number cards
                for (int i = 2; i <= 10; i++)
                {
                    Cards.Add(new Card()
                    {
                        Value = i.ToString(),
                        Suit = suit
                    });
                }

                // Add picture cards
                foreach (PictureCard pictureCard in Enum.GetValues(typeof(PictureCard)))
                {
                    Cards.Add(new Card()
                    {
                        Value = Enum.GetName(pictureCard),
                        Suit = suit
                    });
                }
            }
        }

        private static Random rng = new();
        public static void Shuffle()
        {
            Cards = Cards.OrderBy(a => rng.Next()).ToList();
        }
    }

    public class Actions
    {
        public static bool MoveCard(List<Card> From, List<Card> To, Card card = null)
        {
            if (From.Count() == 0)
            {
                return false; // No cards available to move
            }

            if (card is null)
            {
                card = From.FirstOrDefault(); // FIFO
            }

            if (From.Contains(card) && !To.Contains(card))
            {
                To.Add(card);
                From.Remove(card);
                return true;
            }
            return false; // If we're here, something went bang
        }
    }
}