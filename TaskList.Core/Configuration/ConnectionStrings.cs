namespace TaskList.Core
{
    public class ConnectionStrings
    {
        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        public static string DB
        {
            get { return AppSettings.GetRequiredValueConnectionString<string>(nameof(DB)); }
        }

        /// <summary>
        /// Gets the connection string with the given name.
        /// </summary>
        public static string Get(string connectionStringName)
        {
            return AppSettings.GetRequiredValueConnectionString<string>(connectionStringName);
        }
    }
}
