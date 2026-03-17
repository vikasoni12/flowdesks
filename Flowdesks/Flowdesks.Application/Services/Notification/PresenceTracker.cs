namespace Flowdesks.Application.Services.Notification;

public class PresenceTracker
{
    private static readonly Dictionary<string, int> onlineUsers = new();

    public static Task ConnectionOpened(string connectionId)
    {
        lock (onlineUsers)
        {
            if (onlineUsers.ContainsKey(connectionId))
            {
                onlineUsers[connectionId] += 1;
            }
            else
            {
                onlineUsers.Add(connectionId, 1);
            }
        }

        return Task.CompletedTask;
    }

    public static Task ConnectionClosed(string connectionId)
    {
        lock (onlineUsers)
        {
            if (onlineUsers.ContainsKey(connectionId))
            {
                onlineUsers[connectionId] -= 1;
                if (onlineUsers[connectionId] <= 0)
                {
                    onlineUsers.Remove(connectionId);
                }
            }
        }

        return Task.CompletedTask;
    }

    public static Task<string[]> GetOnlineUsers()
    {
        lock (onlineUsers)
        {
            return Task.FromResult(onlineUsers.Keys.ToArray());
        }
    }
}