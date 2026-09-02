using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Exceptions;

namespace LearnLink.Domain.Guards.Clauses
{
    public static class GuardEmptyableExtensions
    {
        public static AgainstClause<T> AgainstEmpty<T>(this AgainstClause<T> clause) where T : IEmptyable<T>
        {
            if (T.IsEmpty(clause.Value))
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }

            return clause;
        }
    }
}
