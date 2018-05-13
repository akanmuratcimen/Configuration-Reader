namespace ConfigurationReader.Abstraction {
    public class ConfigurationModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public object Value { get; set; }
    }
}