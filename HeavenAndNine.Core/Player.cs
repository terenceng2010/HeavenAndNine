namespace HeavenAndNine.Core
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Card> CardsOnHand { get; private set; }

        public int Score { get; set; } = 0;

        public int WinTower { get; set; } = 0;

        public bool IsBanker { get; set; } = false;

        public bool hasPlayedInThisLap { get; set; } = false;


        /// <summary>
        /// Play 1 or 2 or 3 or 4 cards, depends on the initial player's play, it might not be a valid move.
        /// P.S player might either play or fold their cards, however, it must match the number of cards the first player played.
        /// Ref: 天九#打牌 https://zh.wikipedia.org/zh-hant/%E5%A4%A9%E4%B9%9D#%E6%89%93%E7%89%8C
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="state"></param>
        public ValidateMoveResult Play(PlayerMove playerMove, GameEngine state)
        {
           var lastNonFoldedMove = state.PlayedPlayerMoves.Where(m => m.MoveType == MoveType.Play).LastOrDefault();
           var validateMoveResult = playerMove.ValidateMove(lastNonFoldedMove);
           if(validateMoveResult.IsValidMove) {
              state.PlayedPlayerMoves.Add(playerMove);
              foreach(Card placedCard in playerMove.PlacedCards) {
                this.CardsOnHand.Remove(placedCard);
              }
              return validateMoveResult;
           }
           else
           {
             return validateMoveResult;
           }
        }

        public ValidateMoveResult Fold(PlayerMove playerMove, GameEngine state)
        {
            var lastNonFoldedMove = state.PlayedPlayerMoves.Where(m => m.MoveType == MoveType.Play).LastOrDefault();
            var validateMoveResult = playerMove.ValidateMove(lastNonFoldedMove);
            if (validateMoveResult.IsValidMove)
            {
                state.PlayedPlayerMoves.Add(playerMove);
                foreach (Card placedCard in playerMove.PlacedCards)
                {
                    this.CardsOnHand.Remove(placedCard);
                }
                return validateMoveResult;
            }
            else
            {
                return validateMoveResult;
            }
        }


        public void SetCardsOnNewGame(List<Card> cards)
        {
            CardsOnHand = cards;
        }
    }
}
