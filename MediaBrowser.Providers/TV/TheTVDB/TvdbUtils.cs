using System;
using System.ComponentModel;
using MediaBrowser.Model.Entities;
using TvDbSharper.Dto;

namespace MediaBrowser.Providers.TV.TheTVDB
{
    /// <summary>
    /// Class TVUtils
    /// </summary>
    public static class TvdbUtils
    {
        /// <summary>
        /// The TVDB API key
        /// </summary>
        public static readonly string TvdbApiKey = "OG4V3YJ3FAP7FP2K";
        public static readonly string TvdbBaseUrl = "https://www.thetvdb.com/";
        /// <summary>
        /// The banner URL
        /// </summary>
        public static readonly string BannerUrl = TvdbBaseUrl + "banners/";

        public static ImageType GetImageTypeFromKeyType(string keyType)
        {
            switch (keyType.ToLowerInvariant())
            {
                case "poster":
                case "season": return ImageType.Primary;
                case "series":
                case "seasonwide": return ImageType.Banner;
                case "fanart": return ImageType.Backdrop;
                default: throw new ArgumentException($"Invalid or unknown keytype: {keyType}", nameof(keyType));
            }
        }

        public static string NormalizeLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return null;
            }

            // pt-br is just pt to tvdb
            return language.Split('-')[0].ToLowerInvariant();
        }
    }
}
