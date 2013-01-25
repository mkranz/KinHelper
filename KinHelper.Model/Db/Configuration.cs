using System.Data.Entity.Migrations;
using System.Runtime.Remoting.Contexts;

namespace KinHelper.Model.Db
{
    public class Configuration : DbMigrationsConfiguration<KinHelperContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}