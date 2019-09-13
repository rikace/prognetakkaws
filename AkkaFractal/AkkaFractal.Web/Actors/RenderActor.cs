using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;
using Lib.AspNetCore.ServerSentEvents;
using AkkaFractal.Core;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static AkkaFractal.Core.ColorConsole;

namespace AkkaFractal.Web.Akka
{
    public class RenderActor : ReceivePersistentActor
    {
        string destination = @"./image.jpg";
        private Image<Rgba32> image;
        private int _eventCount;
        private List<RenderedTile> _state;
        private IServerSentEventsService serverSentEventsService;
        private int ys;
        private int xs;
        private int count = 0;

        public override string PersistenceId { get; } = "render-actor";

        public RenderActor(IServerSentEventsService serverSentEventsService, int width, int height, int split)
        {
            this.image = new Image<Rgba32>(width, height);
            this._state = new List<RenderedTile>();
            this.ys = height / split;
            this.xs = width / split;
            this.serverSentEventsService = serverSentEventsService;

            var totalMessages = ys + xs;
            WriteLineGreen($"YS {ys}");
            WriteLineGreen($"totalMessages {totalMessages}");

            Func<RenderedTile, Task> renderedTileAction = async tile =>
            {
                WriteLineGreen($"Received Message {++count}");
                totalMessages--;

                // var sseTile = new SseFormatTile(tile.X, tile.Y, Convert.ToBase64String(tile.Bytes));
                // var text = JsonConvert.SerializeObject(sseTile);
                // await serverSentEventsService.SendEventAsync(text);
                
                WriteLineCyan($"persisting Tile Message for X {tile.X} and Y {tile.Y}");

                Persist(tile, tileHandler =>
                {
                    WriteLineCyan($"persisted Tile Message for X {tile.X} and Y {tile.Y}");

                    _eventCount++;
                    _state.Add(tile);

                    if (_eventCount == 100)
                    {
                        WriteLineGray($"Saving snapshot for X {tile.X} and Y {tile.Y}");

                        SaveSnapshot(_state);

                        WriteLineGray($"Resetting snapshot event count to 0");

                        _eventCount = 0;
                    }
                });
            };

            Action<Completed> complete = _ =>
            {
                image.Save(destination);
                 WriteLineGreen("Tile render completed");
                 count = 0;
            };

            CommandAsync<RenderedTile>(renderedTileAction);
            Command<Completed>(complete);

             
            Recover<RenderedTile>(tile =>
            {
                WriteLineGreen($"Replaying RenderedTile for tile X {tile.X} and Y {tile.Y} from journal");
                _state.Add(tile);
                
                RunTask(async () => await ProcessTile(tile));
                
            });
            
            Recover<SnapshotOffer>(offer =>
            {
                WriteLineGreen($"Received SnapshotOffer from snapshot store, updating state");

                _state.AddRange((List<RenderedTile>) offer.Snapshot);

                WriteLineGreen($"State set from snapshot");
                foreach (var tile in (List<RenderedTile>) offer.Snapshot)
                {
                    RunTask(async () => await ProcessTile(tile));
                }
            });
        }

        private async Task ProcessTile(RenderedTile tile)
        {
            var sseTile = new SseFormatTile(tile.X, tile.Y, Convert.ToBase64String(tile.Bytes));
            var text = JsonConvert.SerializeObject(sseTile);
            await serverSentEventsService.SendEventAsync(text);

            WriteLineGreen($"Received Message {++count}");
			
            var tileImage = tile.Bytes.ToBitmap();
            var xt = 0;
            for (int x = 0; x < xs; x++)
            {
                int yt = 0;
                for (int y = 0; y < ys; y++)
                {
                    image[x + tile.X, y + tile.Y] = tileImage[x, y];
                    yt++;
                }

                xt++;
            }
        }
    }
}