using NHibernate.Mapping.ByCode.Conformist;
using UserPresentaton;

namespace DataAccess.Mappings
{
    public class NotificationSettingsMapping : ClassMapping<NotificationSettings>
    {
        public NotificationSettingsMapping()
        {
            Id(settings => settings);
        }
    }
}