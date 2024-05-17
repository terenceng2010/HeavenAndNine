using System.Diagnostics;

namespace HeavenAndNine.Core
{
    public enum MoveType { Play, Fold }
    public enum CombinationType { 文, 武, 文對, 文武對, 武對, 武尊, 文尊, 三文牌, 三武牌, 四文武牌, Invalid, Fold }

    public class PlayerMove
    {
        public int PlayerId { get; set; }

        public MoveType MoveType { get; private set; }
        public CombinationType CombinationType { get; private set; }
        public int CombinationTypeRank { get; private set; }
        public List<Card> PlacedCards { get; set; }

        public PlayerMove(int playerId, MoveType moveType, List<Card> cards) {
            PlayerId = playerId;
            MoveType = moveType;
            if (moveType == MoveType.Play)
            {
                CombinationType = DetermineCombinationType(cards);
                if(CombinationType != CombinationType.Invalid)
                {
                    CombinationTypeRank = DetermineCombinationTypeRank(cards, CombinationType);
                }
            }
            else
            {
                CombinationType = CombinationType.Fold;
            }
            PlacedCards = cards;
        }

        public static CombinationType DetermineCombinationType(List<Card> Cards, Boolean has文尊 = true)
        {
            CombinationType playType = new CombinationType();
            string cardCombinationRaw = "";
            foreach (Card card in Cards)
            {
                cardCombinationRaw += card.CodeName;
            }

            string cardCombination = NormalizeCardCombination(cardCombinationRaw);
            switch (cardCombination)
            {
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                case "G":
                case "H":
                case "I":
                case "J":
                case "K":
                    playType = CombinationType.文;
                    break;
                case "9":
                case "8":
                case "7":
                case "6":
                case "5":
                case "3":
                    playType = CombinationType.武;
                    break;
                case "AA":
                case "BB":
                case "CC":
                case "DD":
                case "EE":
                case "FF":
                case "GG":
                case "HH":
                case "II":
                case "JJ":
                    playType = CombinationType.文對;
                    break;
                case "99":
                case "88":
                case "77":
                case "55":
                    playType = CombinationType.武對;
                    break;
                case "A9":
                case "B8":
                case "C7":
                case "D5":
                    playType = CombinationType.文武對;
                    break;
                case "63":
                    playType = CombinationType.武尊;
                    break;
                case "KK":
                    if (has文尊)
                    {
                        playType = CombinationType.文尊;
                    }
                    else
                    {
                        playType = CombinationType.文對;
                    }
                    break;
                case "AA9":
                case "BB8":
                case "CC7":
                case "DD5":
                    playType = CombinationType.三文牌;
                    break;
                case "A99":
                case "B88":
                case "C77":
                case "D55":
                    playType = CombinationType.三武牌;
                    break;
                case "AA99":
                case "BB88":
                case "CC77":
                case "DD55":
                    playType = CombinationType.四文武牌;
                    break;
                default:
                    playType = CombinationType.Invalid;
                    break;
            }
            Debug.WriteLine($"cardCombination: {cardCombination} PlayType: {nameof(playType)}");
            return playType;
        }

