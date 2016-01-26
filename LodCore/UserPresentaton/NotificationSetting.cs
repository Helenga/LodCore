﻿using Journalist;

namespace UserPresentaton
{
    public class NotificationSetting
    {
        public virtual int SettingId { get; protected set; }

        public virtual int UserId { get; protected set; }

        public virtual NotificationType NotificationType { get; protected set; }

        public virtual NotificationSettingValue Value { get; protected set; }
        
        protected NotificationSetting()
        {
            
        }

        public NotificationSetting(int userId, NotificationType notificationType, NotificationSettingValue value)
        {
            Require.NotNull(notificationType, nameof(notificationType));
            Require.NotNull(value, nameof(value));
            Require.Positive(userId, nameof(userId));

            UserId = userId;
            NotificationType = notificationType;
            Value = value;
        }
    }
}