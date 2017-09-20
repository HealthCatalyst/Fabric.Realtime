namespace Fabric.Realtime.Core.Utils
{
    using System;

    /// <summary>
    /// Utility class with parameter checking routines.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Throws ArgumentNullException if the given argument is null.
        /// </summary>
        /// <param name="argumentValue">
        /// The argument value.
        /// </param>
        /// <param name="argumentName">
        /// The argument name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The argumentValue is null.
        /// </exception>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
