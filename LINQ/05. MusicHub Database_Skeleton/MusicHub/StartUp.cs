namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = new MusicHubDbContext();
           // DbInitializer.ResetDatabase(context);

           // Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumInfo = context.Albums.Where(x => x.ProducerId == producerId).Select(album => new
            {
                AlbumName = album.Name,
                ReleaseDate = album.ReleaseDate,
                ProducerName = album.Producer.Name,
                Songs = album.Songs.Select(song => new
                {
                    SongName = song.Name,
                    Price = song.Price,
                    Writer = song.Writer.Name
                }).OrderByDescending(song => song.SongName).ThenBy(song => song.Writer).ToArray(),
                AlbumPrice = album.Price
            }).ToArray().OrderByDescending(x => x.AlbumPrice);

            StringBuilder sbAlbum = new StringBuilder();

            foreach (var album in albumInfo)
            {
                sbAlbum.AppendLine($"-AlbumName: {album.AlbumName}");
                sbAlbum.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sbAlbum.AppendLine($"-ProducerName: {album.ProducerName}");
                sbAlbum.AppendLine("-Songs:");

                for (int i = 0; i < album.Songs.Length; i++)
                {
                    var song = album.Songs[i];
                    sbAlbum.AppendLine($"---#{i + 1}");
                    sbAlbum.AppendLine($"---SongName: {song.SongName}");
                    sbAlbum.AppendLine($"---Price: {song.Price:f2}");
                    sbAlbum.AppendLine($"---Writer: {song.Writer}");
                }

                sbAlbum.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return sbAlbum.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs.Select(song => new
            {
                SongName = song.Name,
                Writer = song.Writer.Name,
                Performer = song.SongPerformers.Select(y=> y.Performer.FirstName + " " + y.Performer.LastName).ToList(),
                AlbumProducer = song.Album.Producer.Name,
                Duration = song.Duration
            }).ToArray().Where(x => x.Duration.TotalSeconds > duration).OrderBy(x => x.SongName).ThenBy(x => x.Writer).ThenBy(x => x.Performer);

            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{i++}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.Writer}");
                sb.AppendLine($"---Performer: {song.Performer.FirstOrDefault()}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration:c}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
