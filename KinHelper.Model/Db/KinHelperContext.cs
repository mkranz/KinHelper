using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KinHelper.Model.Entities;

namespace KinHelper.Model.Db
{
    public class KinHelperContext : DbContext
    {
        private readonly List<CharacterClass> _classCache = new List<CharacterClass>();
        private readonly List<Race> _raceCache = new List<Race>();
        private readonly List<Rank> _rankCache = new List<Rank>();

        public DbSet<CharacterClass> Classes { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Rank> Ranks { get; set; }

        public DbSet<Kinship> Kinships { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<KinshipMember> Members { get; set; }
        public DbSet<User> Users { get; set; }

        public Race GetOrCreateRace(string raceString)
        {
            var race = _raceCache.FirstOrDefault(x => x.Name == raceString);
            if (race != null)
                return race;

            race = Races.FirstOrDefault(x => x.Name == raceString) ?? Races.Local.FirstOrDefault(x => x.Name == raceString);

            if (race == null)
            {
                race = new Race() { Name = raceString };
                Races.Add(race);
            }

            _raceCache.Add(race);
            return race;
        }
        public Rank GetOrCreateRank(string rankString)
        {
            var rank = _rankCache.FirstOrDefault(x => x.Name == rankString);
            if (rank != null)
                return rank;

            rank = Ranks.FirstOrDefault(x => x.Name == rankString) ?? Ranks.Local.FirstOrDefault(x => x.Name == rankString); ;
            if (rank == null)
            {
                rank = new Rank() { Name = rankString };
                Ranks.Add(rank);
            }

            _rankCache.Add(rank);
            return rank;
        }

        public CharacterClass GetOrCreateClass(string classString)
        {
            var clazz = _classCache.FirstOrDefault(x => x.Name == classString);
            if (clazz != null)
                return clazz;

            clazz = Classes.FirstOrDefault(x => x.Name == classString) ?? Classes.Local.FirstOrDefault(x => x.Name == classString); ;
            if (clazz == null)
            {
                clazz = new CharacterClass() { Name = classString };
                Classes.Add(clazz);
            }

            _classCache.Add(clazz);

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