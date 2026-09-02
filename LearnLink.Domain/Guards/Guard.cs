using System.Runtime.CompilerServices;

namespace LearnLink.Domain.Guards;

public static class Guard
{
    public static AgainstClause<T> For<T>(T parameter, [CallerArgumentExpression(nameof(parameter))] string expression = "")
    {
        return new AgainstClause<T>(parameter, expression);
    }
}