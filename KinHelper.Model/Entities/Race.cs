namespace KinHelper.Model.Entities
{
    public class Race : INamedEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}