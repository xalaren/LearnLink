using LearnLink.Domain.Exceptions;

namespace LearnLink.Domain.Guards.Clauses
{
    public static class GuardStringExtensions
    {
        public static AgainstClause<string> AgainstWhiteSpace(this AgainstClause<string> clause)
        {
            if (string.IsNullOrWhiteSpace(clause.Value))
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }
            return clause;
        }

        public static AgainstClause<string> AgainstEmpty(this AgainstClause<string> clause)
        {
            if (string.IsNullOrEmpty(clause.Value))
            {
                ParameterRequiredException.Throw(clause.ParameterName);
            }
            return clause;
        }

        public static AgainstClause<string> AgainstOverflow(this AgainstClause<string> clause, int maxLength)
        {
            if (clause.Value?.Length > maxLength)
            {
                ParameterOverflowException.Throw(clause.ParameterName, clause.Value.Length, maxLength);
            }
            return clause;
        }
    }
}
