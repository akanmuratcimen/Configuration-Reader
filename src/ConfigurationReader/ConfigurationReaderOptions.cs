namespace ConfigurationReader {
    public class ConfigurationReaderOptions {
        public string ConnectionString { get; set; }
        public string ApplicationName { get; set; }
        public int RefreshTimerIntervalInMs { get; set; }
    }
}