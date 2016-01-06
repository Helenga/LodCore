using Journalist;

namespace UserPresentaton
{
    public class UserSettings
    {
        public UserSettings(int userId, NotificationSettings notificationSettings)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotNull(notificationSettings, nameof(notificationSettings));

            UserId = userId;
            NotificationSettings = notificationSettings;
        }
        protected UserSettings() { }

        public virtual int UserId { get; protected set; }

        public virtual NotificationSettings NotificationSettings { get; protected set; }
    }
}