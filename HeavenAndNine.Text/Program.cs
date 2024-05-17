using HeavenAndNine.Core;
using System.Linq;

namespace HeavenAndNine.TextMode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var game = GameEngine.CreateGameEngine();

            game.StartNewGame();
            Console.WriteLine("New Game Started");
            Console.WriteLine($"This is Round {game.Round}");
            while(game.GameState == GameState.Playing)
            {
                Console.WriteLine($"Player {game.PlayingPlayerId}");
                Console.WriteLine(game.PrintPlayerCardsById(game.PlayingPlayerId));
                Console.WriteLine("what is your action ? Key-in the card id(s) to select them.");

                string? keyInCardIdsString = Console.ReadLine();
                if(keyInCardIdsString == string.Empty)
                {
                    Console.WriteLine("Please input a valid key.");
                }
                else
                {
                    Console.WriteLine($"You input {keyInCardIdsString}");
                    List<string> keyInCardIds = keyInCardIdsString.Split(" ").ToList();
                    Boolean isAllKeyInCardIdsExist = true;
                    List<Card> selectedCards = new List<Card>();
                    foreach (string keyInCardId in keyInCardIds)
                    {
                        Card? selectedCard = game.CurrentPlayer.CardsOnHand.Find(c => c.Id == int.Parse(keyInCardId));
                        if(selectedCard == null)
                        {
                            isAllKeyInCardIdsExist = false;
                        }
                        else
                        {
                            selectedCards.Add(selectedCard);
                        }

                    }
                    if (!isAllKeyInCardIdsExist)
                    {
                        Console.WriteLine("Key-in card(s) not exist. Please Key-in valid card id(s) to select them.");
                        continue;
                    }

                    string selectedCardsText = String.Join("," , selectedCards.Select(s => s.ToString()).ToList());
                    Console.WriteLine($"You have selected {selectedCardsText.ToString()}.");
                    CombinationType combinationType = PlayerMove.DetermineCombinationType(selectedCards);
                    Console.Write($"This is combination of: {combinationType}");
                    if(combinationType == CombinationType.Invalid)
                    {
                        Console.WriteLine($"\nWhat do you want to do with them? Press F to fold them, N to start-over.");
                    }
                    else
                    {
                        int combinationTypeRank = PlayerMove.DetermineCombinationTypeRank(selectedCards, combinationType);
                        Console.WriteLine($" , of rank {combinationTypeRank}");
                        Console.WriteLine($"What do you want to do with them? Press P to play them, F to fold them, N to start-over.");
                    }
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.P:
                            Console.WriteLine($"\nPlayed {selectedCards.Count} cards.");
                            var playMove = new PlayerMove(game.CurrentPlayer.Id, MoveType.Play, selectedCards);
                            var playResult = game.CurrentPlayer.Play(playMove, game);
                            if (playResult.IsValidMove)
                            {
                                game.EndTurn();
                            }
                            else
                            {
                                Console.WriteLine(playResult.InvalidMoveReason);
                            }
                            break;
                        case ConsoleKey.F:
                            Console.WriteLine($"\nFolded {selectedCards.Count} cards.");
                            var foldMove = new PlayerMove(game.CurrentPlayer.Id, MoveType.Fold, selectedCards);
                            var foldResult = game.CurrentPlayer.Play(foldMove, game);
                            if (foldResult.IsValidMove)
                            {
                                game.EndTurn();
                            }
                            else
                            {
                                Console.WriteLine(foldResult.InvalidMoveReason);
                            }
                            break;
                        case ConsoleKey.N:
                            Console.WriteLine("\nStart-over");
                            break;
                        default:
                            Console.WriteLine("\nStart-over");
                            break;
                    }
                }                
            }

        }
    }
}
