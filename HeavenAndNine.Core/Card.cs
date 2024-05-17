namespace HeavenAndNine.Core
{
    //public enum TileType { civil, military }

    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CName { get; set; }
        public string CodeName { get; set; }
        public string TileType { get; set; }
        public int Rank { get; set; }
        public string Icon { get; set; }

        public override string ToString()
        {
            return $"{CName} (id:{ Id }, Code: {CodeName}, type: {TileType})";
        }
    }
}