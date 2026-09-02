using LearnLink.Domain.Exceptions;

namespace LearnLink.Domain.Guards.Clauses
{
    public static class GuardClassExtensions
    {
        public static AgainstClause<T> AgainstNull<T>(this AgainstClause<T> clause) where T: class
        {
            if(clause.Value is null)
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }

            return clause;
        }
    }
}
