using LearnLink.Core.Exceptions;

namespace LearnLink.Application.Helpers;

public class Permission(bool accessGranted)
{
    public bool AccessGranted { get; } = accessGranted;

    public void ThrowExceptionIfAccessNotGranted(string message = "Доступ отклонен")
    {
        if (!AccessGranted)
        {
            throw new AccessLevelException(message);
        }
    }
}