        /// <summary>
        /// Small number == higher rank. e.g Rank 1 > Rank 2 > Rank 3...
        /// </summary>
        /// <param name="Cards"></param>
        /// <param name="combinationType"></param>
        /// <param name="has文尊"></param>
        /// <returns></returns>
        public static int DetermineCombinationTypeRank(List<Card> Cards, CombinationType combinationType, bool has文尊 = true)
        {

            string cardCombinationRaw = "";
            foreach (Card card in Cards)
            {
                cardCombinationRaw += card.CodeName;
            }
            string cardCombination = NormalizeCardCombination(cardCombinationRaw);
            Dictionary<string, int> rankDict = new Dictionary<string, int>();

            //CombinationType.文
            rankDict.Add("A", 1);
            rankDict.Add("B", 2);
            rankDict.Add("C", 3);
            rankDict.Add("D", 4);
            rankDict.Add("E", 5);
            rankDict.Add("F", 6);
            rankDict.Add("G", 7);
            rankDict.Add("H", 8);
            rankDict.Add("I", 9);
            rankDict.Add("J", 10);
            rankDict.Add("K", 11);

            // CombinationType.武
            rankDict.Add("9", 1);
            rankDict.Add("8", 2);
            rankDict.Add("7", 3);
            rankDict.Add("6", 4);
            rankDict.Add("5", 5);
            rankDict.Add("3", 6);

            //CombinationType.文對
            rankDict.Add("AA", 1);
            rankDict.Add("BB", 2);
            rankDict.Add("CC", 3);
            rankDict.Add("DD", 4);
            rankDict.Add("EE", 5);
            rankDict.Add("FF", 6);
            rankDict.Add("GG", 7);
            rankDict.Add("HH", 8);
            rankDict.Add("II", 9);
            rankDict.Add("JJ", 10);

            //CombinationType.武對;
            rankDict.Add("99", 1);
            rankDict.Add("88", 2);
            rankDict.Add("77", 3);
            rankDict.Add("55", 4);

            //CombinationType.文武對;
            rankDict.Add("A9", 1);
            rankDict.Add("B8", 2);
            rankDict.Add("C7", 3);
            rankDict.Add("D5", 4);

            //CombinationType.武尊;
            rankDict.Add("63", 1);


            //CombinationType.文尊
            if (has文尊)
            {
                rankDict.Add("KK", 1);
            }
            else //CombinationType.文對
            {
                rankDict.Add("KK", 11);
            }

            //CombinationType.三文牌;
            rankDict.Add("AA9", 1);
            rankDict.Add("BB8", 2);
            rankDict.Add("CC7", 3);
            rankDict.Add("DD5", 4);

            //CombinationType.三武牌;
            rankDict.Add("A99", 1);
            rankDict.Add("B88", 2);
            rankDict.Add("C77", 3);
            rankDict.Add("D55", 4);

            //CombinationType.四文武牌;
            rankDict.Add("AA99", 1);
            rankDict.Add("BB88", 2);
            rankDict.Add("CC77", 3);
            rankDict.Add("DD55", 4);

            int rank = rankDict[cardCombination];
            Debug.WriteLine($"cardCombination: {cardCombination} rank: {rank}");
            return rank;
        }
        public static string NormalizeCardCombination(string cardCombinationRaw)
        {
            //This part tries to normalize the selected cards, for example AA9, A9A and 9AA should be normalized to AA9.
            //i.e to put Letter Card first, and put Digit Card second, and Sort them by desc order.
            List<char> cardLetterCombination = new List<char>();
            List<int> cardDigitCombination = new List<int>();
            foreach (char c in cardCombinationRaw)
            {
                if (Char.IsLetter(c))
                {
                    cardLetterCombination.Add(c);
                }
                else if (Char.IsDigit(c))
                {
                    cardDigitCombination.Add((int)(Char.GetNumericValue(c)));
                }
            }
            cardLetterCombination = cardLetterCombination.OrderByDescending(i => i).ToList();
            cardDigitCombination = cardDigitCombination.OrderByDescending(i => i).ToList();

            string cardCombination = String.Concat(cardLetterCombination) + String.Concat(cardDigitCombination);
            return cardCombination;
        }

        public ValidateMoveResult ValidateMove(PlayerMove lastMove = null)
        {

            if(lastMove == null) // If this is the first player in a round (no lastMove), as long as the combination type is not invalid and the move type is not folded, it is a valid move.
            {
                 if( CombinationType == CombinationType.Invalid)
                 {
                    return new ValidateMoveResult(false, "The CombinationType is invalid.");
                 }
                 else if(MoveType == MoveType.Fold)
                 {
                    return new ValidateMoveResult(false, "You cannot fold as the first player.");
                 }
                 else
                 {
                    return new ValidateMoveResult(true, "");
                 }
            }
            else // subsequent player in a round can either fold or play cards.

                if (MoveType == MoveType.Fold)
                {
                    //For fold, it needs to match the number of placed cards.
                    if (lastMove.PlacedCards.Count == this.PlacedCards.Count)
                    {
                        return new ValidateMoveResult(true, "");
                    }
                    else
                    {
                        return new ValidateMoveResult(false, $"Last move has placed {lastMove.PlacedCards.Count} cards. You try to fold with {this.PlacedCards.Count} cards, which is invalid.");
                    }
                }
                else
                {
                    //For play, it needs to be a valid combination type and match the combination type of last non-folded move.
                    if (CombinationType == CombinationType.Invalid)
                    {
                        return new ValidateMoveResult(false, "The CombinationType is invalid.");
                    }else if(lastMove.CombinationType != this.CombinationType)
                    {
                        return new ValidateMoveResult(false, $"Last non-folded move has combination type: {lastMove.CombinationType}. You try to play with combination type {this.CombinationType}, which is invalid.");
                    }else if(lastMove.CombinationTypeRank < this.CombinationTypeRank)
                    {
                    return new ValidateMoveResult(false, $"Last non-folded move has combination type rank: {lastMove.CombinationTypeRank}. You try to play with combination type rank {this.CombinationTypeRank}, which is invalid as it is smaller.");
                }
                    else
                    {
                        return new ValidateMoveResult(true, "");
                    }
                }
            }

        
    }

    public class ValidateMoveResult
    { 
        public Boolean IsValidMove { get; set; }
        public String  InvalidMoveReason { get; set; }
        public ValidateMoveResult(Boolean isValidMove, String invalidMoveReason)
        {
            IsValidMove = isValidMove;
            InvalidMoveReason = invalidMoveReason;
        }
    }
    
}
