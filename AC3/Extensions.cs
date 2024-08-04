namespace AC3
{
    internal static class Extensions
    {
        static Random rng = new Random();
        public static bool IsEmpty<T>(this IEnumerable<T> s)
        {
            return s.Count() == 0;
        }
        public static int RandomIndex<T>(this HashSet<T> s)
        {
            return rng.Next(s.Count);
        }
        public static int RandomIndex<T>(this List<T> s)
        {
            return rng.Next(s.Count);
        }

        public static void AddAll<T>(this HashSet<T> set, IEnumerable<T> toAdd)
        {
            foreach (var item in toAdd)
                set.Add(item);
        }
        public static void AddAll<T>(this List<T> set, IEnumerable<T> toAdd)
        {
            foreach(var item in toAdd)
                set.Add(item);
        }
    }
}
