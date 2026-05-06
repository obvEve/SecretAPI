namespace SecretAPI.Extensions;

using System;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

/// <summary>
/// Extensions related to <see cref="CodeMatcher"/>.
/// </summary>
public static class CodeMatcherExtensions
{
    extension(CodeMatcher matcher)
    {
        /// <summary>
        /// Sets <see cref="CodeMatcher.Pos"/> to the end and then backtracks X amount.
        /// </summary>
        /// <param name="backtrackCount">The amount to reverse. Must be positive number.</param>
        /// <returns>The current <see cref="CodeMatcher"/>.</returns>
        public CodeMatcher EndAndBacktrack(int backtrackCount)
        {
            matcher.End();
            matcher.Advance(-backtrackCount);
            return matcher;
        }

        /// <summary>
        /// Declares a local to be used of a certain <see cref="Type"/>.
        /// </summary>
        /// <param name="localType">The <see cref="Type"/> of the local to declare.</param>
        /// <param name="localBuilder">The <see cref="LocalBuilder"/> declared.</param>
        /// <returns>The current <see cref="CodeMatcher"/>.</returns>
        public CodeMatcher DeclareLocal(Type localType, out LocalBuilder localBuilder)
        {
            localBuilder = matcher.generator.DeclareLocal(localType);
            return matcher;
        }

        /// <summary>
        /// Declares a local to be used of a certain <see cref="Type"/>.
        /// </summary>
        /// <param name="localType">The <see cref="Type"/> of the local to declare.</param>
        /// <param name="localIndex">The index of the local declared.</param>
        /// <returns>The current <see cref="CodeMatcher"/>.</returns>
        public CodeMatcher DeclareLocal(Type localType, out int localIndex)
        {
            DeclareLocal(matcher, localType, out LocalBuilder builder);
            localIndex = builder.LocalIndex;
            return matcher;
        }

        /// <summary>
        /// Gets the first label at the current position.
        /// </summary>
        /// <param name="label">The first label at the current position.</param>
        /// <returns>The current <see cref="CodeMatcher"/>.</returns>
        public CodeMatcher GetFirstLabel(out Label label)
        {
            label = matcher.Labels.FirstOrDefault();
            return matcher;
        }

        /// <summary>
        /// Gets the first label at a specific position.
        /// </summary>
        /// <param name="position">The position to get label at.</param>
        /// <param name="label">The label at the position.</param>
        /// <returns>The current <see cref="CodeMatcher"/>.</returns>
        public CodeMatcher GetFirstLabelAt(int position, out Label label)
        {
            label = matcher.codes[position].labels.FirstOrDefault();
            return matcher;
        }
    }
}