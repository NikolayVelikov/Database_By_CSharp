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

            DbInitializer.ResetDatabase(context);

            string result = ExportAlbumsInfo(context, 9);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumInfo = context.Producers
                .FirstOrDefault(x => x.Id == producerId)
                .Albums
                .Select(album => new
                {
                    AlbumName = album.Name,
                    ReleaseDate = album.ReleaseDate,
                    ProducerName = album.Producer.Name,
                    Songs = album.Songs.Select(song => new
                    {
                        SongName = song.Name,
                        Price = song.Price,
                        Writer = song.Writer.Name
                    }).OrderByDescending(x => x.SongName).ThenBy(w => w.Writer).ToArray(),
                    AlbumPrice = album.Price
                }).OrderByDescending(x => x.AlbumPrice).ToArray();

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
            throw new NotImplementedException();
        }
    }
}
