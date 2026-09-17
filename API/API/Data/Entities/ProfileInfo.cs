namespace API.Data.Entities
{
    public class ProfileInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? InstagramUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? YoutubeUrl { get; set; }
    }
}
