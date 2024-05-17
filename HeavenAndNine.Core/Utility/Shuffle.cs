namespace HeavenAndNine.Core
{
    public static class Shuffle
    {
        /// <summary>
        /// Simple way to randomly shuffle list
        /// https://stackoverflow.com/a/49570293
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleMe<T>(this IList<T> list)
        {
            Random random = new Random();
            int n = list.Count;

            for (int i = list.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }
    }
}
