public interface IEdition
{
    int id { get; }
    string name { get; set; }
    string displayName { get; set; }
    string englishName { get; set; }
    int module { get; }
}