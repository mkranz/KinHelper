namespace KinHelper.Model.Entities
{
    public class KinshipMember
    {
        public int Id { get; set; }
        public virtual Kinship Kinship { get; set; }
        public virtual Character Character { get; set; }
        public virtual Rank Rank { get; set; }

        public override string ToString()
        {
            return string.Format("Character: {0}, Rank: {1}", Character, Rank);
        }
    }
}