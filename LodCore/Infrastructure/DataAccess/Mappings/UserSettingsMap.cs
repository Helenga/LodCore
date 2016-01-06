using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using UserPresentaton;

namespace DataAccess.Mappings
{
    public class UserSettingsMap : ClassMapping<UserSettings>
    {
        public UserSettingsMap()
        {
            Table("UserSettings");
            Id(settings => settings.UserId);
            ManyToOne(settings => settings.NotificationSettings, mapper =>
            {
                mapper.Cascade(Cascade.All);
                mapper.Class(typeof(NotificationSettings));
            });
        }
    }
}