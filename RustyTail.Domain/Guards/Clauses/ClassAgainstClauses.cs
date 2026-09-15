using RustyTail.Domain.Exceptions;

namespace RustyTail.Domain.Guards.Clauses;

/// <summary>
/// Extension of <see cref="AgainstClause{T}" for class clauses/>
/// </summary>
public static class ClassAgainstClauses
{
    extension<T>(AgainstClause<T> clause) where T : class
    {
        /// <summary>
        /// Clause against null. Ensures that parameter is not null. 
        /// </summary>
        /// <exception cref="ParameterRequiredException"></exception>
        /// <returns><see cref="AgainstClause{T}"/> example</returns>
        public AgainstClause<T> AgainstNull()
        {
            if (clause.Value is null)
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }

            return clause;
        }
    }
}
