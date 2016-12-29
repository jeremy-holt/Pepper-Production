namespace PCal.Models
{
  public class BaseDictionaryItem : IEntity
  {
    protected BaseDictionaryItem(string id, string name)
    {
      Id = id;
      Name = name;
    }

    protected BaseDictionaryItem()
    {
    }

    public string Name { get; set; }
    public string Description { get; set; }

    public string Id { get; set; }


    public override string ToString()
    {
      return $"Name: {Name}, Id: {Id}";
    }
  }
}
