using PlayingCards;

namespace StripJackNaked
{
    internal static class Constants
    {
        public const int PlayerCount = 2;
    }

    internal class Kitty
    {
        public static List<Card> Pile = new();
    }

    internal class Player
    {
        public List<Card> Hand = new();
    }

    internal class Program
    {
        private static void Main()
        {
            Deck deck = new();

            // Init players
            List<Player> Players = new();
            for (int i = 0; i < Constants.PlayerCount; i++)
            {
                Players.Add(new Player());
            }

            // Init deck
            //Deck.Enumerate();
            // Deck.Shuffle();

            // Deal cards
            while (deck.Cards.Count > 0)
            {
                Card card = deck.Cards.Last();
                int player = deck.Cards.Count() % Constants.PlayerCount; // Alternate dealing cards between players 0 and 1
                _ = Actions.MoveCard(deck.Cards, Players[player].Hand, card);
            }

            // Init first round
            int CardsToDraw = 1;
            Card? ActivePictureCard = null;

            int ActivePlayer = 0; // Player 0 goes first

            // Let's play
            while (PlayCard(Players[ActivePlayer].Hand))
            {
                Card LastCard = Kitty.Pile.LastOrDefault();
                if (LastCard.IsPicture)
                {
                    // Active player just played a picture card, next player's turn
                    ActivePictureCard = LastCard;
                    CardsToDraw = (int)Enum.Parse(typeof(PictureCard), ActivePictureCard.Value); // Cards to draw correlates to PictureCard enum value
                    ActivePlayer = NextPlayer(ActivePlayer);
                }
                else if (ActivePictureCard is not null)
                {
                    // Active player didn't play a picture card when needed to
                    CardsToDraw--;
                }

                if (CardsToDraw < 1)
                {
                    // Active player lost round
                    TakePile(Players[NextPlayer(ActivePlayer)].Hand);

                    // Init next round
                    CardsToDraw = 1;
                    ActivePictureCard = null;
                }

                if (ActivePictureCard is null)
                {
                    ActivePlayer = NextPlayer(ActivePlayer);
                }

                // Show some output
                Console.Write("Player 0: ");
                ShowList(Players[0].Hand);

                Console.Write("Player 1: ");
                ShowList(Players[1].Hand);

                Console.Write("Kitty: ");
                ShowList(Kitty.Pile);

                Console.WriteLine("---------------------");
            }
            Console.WriteLine("Player " + NextPlayer(ActivePlayer) + " Won!");
        }

        private static int NextPlayer(int CurrentPlayer)
        {
            return ++CurrentPlayer % Constants.PlayerCount;
        }

        private static bool PlayCard(List<Card> From)
        {
            return Actions.MoveCard(From, Kitty.Pile);
        }

        private static void TakePile(List<Card> Winner)
        {
            while (Kitty.Pile.Count() > 0)
            {
                _ = Actions.MoveCard(Kitty.Pile, Winner);
            }
        }

        private static void ShowList(List<Card> Cards)
        {
            foreach (Card card in Cards)
            {
                Console.Write(card.NamedValue + ((card == Cards.LastOrDefault()) ? "" : ", "));
            }
            Console.WriteLine();
        }
    }
}