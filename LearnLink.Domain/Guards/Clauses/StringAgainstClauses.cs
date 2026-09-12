using LearnLink.Domain.Exceptions;

namespace LearnLink.Domain.Guards.Clauses;

/// <summary>
/// Extension of <see cref="AgainstClause{T}" for string clauses/>
/// </summary>
public static class StringAgainstClauses
{
    extension(AgainstClause<string> clause)
    {
        /// <summary>
        /// Clause against null or whitespace. Ensures that string is not null or whitespace. 
        /// </summary>
        /// <exception cref="ParameterRequiredException"></exception>
        /// <returns><see cref="AgainstClause{T}"/> example</returns>
        public AgainstClause<string> AgainstWhiteSpace()
        {
            if (string.IsNullOrWhiteSpace(clause.Value))
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }
            return clause;
        }

        /// <summary>
        /// Clause against null or empty. Ensures that string is not null or empty. 
        /// </summary>
        /// <exception cref="ParameterRequiredException"></exception>
        /// <returns><see cref="AgainstClause{T}"/> example</returns>
        public AgainstClause<string> AgainstEmpty()
        {
            if (string.IsNullOrEmpty(clause.Value))
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }
            return clause;
        }

        /// <summary>
        /// Clause against overflow. Ensures that string is not overflow max length. 
        /// </summary>
        /// <exception cref="ParameterOverflowException"></exception>
        /// <returns><see cref="AgainstClause{T}"/> example</returns>
        public AgainstClause<string> AgainstOverflow(int maxLength)
        {
            if (clause.Value?.Length > maxLength)
            {
                ParameterOverflowException.Throw(clause.ParameterName, clause.Value.Length, maxLength);
            }
            return clause;
        }
    }
}
