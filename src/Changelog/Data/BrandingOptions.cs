﻿namespace Changelog.Data
{
    public class BrandingOptions
    {
        /// <summary>
        /// The name of the section to read in appsettings.json
        /// </summary>
        public const string AppsettingsSectionName = "Branding";

        /// <summary>
        /// Name of this changelog system, e.g. "{Your company} changelog" 
        /// </summary>
        public string Name { get; set; }
    }
}
