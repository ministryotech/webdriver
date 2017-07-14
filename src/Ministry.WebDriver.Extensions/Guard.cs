using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// A static class of extension methods used to throw standard reference validation exceptions in a fluent manner.
    /// </summary>
	/// <remarks>
	/// This class should be used to throw <see cref="ArgumentException"/>s and other exceptions used to guard against bad input into a method.
	/// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal static class Guard
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the argument is null.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument to check for null.</typeparam>
        /// <param name="argument">The argument whose value should be checked for null.</param>
        /// <param name="argumentName">The name of the argument on the caller's method signature.</param>
        /// <returns>The argument which was passed in.</returns>
        /// <remarks>
        /// This method is a non-verbose and fluent way of throwing a standard <see cref="ArgumentNullException"/> where needed;
        /// it should be called upon the first needed use of the given argument which should not be null.
        /// The method can be chained prior to other calls on the argument due to its return value being that which was passed in.
        /// </remarks>
        [DebuggerHidden]
        public static TArgument ThrowIfNull<TArgument>(this TArgument argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);

            return argument;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the argument is null or an <see cref="ArgumentException"/> if it is empty.
        /// </summary>
        /// <param name="argument">The argument whose value should be checked for null.</param>
        /// <param name="argumentName">The name of the argument on the caller's method signature.</param>
        /// <returns>The argument which was passed in.</returns>
        /// <remarks>
        /// This method is a non-verbose and fluent way of throwing a standard <see cref="ArgumentNullException"/> or an <see cref="ArgumentException"/> where needed;
        /// it should be called upon the first needed use of the given argument which should not be null.
        /// The method can be chained prior to other calls on the argument due to its return value being that which was passed in.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the argumentName is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the argumentName is empty.</exception>
        [DebuggerHidden]
        public static string ThrowIfNullOrEmpty(this string argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);

            if (string.IsNullOrEmpty(argument))
                throw new ArgumentException($"The argument {argumentName} is empty.", argumentName);

            return argument;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> with the provided argumentExceptionMessage if the provided argumentExceptionPredicate evaluates to true.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument to evaluate.</typeparam>
        /// <param name="argument">The argument to evaluate.</param>
        /// <param name="argumentName">The name of the argument on the caller's method signature.</param>
        /// <param name="argumentExceptionPredicate">A <see cref="Predicate{TArgument}"/> which evaluates the argument and should return true if an <see cref="ArgumentException"/> should be thrown.</param>
        /// <param name="argumentExceptionMessageProducer">The function to produce the argumentExceptionMessage to pass into the <see cref="ArgumentException"/> if one needs to be thrown.  The input parameter for this function is the argumentName provided to this method.</param>
        /// <returns>The argument which was passed in.</returns>
        /// <remarks>
        /// This method is a non-verbose and fluent way of throwing an <see cref="ArgumentException"/> with injected exception-throwing logic.
        /// It should be called upon the first needed use of the given argument which should not cause the argumentExceptionPredicate to evaluate to true.
        /// The method can be chained prior to other calls on the argument due to its return value being that which was passed in.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the argumentExceptionPredicate, argumentName or argumentExceptionMessageProducer are null.</exception>
        [DebuggerHidden]
        public static TArgument ThrowIf<TArgument>(this TArgument argument, Predicate<TArgument> argumentExceptionPredicate, string argumentName, Func<string, string> argumentExceptionMessageProducer)
        {
            return ThrowIf(argument, argumentExceptionPredicate, argumentName, argumentExceptionMessageProducer.ThrowIfNull("argumentExceptionMessageProducer")(argumentName));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> with the provided argumentExceptionMessage if the provided <see cref="Boolean"/> is true.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument to evaluate.</typeparam>
        /// <param name="argument">The argument to evaluate.</param>
        /// <param name="argumentName">The name of the argument on the caller's method signature.</param>
        /// <param name="isArgumentException">True, if the exception is to be thrown; otherwise, false.</param>
        /// <param name="argumentExceptionMessageProducer">The function to produce the argumentExceptionMessage to pass into the <see cref="ArgumentException"/> if one needs to be thrown.  The input parameter for this function is the argumentName provided to this method.</param>
        /// <returns>The argument which was passed in.</returns>
        /// <remarks>
        /// This method is a non-verbose and fluent way of throwing an <see cref="ArgumentException"/> with injected exception-throwing logic.
        /// It should be called upon the first needed use of the given argument which should not cause the argumentExceptionPredicate to evaluate to true.
        /// The method can be chained prior to other calls on the argument due to its return value being that which was passed in.
        /// </remarks>
        [DebuggerHidden]
        public static TArgument ThrowIf<TArgument>(this TArgument argument, bool isArgumentException, string argumentName, Func<string, string> argumentExceptionMessageProducer)
        {
            return ThrowIf(argument, isArgumentException, argumentName, argumentExceptionMessageProducer.ThrowIfNull("argumentExceptionMessageProducer")(argumentName));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> with the provided argumentExceptionMessage if the provided argumentExceptionPredicate evaluates to true.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument to evaluate.</typeparam>
        /// <param name="argument">The argument to evaluate.</param>
        /// <param name="argumentName">The name of the argument on the caller's method signature.</param>
        /// <param name="argumentExceptionPredicate">A <see cref="Predicate{TArgument}"/> which evaluates the argument and should return true if an <see cref="ArgumentException"/> should be thrown.</param>
        /// <param name="argumentExceptionMessage">The argumentExceptionMessage to pass into the <see cref="ArgumentException"/> if one needs to be thrown.</param>
        /// <returns>The argument which was passed in.</returns>
        /// <remarks>
        /// This method is a non-verbose and fluent way of throwing an <see cref="ArgumentException"/> with injected exception-throwing logic.
        /// It should be called upon the first needed use of the given argument which should not cause the argumentExceptionPredicate to evaluate to true.
        /// The method can be chained prior to other calls on the argument due to its return value being that which was passed in.
        /// </remarks>
        [DebuggerHidden]
        public static TArgument ThrowIf<TArgument>(this TArgument argument, Predicate<TArgument> argumentExceptionPredicate, string argumentName, string argumentExceptionMessage = null)
        {
            return ThrowIf(argument, argumentExceptionPredicate(argument), argumentName, argumentExceptionMessage);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> with the provided argumentExceptionMessage if the provided <see cref="Boolean"/> is true.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument to evaluate.</typeparam>
        /// <param name="argument">The argument to evaluate.</param>
        /// <param name="argumentName">The name of the argument on the caller's method signature.</param>
        /// <param name="isArgumentException">True, if the exception is to be thrown; otherwise, false.</param>
        /// <param name="argumentExceptionMessage">The argumentExceptionMessage to pass into the <see cref="ArgumentException"/> if one needs to be thrown.</param>
        /// <returns>The argument which was passed in.</returns>
        /// <remarks>
        /// This method is a non-verbose and fluent way of throwing an <see cref="ArgumentException"/> with injected exception-throwing logic.
        /// It should be called upon the first needed use of the given argument which should not cause the argumentExceptionPredicate to evaluate to true.
        /// The method can be chained prior to other calls on the argument due to its return value being that which was passed in.
        /// </remarks>
        [DebuggerHidden]
        public static TArgument ThrowIf<TArgument>(this TArgument argument, bool isArgumentException, string argumentName, string argumentExceptionMessage = null)
        {
            var msgArgumentName = argumentName ?? "unknown argument";
            if (isArgumentException)
                throw new ArgumentException(argumentExceptionMessage ?? $"\"{msgArgumentName}\" of \"{argument}\" is invalid.", argumentName);

            return argument;
        }

        /// <summary>
        /// Throws the argument out of range exception if true.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="argumentOutOfRangeExceptionPredicate">The argument out of range exception predicate.</param>
        /// <param name="argumentExceptionMessage">The argument exception message.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns></returns>
        [DebuggerHidden]
        public static TArgument ThrowOutOfRangeIf<TArgument>(this TArgument argument, Predicate<TArgument> argumentOutOfRangeExceptionPredicate, string argumentName = null, string argumentExceptionMessage = null)
        {
            if (argumentOutOfRangeExceptionPredicate.ThrowIfNull(nameof(argumentOutOfRangeExceptionPredicate))(argument))
                throw new ArgumentOutOfRangeException(argumentName, argument, argumentExceptionMessage ?? $"{argumentName} of \"{argument}\" is outside of the expected range.");

            return argument;
        }

        /// <summary>
        /// Throws the argument out of range exception if out of range.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="minimumValue">The minimum value.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns></returns>
        [DebuggerHidden]
        public static TArgument ThrowOutOfRangeIf<TArgument>(this TArgument argument, string argumentName, object minimumValue, object maximumValue) where TArgument : IComparable
        {
            if (argument.ThrowIfNull(nameof(argument)).CompareTo(minimumValue) < 0 || argument.CompareTo(maximumValue) > 0)
                throw new ArgumentOutOfRangeException(argumentName);

            return argument;
        }

        /// <summary>
        /// Throws the argument out of range exception if out of range.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="minimumValue">The minimum value.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns></returns>
        [DebuggerHidden]
        public static TArgument ThrowOutOfRangeIf<TArgument>(this TArgument argument, string argumentName, TArgument minimumValue, TArgument maximumValue) where TArgument : IComparable<TArgument>
        {
            if (argument.ThrowIfNull(nameof(argument)).CompareTo(minimumValue) < 0 || argument.CompareTo(maximumValue) > 0)
                throw new ArgumentOutOfRangeException(argumentName);

            return argument;
        }

        /// <summary>
        /// Throws the argument out of range exception if out of range.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="boundaryValue">The boundary value.</param>
        /// <param name="boundary">The boundary.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns></returns>
        /// <exception cref="EnumValueNotSupportedException">boundary</exception>
        [DebuggerHidden]
        public static TArgument ThrowOutOfRangeIf<TArgument>(this TArgument argument, string argumentName, TArgument boundaryValue, Boundary boundary) where TArgument : IComparable<TArgument>
        {
            var comparison = argument.ThrowIfNull(nameof(argument)).CompareTo(boundaryValue);
            var argumentOutOfRangeException = new ArgumentOutOfRangeException(argumentName);

            switch (boundary)
            {
                case Boundary.Minimum:
                    if (comparison < 0)
                        throw argumentOutOfRangeException;

                    break;
                case Boundary.Maximum:
                    if (comparison > 0)
                        throw argumentOutOfRangeException;

                    break;
                default:
                    throw new EnumValueNotSupportedException(boundary, "boundary");
            }

            return argument;
        }

        #region | Internal Classes |

        /// <summary>
        /// An processingException class for handling the usage of unsupported enum values.
        /// </summary>
        /// <remarks>This would typically be used in the default case of a switch statement on an enum itemProcess (when no default behaviour exists).</remarks>
        public class EnumValueNotSupportedException : NotSupportedException
        {

            #region | Construction |

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumValueNotSupportedException"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="paramName">Name of the parameter.</param>
            /// <param name="innerException">The inner exception.</param>
            public EnumValueNotSupportedException(Enum value, string paramName = null, Exception innerException = null)
                : base(GenerateErrorMessage(value, paramName), innerException)
            {
			}

            #endregion


            /// <summary>
            /// Generates the error message.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="paramName">Name of the parameter.</param>
            /// <returns></returns>
            private static string GenerateErrorMessage(Enum value, string paramName)
            {
                var messageBuilder = new StringBuilder().AppendFormat("The {0} enumeration unsupportedType of \"{1}\" is not supported", value.GetType().Name, value);

                if (paramName != null)
                    messageBuilder.AppendFormat("; parameter name = \"{0}\"", paramName);

                return messageBuilder.Append('.').ToString();
            }
        }

        #endregion

        #region | Enumerators |

        /// <summary>
        /// A boundary indicator.
        /// </summary>
        public enum Boundary
        {
            Minimum,
            Maximum
        }

        #endregion
    }
}