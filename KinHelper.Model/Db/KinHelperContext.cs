using System.Data.Entity;
using System.Linq;
using KinHelper.Model.Entities;

namespace KinHelper.Model.Db
{
    public class KinHelperContext : DbContext
    {
        public DbSet<Kinship> Kinships { get; set; }
        public DbSet<CharacterClass> Classes { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<KinshipMember> Members { get; set; }
        public DbSet<User> Users { get; set; }

        public Race GetOrCreateRace(string raceString)
        {
            var race = Races.FirstOrDefault(x => x.Name == raceString) ?? Races.Local.FirstOrDefault(x => x.Name == raceString);

            if (race == null)
            {
                race = new Race() { Name = raceString };
                Races.Add(race);
            }
            return race;
        }
        public Rank GetOrCreateRank(string rankString)
        {
            var rank = Ranks.FirstOrDefault(x => x.Name == rankString) ?? Ranks.Local.FirstOrDefault(x => x.Name == rankString); ;
            if (rank == null)
            {
                rank = new Rank() { Name = rankString };
                Ranks.Add(rank);
            }
            return rank;
        }

        public CharacterClass GetOrCreateClass(string classString)
        {
            var clazz = Classes.FirstOrDefault(x => x.Name == classString) ?? Classes.Local.FirstOrDefault(x => x.Name == classString); ;
            if (clazz == null)
            {
                clazz = new CharacterClass() { Name = classString };
                Classes.Add(clazz);
            }
            return clazz;
        }

        public User GetOrCreateUser(string lotroId, string hiddenId, string name)
        {
            var user = Users.FirstOrDefault(x => x.HiddenId == hiddenId) ?? Users.Local.FirstOrDefault(x => x.HiddenId == hiddenId);
            if (user == null)
            {
                user = new User() { Name = name, LotroId = lotroId, HiddenId = hiddenId };
                Users.Add(user);
            }
            return user;
        }

        public User GetOrCreateUserFromHidden(string hiddenUserId)
        {
            var user = Users.FirstOrDefault(x => x.HiddenId == hiddenUserId) ?? Users.Local.FirstOrDefault(x => x.HiddenId == hiddenUserId);
            if (user == null)
            {
                user = new User() { HiddenId = hiddenUserId };
                Users.Add(user);
            }
            return user;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<KinHelperContext,Configuration>());
        }
    }
}