namespace PopStudio.Trail
{
    internal class Trail
    {
        public int? MaxPoints;
        public float? MinPointDistance;
        public int TrailFlags;
        public object Image;
        public string ImageResource;
        public TrailTrackNode[] WidthOverLength;
        public TrailTrackNode[] WidthOverTime;
        public TrailTrackNode[] AlphaOverLength;
        public TrailTrackNode[] AlphaOverTime;
        public TrailTrackNode[] TrailDuration;
    }
}
