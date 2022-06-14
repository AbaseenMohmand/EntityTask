namespace EntityTask.Models
{
    public class Unit
    {
        public int Id { get; set; }
        
        public string UnitName { get; set; }
        public List<ItemUnit> ItemUnit { get; set; }
    }
}
