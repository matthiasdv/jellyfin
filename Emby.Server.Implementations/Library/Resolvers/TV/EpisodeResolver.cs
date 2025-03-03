#nullable disable

using System;
using System.Linq;
using Emby.Naming.Common;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Entities;

namespace Emby.Server.Implementations.Library.Resolvers.TV
{
    /// <summary>
    /// Class EpisodeResolver.
    /// </summary>
    public class EpisodeResolver : BaseVideoResolver<Episode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeResolver"/> class.
        /// </summary>
        /// <param name="namingOptions">The naming options.</param>
        public EpisodeResolver(NamingOptions namingOptions)
            : base(namingOptions)
        {
        }

        /// <summary>
        /// Resolves the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>Episode.</returns>
        public override Episode Resolve(ItemResolveArgs args)
        {
            var parent = args.Parent;

            if (parent == null)
            {
                return null;
            }

            // Just in case the user decided to nest episodes.
            // Not officially supported but in some cases we can handle it.

            var season = parent as Season ?? parent.GetParents().OfType<Season>().FirstOrDefault();

            // If the parent is a Season or Series and the parent is not an extras folder, then this is an Episode if the VideoResolver returns something
            // Also handle flat tv folders
            if ((season != null ||
                 string.Equals(args.GetCollectionType(), CollectionType.TvShows, StringComparison.OrdinalIgnoreCase) ||
                 args.HasParent<Series>())
                && (parent is Series || !BaseItem.AllExtrasTypesFolderNames.ContainsKey(parent.Name)))
            {
                var episode = ResolveVideo<Episode>(args, false);

                if (episode != null)
                {
                    var series = parent as Series ?? parent.GetParents().OfType<Series>().FirstOrDefault();

                    if (series != null)
                    {
                        episode.SeriesId = series.Id;
                        episode.SeriesName = series.Name;
                    }

                    if (season != null)
                    {
                        episode.SeasonId = season.Id;
                        episode.SeasonName = season.Name;
                    }

                    // Assume season 1 if there's no season folder and a season number could not be determined
                    if (season == null && !episode.ParentIndexNumber.HasValue && (episode.IndexNumber.HasValue || episode.PremiereDate.HasValue))
                    {
                        episode.ParentIndexNumber = 1;
                    }
                }

                return episode;
            }

            return null;
        }
    }
}
