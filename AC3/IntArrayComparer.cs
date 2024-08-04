using System.Diagnostics.CodeAnalysis;
namespace AC3
{
    internal class IntArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? x, int[]? y)
        {
            if (x is null || y is null) 
                return false;
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] int[] obj)
        {
            return string.Join(string.Empty, obj).GetHashCode();
        }
    }
}
