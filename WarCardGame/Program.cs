using System;
using System.Collections.Generic;
using System.Linq;

namespace WarCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Game of War Started");

            var deck = new Deck();
            var rounds = 0; 

            var player1 = new Player() { Hand = new List<Card>() };
            var player2 = new Player() { Hand = new List<Card>() };

            deck.Deal(player1, player2);

            while (player1.Hand.Count > 0 && player2.Hand.Count > 0)
            {
                var cardPot = new List<Card>(); //the pot holds the cards that go to the winner

                DetermineWinner(player1, player2, cardPot);

                Console.WriteLine($"   Game State: P1: {player1.Hand.Count} P2: {player2.Hand.Count} after {++rounds} rounds");

                if (rounds % 26 == 0)
                {
                    //Shuffle hands every 26 turns to prevent a rare situation where card order causes no winner
                    player1.Shuffle();
                    player2.Shuffle();
                }
            }

            if (player1.Hand.Count == player2.Hand.Count && player1.Hand.Count == 0)
            {
                Console.WriteLine("IT'S A DRAW!!"); //this only happens in rare cases when a war goes long enough that both players run out of cards
            }
            else if (player2.Hand.Count == 0)
            {
                Console.WriteLine($"Player 1 WINS!!"); 
            }
            else if (player1.Hand.Count == 0)
            {
                Console.WriteLine($"Player 2 WINS!!");
            }

            Console.ReadLine();
        }

        //Recursive function for playing a hand
        private static void DetermineWinner(Player player1, Player player2, List<Card> cardPot)
        {
            var player1Card = player1.Hand.FirstOrDefault();
            player1.Hand.Remove(player1Card);
            cardPot.Add(player1Card);

            var player2Card = player2.Hand.FirstOrDefault();
            player2.Hand.Remove(player2Card);
            cardPot.Add(player2Card);
            
            if (player1Card.Value > player2Card.Value)
            {
                Console.WriteLine($"Player 1 wins ({player1Card.Value}) to ({player2Card.Value})");
                player1.Hand.AddRange(cardPot);
            }
            else if (player2Card.Value > player1Card.Value)
            {
                Console.WriteLine($"Player 2 wins ({player2Card.Value}) to ({player1Card.Value})");
                player2.Hand.AddRange(cardPot);
            }
            else
            {
                Console.WriteLine($"WAR between ({player1Card.Value}) and ({player2Card.Value})");

                var cardsForPotCount = player1.Hand.Count > 3 ? 3 : player1.Hand.Count - 1; //Handle hand size less than 4
                var player1WarCardsForPot = player1.Hand.Take(cardsForPotCount).ToList();
                if (player1WarCardsForPot.Count > 0)
                {
                    player1.Hand.RemoveAll(h => player1WarCardsForPot.Contains(h));
                    cardPot.AddRange(player1WarCardsForPot);
                }

                cardsForPotCount = player2.Hand.Count > 3 ? 3 : player2.Hand.Count - 1; //Handle hand size less than 4
                var player2WarCardsForPot = player2.Hand.Take(cardsForPotCount).ToList();
                if (player2WarCardsForPot.Count > 0)
                {
                    player2.Hand.RemoveAll(h => player2WarCardsForPot.Contains(h));
                    cardPot.AddRange(player2WarCardsForPot);
                }

                if (player1.Hand.Count == 0 && player2.Hand.Count == 0)
                {
                    //Special Condition:
                    //It's a draw if both players run out of cards during war... this is pretty rare
                    return;
                }

                if (player1.Hand.Count == 0)
                {
                    //Special Condition:
                    //Player 2 wins if player 1 runs out of cards during war
                    player2.Hand.AddRange(cardPot);
                    return;
                }

                if (player2.Hand.Count == 0)
                {
                    //Special Condition:
                    //Player 1 wins if player 2 runs out of cards during war
                    player1.Hand.AddRange(cardPot);
                    return;
                }

                //War can recursively call this funtion, building up the card pot
                DetermineWinner(player1, player2, cardPot);
            }
        }
    }

    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            this.Cards = new List<Card>();

            for (int i = 0; i < 13; i++)
            {
                foreach (var suit in new List<string>() { "hearts", "spades", "clubs", "diamonds" })
                {
                    this.Cards.Add(new Card() { Value = i, Suit = suit });
                    //Console.WriteLine($"Created card {i} - {suit}");
                }
            }
        }

        public void Shuffle()
        {
            this.Cards = Utils.ShuffleCards(this.Cards);

            Console.WriteLine("Finished shuffling.");
        }

        public void Deal(Player player1, Player player2)
        {
            this.Shuffle();
            player1.Hand = this.Cards.GetRange(0, 26);
            player2.Hand = this.Cards.GetRange(26, 26);
        }
    }

    public class Card
    {
        public int Value { get; set; }
        public string Suit { get; set; }
        public string Display {
            get {
                return this.Value.ToString();
            }
        }
    }

    public class Player
    {
        public List<Card> Hand { get; set; }

        public void Shuffle()
        {
            this.Hand = Utils.ShuffleCards(this.Hand);
        }
    }

    public static class Utils
    {
        public static List<Card> ShuffleCards(List<Card> unshuffled)
        {
            var numberOfCards = unshuffled.Count;
            var shuffled = new List<Card>();
            while (shuffled.Count < numberOfCards)
            {
                //Randomly pull cards from the unshuffled deck and add them to shuffled
                var randomIndex = new Random().Next(0, unshuffled.Count - 1);
                var card = unshuffled[randomIndex];
                shuffled.Add(card);
                unshuffled.RemoveAt(randomIndex);
                //Console.WriteLine($"Shuffled card: {card.Value} - {card.Suit}");
            }

            return shuffled;
        }
    }
}
