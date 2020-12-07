using Common.Script.Utility;
using Common.Script.Visitors;
using Common.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Models
{
    public class ThingInstance
    {
        public string Id { get; set; }
        public bool IsIdGenerated { get; set; }
        public Thing Breed { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 MovementDelta { get; set; }
        public TimeSpan AnimationTime { get; set; }
        public virtual string StateString => $"Idle";
        public Region Region { get; set; }
        public Dictionary<string, Symbol> Variables { get; set; } = new Dictionary<string, Symbol>();

        protected Animation CurrentAnimation { get => Breed?.Model[StateString] ?? Animation.Empty; }
        protected Rectangle CurrentFrame { get => CurrentAnimation.FrameAt(AnimationTime).Source; }
        protected Vector2 DrawPosition { get => new Vector2((int)(Position.X - CurrentFrame.Width / 2), (int)(Position.Y - CurrentFrame.Height)); }
        protected Rectangle BoundingBox { get => new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, (int)Breed.Model.FrameSize.X, (int)Breed.Model.FrameSize.Y); }
        protected Rectangle BlockingBox
        {
            get => new Rectangle(
                (int)DrawPosition.X + Breed.Model.CollisionBox.X,
                (int)DrawPosition.Y + Breed.Model.CollisionBox.Y,
                Breed.Model.CollisionBox.Width,
                Breed.Model.CollisionBox.Height);
        }

        public virtual void Update(GameTime gameTime)
        {
            AnimationTime += gameTime.ElapsedGameTime;
            if (gameTime.IsRunningSlowly) {
            if (AnimationTime.TotalMilliseconds > CurrentAnimation.RoundDuration * 2)
                AnimationTime -= TimeSpan.FromMilliseconds(CurrentAnimation.RoundDuration) * 2;
            }

            if (MovementDelta.Length() > 0)
            {
                Position += MovementDelta;

                var tiles = GetTiles();
                var hasBlockingTile = tiles.Any(tile => tile == null || !tile.IsWalkable);
                if (hasBlockingTile)
                {
                    Position -= MovementDelta;
                }
                else
                {
                    var intersects = Region.instances.Any(instance => instance != this && instance.BlockingBox.Size != Point.Zero && BlockingBox.Intersects(instance.BlockingBox));
                    if (intersects)
                        Position -= MovementDelta;
                }
            }

            var elapsedTime = new Symbol("ElapsedTime", TypeSystem.Instance["Number"], gameTime.ElapsedGameTime.TotalMilliseconds.ToString());
            ExecutionVisitor.ExecuteRunBlock(this, "Updated", new List<Symbol>() { elapsedTime });
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Breed.Model.SpriteSheet.Value, DrawPosition, CurrentFrame, Color.White);

            if (EngineVariables.ShowEntityBoundingBox)
                spriteBatch.Draw(Assets.TransparentBox, BoundingBox, Color.White);
            if (EngineVariables.ShowEntityCollisionBox)
                spriteBatch.Draw(Assets.TransparentBox, BlockingBox, Color.Red);
        }

        private List<Tile> GetTiles()
        {
            var tiles = new List<Tile>();

            //add tiles for top-left and those between the corners
            for (int y = BlockingBox.Y; y < BlockingBox.Y + BlockingBox.Height; y += Region.TileHeight)
            {
                for (int x = BlockingBox.X; x < BlockingBox.X + BlockingBox.Width; x += Region.TileWidth)
                {
                    tiles.Add(Region.GetTileForPosition(x, y));
                }
            }

            //add tiles for top-right, bottom-left, bottom-right corners
            tiles.Add(Region.GetTileForPosition(BlockingBox.X + BlockingBox.Width, BlockingBox.Y));
            tiles.Add(Region.GetTileForPosition(BlockingBox.X, BlockingBox.Y + BlockingBox.Height));
            tiles.Add(Region.GetTileForPosition(BlockingBox.X + BlockingBox.Width, BlockingBox.Y + BlockingBox.Height));

            return tiles;
        }
        public ThingInstance ClosestInstance
        {
            get
            {
                var reachLength = Breed.Model.FrameSize.Length() / 2;
                var closestInstance = Region.instances
                    .Where(instance => instance != this && instance.GetType() != typeof(ThingInstance) && (Position - instance.Position).Length() < reachLength)
                    .OrderBy(instance => (Position - instance.Position).Length())
                    .FirstOrDefault();
                return closestInstance;
            }
        }
    }
}
