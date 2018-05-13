namespace ConfigurationReader.Abstraction {
    public interface IConfiguration<TKey> {
        TKey Id { get; set; }
        string Name { get; set; }
        bool IsActive { get; set; }
        string ApplicationName { get; set; }
        object Value { get; set; }
    }
}