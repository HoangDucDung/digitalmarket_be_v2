namespace Project.Extensions.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();
        public static bool HasValue(this string? value) => !string.IsNullOrEmpty(value);

    }
}
