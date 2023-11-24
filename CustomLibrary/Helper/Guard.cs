using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace CustomLibrary.Helper
{
    public interface IGuardClauses
    {

    }

    public class Guard : IGuardClauses
    {
        private Guard() { }

        public static IGuardClauses Against { get; } = new Guard();
    }

    public static class GuardExtensions
    {
        public static T Null<T>(this IGuardClauses guardClause, T? input, string paramName, string? message = null)
        {
            if (input is null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new ArgumentNullException(paramName);
                }
                throw new ArgumentNullException(message, (Exception?)null);
            }
            return input;
        }
        public static string NullOrEmpty(this IGuardClauses guardClause, string? input, string paramName, string? message = null)
        {
            Guard.Against.Null(input, paramName);
            if (input == string.Empty) throw new ArgumentException(message ?? $"{paramName} Tidak Boleh Kosong", paramName);
#pragma warning disable CS8603 // Possible null reference return.
            return input;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public static Guid NullOrEmpy(this IGuardClauses guardClauses, Guid? input, string paramName, string? message = null)
        {
            Guard.Against.Null(input, paramName);
            if (input == Guid.Empty)
            {
                throw new ArgumentException(message ?? $"{paramName} Tidak Boleh Kosong", paramName);
            }
#pragma warning disable CS8629 // Nullable value type may be null.
            return input.Value;
#pragma warning restore CS8629 // Nullable value type may be null.
        }
        public static IEnumerable<T> NullOrEmpty<T>(this IGuardClauses guardClauses, IEnumerable<T>? input, string paramName, string? message = null)
        {
            Guard.Against.Null(input, paramName);
#pragma warning disable CS8604 // Possible null reference argument.
            if (!input.Any())
            {
                throw new ArgumentException(message ?? $"{paramName} Tidak Boleh Kosong", paramName);
            }
#pragma warning restore CS8604 // Possible null reference argument.
            return input;
        }
        public static string NullOrWhiteSpace(this IGuardClauses guardClauses, string? input, string paramName, string? message = null)
        {
            Guard.Against.NullOrEmpty(input, paramName);
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException($"{paramName} Tidak Boleh Kosong", paramName);
            return input;
        }
        private static T NegativeOrZero<T>(this IGuardClauses guardClauses, T input, string paramName, string? message = null) where T : IComparable, IComparable<T>
        {
            if (input.CompareTo(default(T)) <= 0)
            {
                throw new ArgumentException(message ?? $"{paramName} Tidak Boleh Kurang Dari Sama Dengan Nol", paramName);
            }
            return input;
        }
        public static TimeSpan NegativeOrZero(this IGuardClauses guardClauses, TimeSpan input, string paramName, string? message = null)
        {
            return NegativeOrZero<TimeSpan>(guardClauses, input, paramName, message);
        }
        public static double NegativeOrZero(this IGuardClauses guardClauses, double input, string paramName, string? message = null)
        {
            return NegativeOrZero<Double>(guardClauses, input, paramName, message);
        }
        public static float NegativeOrZero(this IGuardClauses guardClauses, float input, string paramName, string? message = null)
        {
            return NegativeOrZero<float>(guardClauses, input, paramName, message);
        }
        public static long NegativeOrZero(this IGuardClauses guardClauses, long input, string paramName, string? message = null)
        {
            return NegativeOrZero<long>(guardClauses, input, paramName, message);
        }
        public static decimal NegativeOrZero(this IGuardClauses guardClauses, decimal input, string paramName, string? message = null)
        {
            return NegativeOrZero<decimal>(guardClauses, input, paramName, message);
        }
        public static int NegativeOrZero(this IGuardClauses guardClauses, int input, string paramName, string? message = null)
        {
            return NegativeOrZero<int>(guardClauses, input, paramName, message);
        }
        private static T Negative<T>(this IGuardClauses guardClauses, T input, string paramName, string? message = null) where T : struct, IComparable
        {
            if (input.CompareTo(default(T)) < 0)
            {
                throw new ArgumentException(message ?? $"{paramName} Tidak Boleh Negatif", paramName);
            }
            return input;
        }
        public static int Negative(this IGuardClauses guardClauses, int input, string paramName, string? message = null)
        {
            return Negative<int>(guardClauses, input, paramName, message);
        }
        public static TimeSpan Negative(this IGuardClauses guardClauses, TimeSpan input, string paramName, string? message = null)
        {
            return Negative<TimeSpan>(guardClauses, input, paramName, message);
        }
        public static double Negative(this IGuardClauses guardClauses, double input, string paramName, string? message = null)
        {
            return Negative<double>(guardClauses, input, paramName, message);
        }
        public static float Negative(this IGuardClauses guardClauses, float input, string paramName, string? message = null)
        {
            return Negative<float>(guardClauses, input, paramName, message);
        }
        public static decimal Negative(this IGuardClauses guardClauses, decimal input, string paramName, string? message = null)
        {
            return Negative<decimal>(guardClauses, input, paramName, message);
        }
        public static long Negative(this IGuardClauses guardClauses, long input, string paramName, string? message = null)
        {
            return Negative<long>(guardClauses, input, paramName, message);
        }
    }
}
