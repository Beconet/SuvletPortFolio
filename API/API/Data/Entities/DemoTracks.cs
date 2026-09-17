namespace API.Data.Entities
{
    public class DemoTracks
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // ปรับจาก bigint เป็น Guid (uuid)
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AudioUrl { get; set; } = string.Empty;
        public string? WaveformDataJson { get; set; }
    }
}
