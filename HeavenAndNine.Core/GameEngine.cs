using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace HeavenAndNine.Core
{

    public enum GameState
    {
        New,
        Playing,
        End
    }

    public class GameEngine
    {
        public int Round { get; private set; } = 0;
        public int Lap { get; private set; } = 0;
        public int PlayingPlayerId { get; private set; } = 0;
        public List<Player> Players { get; set; } = new List<Player>();

        public Player? CurrentPlayer => Players.Where(p => p.Id == PlayingPlayerId).SingleOrDefault();

        public GameState GameState { get; set; } = GameState.New;

        public List<PlayerMove> PlayedPlayerMoves { get; set; } = new List<PlayerMove>();

        public static GameEngine CreateGameEngine()
        {
            Debug.WriteLine("Create and return new game engine");
            return new GameEngine();
        }

        public void StartNewGame()
        {
            Round = 1;
            Lap = 1;
            int numberOfPlayer = 4;
            List<Card> allCards = LoadCardsFromJSON();
            allCards.ShuffleMe();

            int numberOfPlayerAssigned = 0;
            int cardRange = 0;
            for (numberOfPlayerAssigned = 0; numberOfPlayerAssigned < numberOfPlayer; numberOfPlayerAssigned++)
            {
                var player = new Player();
                player.Id = numberOfPlayerAssigned + 1; //i.e Player 1, Player 2...Player 4.
                player.Name = "Player " + player.Id;
                player.SetCardsOnNewGame( allCards.GetRange(cardRange, 8).ToList() );
                Players.Add(player);
                cardRange += 8;
            }

            Random rnd = new Random();
            int bankerIndex = rnd.Next(0, 3);
            Players[bankerIndex].IsBanker = true;
            Debug.WriteLine($"Player {Players[bankerIndex].Id} is banker.");

            Debug.WriteLine("New Game started.");
            PlayingPlayerId = Players[bankerIndex].Id;
            GameState = GameState.Playing;
        }

        //TBD: maybe this should be more event-based driven. Also all logic has been cramped into here currently...
        public void EndTurn()
        {
            CurrentPlayer.hasPlayedInThisLap = true;

            if( Players.All(p => p.hasPlayedInThisLap))
            {
                Debug.WriteLine("This lap has ended!");

                var lastNonFoldedMove = PlayedPlayerMoves.Where(m => m.MoveType == MoveType.Play).LastOrDefault();
                var playerWinThisLap = Players.Find(p => p.Id == lastNonFoldedMove.PlayerId);
                int numberOfWinTower = PlayedPlayerMoves.First().PlacedCards.Count;
                playerWinThisLap.WinTower += numberOfWinTower;
                Debug.WriteLine($"Player {playerWinThisLap.Id} wins this lap. He/She wins {numberOfWinTower} tower(s) in this lap.");
               
                if(playerWinThisLap.CardsOnHand.Count == 0)
                {
                    Debug.WriteLine("This round has ended!");
                    Debug.WriteLine("Do Score Calculation and find the next banker!");
                }
                else
                {
                    //Reset state for next lap.
                    Players.ForEach(p => p.hasPlayedInThisLap = false);
                    Lap++;
                    PlayingPlayerId = playerWinThisLap.Id;
                    this.PlayedPlayerMoves = new List<PlayerMove>();
                }

            }
            else
            {
                if (PlayingPlayerId != 4)
                {
                    PlayingPlayerId++;
                }
                else
                {
                    PlayingPlayerId = 1;
                }
            }

        }

        public string PrintPlayerCardsById(int playerId)
        {
           var player =  this.Players.Where(p => p.Id == playerId).SingleOrDefault();
           string cardsText = String.Join( ", ", player.CardsOnHand.Select(card => card.ToString()) );
           
           return $"You have the following cards: {cardsText} ";
          
        }

        public static List<Card> LoadCardsFromJSON()
        {
            List<Card> cards = null;
            using (StreamReader r = new StreamReader("./Data/cards.json"))
            {
                string json = r.ReadToEnd();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                cards = JsonSerializer.Deserialize<List<Card>>(json, options);
            }
            Debug.WriteLine("Cards loaded");
            return cards;
        }
    }
}
