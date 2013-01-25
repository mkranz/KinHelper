using System.Data.Entity;

namespace KinHelper.Model.Db
{
    public class RepositoryInitializer : DropCreateDatabaseIfModelChanges<KinHelperContext>
    {
        protected override void Seed(KinHelperContext context)
        {

        }
    }
}   