using System.Diagnostics.CodeAnalysis;

namespace Project.Extensions.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value) => string.IsNullOrEmpty(value);
        
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable) => enumerable == null || !enumerable.Any();
        
        public static bool HasValue([NotNullWhen(true)] this string? value) => !string.IsNullOrEmpty(value);
    }
}

