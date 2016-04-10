using HaloApp.Domain.Enums;
using System;

namespace HaloApp.Domain.Models.Metadata
{
    public class Medal
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MedalClassification Classification { get; set; }
        public int Difficulty { get; set; }
        public MedalSpriteLocation SpriteLocation { get; set; }
        public int Id { get; set; }

    }

    public class MedalSpriteLocation
    {
        public Uri SpriteSheetUri { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
    }
}
