using System;
using System.Configuration;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace TaskList.Core
{
    /// <summary>
    /// Provides strongly-typed access to application settings defined in the configuration file.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Defines if we should secure cookies. Sets it to true always in prod, otherwise checks SSLCertInstalled. Prohibits users from logging in
        /// on PROD if no SSL cert is installed. 
        /// </summary>
        public static bool RequireSSLCookies
        {
            get { return GetOptionalValue<bool>(nameof(RequireSSLCookies), true); }
        }

        /// <summary>
        /// Gets the thumbprint that is used to find the correct Certificate
        /// installed on the machine.  This cert is then used to get a token
        /// to access the azure key vault.
        /// </summary>
        public static string AzureKeyVaultCertificateThumbprint
        {
            get { return GetRequiredValue<string>(nameof(AzureKeyVaultCertificateThumbprint)); }
        }

        /// <summary>
        /// Gets the ApplicationID as the azure key vault is also authenticated
        /// through azure ad, and we must assure that this is the appropriate application
        /// </summary>
        public static string AzureKeyVaultADApplicationID
        {
            get { return GetRequiredValue<string>(nameof(AzureKeyVaultADApplicationID)); }
        }

        /// <summary>
        /// Gets the base Uri that is used to determine the uris for each
        /// of the secrets being held in the vault
        /// </summary>
        public static string AzureKeyVaultBaseUri
        {
            get { return GetRequiredValue<string>(nameof(AzureKeyVaultBaseUri)); }
        }

        /// <summary>
        /// Gets the App Insights Instrumentation Key
        /// </summary>
        public static string APPINSIGHTS_INSTRUMENTATIONKEY
        {
            get { return GetRequiredValue<string>(nameof(APPINSIGHTS_INSTRUMENTATIONKEY)); }
        }

        /// <summary>
        /// Gets the Azure Blob Content Container Uri
        /// </summary>
        public static string AzureBlobContentContainerUri
        {
            get { return GetRequiredValue<string>(nameof(AzureBlobContentContainerUri)); }
        }

        /// <summary>
        /// Gets the Azure Blob Content Container SAS Token
        /// </summary>
        public static string AzureBlobContentContainerSAS
        {
            get
            {
                var uri = AzureBlobContentContainerUri;
                var sas = uri.Substring(uri.IndexOf('?'));
                return sas;
            }
        }

        /// <summary>
        /// Gets the SMTP Host
        /// </summary>
        public static string SmtpHost
        {
            get { return GetRequiredValue<string>(nameof(SmtpHost)); }
        }

        /// <summary>
        /// Gets the SMTP Port
        /// </summary>
        public static int SmtpPort
        {
            get { return GetRequiredValue<int>(nameof(SmtpPort)); }
        }

        /// <summary>
        /// Gets the SMTP SSL
        /// </summary>
        public static bool SmtpEnableSsl
        {
            get { return GetRequiredValue<bool>(nameof(SmtpEnableSsl)); }
        }

        /// <summary>
        /// Gets the SMTP Username
        /// </summary>
        public static string SmtpUsername
        {
            get { return GetRequiredValue<string>(nameof(SmtpUsername)); }
        }

        /// <summary>
        /// Gets the SMTP Password
        /// </summary>
        public static string SmtpPassword
        {
            get { return GetRequiredValue<string>(nameof(SmtpPassword)); }
        }

        /// <summary>
        /// Gets the number of days before the Term End Date to run the End Of Term Notification
        /// </summary>
        public static int EndOfTermNotification
        {
            get { return GetRequiredValue<int>(nameof(EndOfTermNotification)); }
        }

        public static string GOOGLE_CLIENT_ID
        {
            get { return GetRequiredValue<string>(nameof(GOOGLE_CLIENT_ID)); }
        }

        public static string FACEBOOK_APP_ID
        {
            get { return GetRequiredValue<string>(nameof(FACEBOOK_APP_ID)); }
        }

        public static string LINKEDIN_CLIENT_ID
        {
            get { return GetRequiredValue<string>(nameof(LINKEDIN_CLIENT_ID)); }
        }

        public static string LINKEDIN_CLIENT_SECRET
        {
            get { return GetRequiredValue<string>(nameof(LINKEDIN_CLIENT_SECRET)); }
        }

        public static string LINKEDIN_REDIRECT_URI
        {
            get { return GetRequiredValue<string>(nameof(LINKEDIN_REDIRECT_URI)); }
        }

        public static string MELISSA_KEY
        {
            get { return GetRequiredValue<string>(nameof(MELISSA_KEY)); }
        }

        public static string MELISSA_URI
        {
            get { return GetRequiredValue<string>(nameof(MELISSA_URI)); }
        }

        public static string DCIContactName
        {
            get { return GetRequiredValue<string>(nameof(DCIContactName)); }
        }

        public static string DCIContactEmail
        {
            get { return GetRequiredValue<string>(nameof(DCIContactEmail)); }
        }

        public static string BackgroundCheckLink
        {
            get { return GetRequiredValue<string>(nameof(BackgroundCheckLink)); }
        }

        public static string BaseUrl
        {
            get { return GetRequiredValue<string>(nameof(BaseUrl)); }
        }

        #region --- GetValue Methods ---

        private static ConcurrentDictionary<string, object> s_settings = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Retrieves the value of a required setting with a given name.
        /// An exception is thrown if the setting is not found.
        /// </summary>
        /// <typeparam name="T">Type of value to return.</typeparam>
        /// <param name="settingName">Name of the setting to retrieve.</param>
        /// <returns>The value of the given setting found in the configuration file.</returns>
        public static T GetRequiredValue<T>(string settingName)
        {
            return GetValue(settingName, true, default(T), null);
        }

        /// <summary>
        /// Retrieves the value of a required connection string with a given name.
        /// An exception is thrown if the setting is not found.
        /// </summary>
        /// <typeparam name="T">Type of value to return.</typeparam>
        /// <param name="connectionStringName">Name of the setting to retrieve.</param>
        /// <returns>The value of the given setting found in the configuration file.</returns>
        public static T GetRequiredValueConnectionString<T>(string connectionStringName)
        {
            return GetValue(connectionStringName, true, default(T), null, true);
        }

        /// <summary>
        /// Retrieves the value of a required setting with a given name and uses a specified function to transform the value.
        /// An exception is thrown if the setting is not found.
        /// </summary>
        /// <typeparam name="T">Type of value to return.</typeparam>
        /// <param name="settingName">Name of the setting to retrieve.</param>
        /// <param name="transformFunction">Function that will be called to transform the string value into the final value.</param>
        /// <returns>The value of the given setting found in the configuration file.</returns>
        private static T GetRequiredValue<T>(string settingName, Func<string, T> transformFunction)
        {
            return GetValue(settingName, true, default(T), transformFunction);
        }

        /// <summary>
        /// Retrieves the value of an optional setting with a given name, returning a default value if not found.        
        /// </summary>
        /// <typeparam name="T">Type of value to return.</typeparam>
        /// <param name="settingName">Name of the setting to retrieve.</param>
        /// <param name="defaultValue">Default value to return if the setting is not found.</param>
        /// <returns>The value of the given setting found in the configuration file.</returns>
        public static T GetOptionalValue<T>(string settingName, T defaultValue)
        {
            return GetValue(settingName, false, defaultValue, null);
        }

        /// <summary>
        /// Retrieves the value of an optional setting with a given name, returning a default value if not found.
        /// Uses a specified function to transform the string value to the final setting value.
        /// </summary>
        /// <typeparam name="T">Type of value to return.</typeparam>
        /// <param name="settingName">Name of the setting to retrieve.</param>
        /// <param name="defaultValue">Default value to return if the setting is not found.</param>
        /// <param name="transformFunction">Function that will be called to transform the string value into the final value.</param>
        /// <returns>The value of the given setting found in the configuration file.</returns>
        private static T GetOptionalValue<T>(string settingName, T defaultValue, Func<string, T> transformFunction)
        {
            return GetValue(settingName, false, defaultValue, transformFunction);
        }

        public static bool IsNullableType(Type type)
        {
            return (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// For types that implement <see cref="T:Nullable{}"/>, the underlying type is returned.
        /// For typee that do not implement <see cref="T:Nullable{}"/>, the specified type is returned.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns>The underlying non-nullable type; or the passed type if already not nullable.</returns>
        public static Type GetNonNullableType(Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetGenericArguments()[0];
            }
            else
            {
                return type;
            }
        }

        private static T GetValue<T>(string settingName, bool isRequired, T defaultValue, Func<string, T> transformFunction, bool fromConnectionString = false)
        {
            // try to get the value from our cache
            object result = s_settings.GetOrAdd(settingName, (key) =>
            {
                object value;

                // try to get the setting
                string strValue = string.Empty;
                if (fromConnectionString)
                {
                    strValue = ConfigurationManager.ConnectionStrings[settingName]?.ConnectionString;
                }
                else
                {
                    strValue = ConfigurationManager.AppSettings[settingName];
                }

                if (strValue != null)
                {
                    // replace any variables
                    strValue = ResolveAppSettingsVariables(strValue);

                    // replace secrets Variables
                    strValue = ResolveSecretsVariables(strValue);

                    // convert string value to method return type
                    if (transformFunction != null)
                    {
                        // execute custom transform function
                        try
                        {
                            value = transformFunction(strValue);
                        }
                        catch (Exception ex)
                        {
                            throw new ConfigurationErrorsException("Application setting '" + settingName + "' with value '" + strValue + "' failed transform to type '" + typeof(T) + ". Error: " + ex.Message, ex);
                        }
                    }
                    else // default conversion
                    {
                        if (typeof(T) == typeof(bool))
                        {
                            // handle the most common cases
                            switch (strValue.ToLower())
                            {
                                case "true":
                                case "1":
                                    value = true;
                                    break;
                                case "false":
                                case "0":
                                    value = false;
                                    break;
                                default:
                                    throw new ConfigurationErrorsException("Application setting '" + settingName + "' is invalid. Could not convert '" + strValue + "' to type '" + typeof(T) + ".");
                            }
                        }
                        else // not boolean
                        {
                            if (strValue.IsNullOrEmpty())
                            {
                                return default(T);
                            }

                            var nonNullType = GetNonNullableType(typeof(T));
                            try
                            {
                                value = Convert.ChangeType(strValue, nonNullType);
                            }
                            catch
                            {
                                throw new ConfigurationErrorsException("Application setting '" + settingName + "' is invalid. Could not convert '" + strValue + "' to type '" + typeof(T) + ".");
                            }
                        }
                    }
                }
                else // setting not found
                {
                    // make sure value was not required
                    if (isRequired)
                    {
                        throw new ConfigurationErrorsException("Application setting '" + settingName + "' is missing from configuration file and must be specified.");
                    }

                    // use the default value passed
                    value = defaultValue;
                }

                return value;
            });

            return (T)result;
        }

        /// <summary>
        /// Replaces each MSBuild-style variable (like '$(ProductSKU)') in the passed string with value from the AppSetting with the corresponding name.
        /// </summary>
        /// <param name="input">Value to be processed</param>
        /// <returns>Input value with all variables replaced with their AppSetting value equivalents.</returns>
        public static string ResolveAppSettingsVariables(string input)
        {
            //throw new NotImplementedException("TODO: Unit test then enable.");

            // nothing to do if input is empty
            if (input.IsNullOrEmpty())
            {
                return input;
            }

            // Find variables using syntax $(Value) like MSBuild
            var matches = Regex.Matches(input, @"\$\((?<name>[\w\.]+)\)", RegexOptions.Compiled);

            // stop if no variables found
            if (matches.Count == 0)
            {
                return input;
            }

            string output = input;
            foreach (Match match in matches)
            {
                string variableName = match.Value;
                string settingName = match.Groups["name"].Value;

                string settingValue = ConfigurationManager.AppSettings[settingName];
                if (settingValue == null)
                {
                    throw new ConfigurationErrorsException("Application setting '" + settingName + "' contains variable '" + variableName + "' which could not be resolved to an existing AppSetting with the same name.");
                }

                // recursively resolve variables in dependent settings?
                // would need to refactor this to make it capable of avoiding cyclic referencing

                output = output.Replace(variableName, settingValue);
            }

            return output;
        }

        /// <summary>
        /// Replaces the secrets variable {{DB_Password}} in the passed string with value from the Azure Key Vault
        /// </summary>
        /// <param name="input">The string to be parsed</param>
        /// <returns></returns>
        public static string ResolveSecretsVariables(string input)
        {
            //nothing to do if input is empty
            if (input.IsNullOrEmpty())
            {
                return input;
            }

            // Find variables using syntax {{value}}
            var matches = Regex.Matches(input, @"\{\{(?<name>[\w\.]+)\}\}", RegexOptions.Compiled);

            //stop if no variables found
            if (matches.Count == 0)
            {
                return input;
            }

            string output = input;
            foreach (Match match in matches)
            {
                string variableName = match.Value;
                string settingName = match.Groups["name"].Value;

                string settingValue = AzureKeyVaultUtil.GetSecretByName(settingName); //Create a method for getting based on Name
                if (settingValue == null)
                {
                    throw new ConfigurationErrorsException("Azure Key Vault does not contain a secret for " + settingName + ".");
                }

                // recursively resolve variables in dependent settings?
                // would need to refactor this to make it capable of avoiding cyclic referencing

                output = output.Replace(variableName, settingValue);
            }

            return output;
        }

        #endregion
    }
}
