using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Exceptions;

namespace LearnLink.Domain.Guards.Clauses;

/// <summary>
/// Extension of <see cref="AgainstClause{T}" for emptyable clauses/>
/// </summary>
public static class EmptyableAgainstClauses
{
    extension<T>(AgainstClause<T> clause) where T : IEmptyable<T>
    {
        /// <summary>
        /// Clause against empty. Ensures that parameter is not empty. 
        /// </summary>
        /// <exception cref="ParameterRequiredException"></exception>
        /// <returns><see cref="AgainstClause{T}"/> example</returns>
        public AgainstClause<T> AgainstEmpty()
        {
            if (T.IsEmpty(clause.Value))
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }

            return clause;
        }
    }
}
