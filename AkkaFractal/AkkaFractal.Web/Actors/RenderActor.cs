using System;
using System.Threading.Tasks;
using Akka.Actor;
using Lib.AspNetCore.ServerSentEvents;
using AkkaFractal.Core;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static AkkaFractal.Core.ColorConsole;

namespace AkkaFractal.Web.Akka
{
    public class RenderActor : ReceiveActor
    {
        string destination = @"./image.jpg";
        private Image<Rgba32> image;
        public RenderActor(IServerSentEventsService serverSentEventsService, 
                int width, int height, int split) 
        {
            image = new Image<Rgba32>(width, height);
            var ys = height / split;
            var xs = width / split;

            var totalMessages = ys + xs;
        
            WriteLineGreen($"totalMessages {totalMessages}");
  
            int count = 0;

            Func<RenderedTile, Task> renderedTileAction = async tile =>
            {
                var sseTile = new SseFormatTile(tile.X, tile.Y, Convert.ToBase64String(tile.Bytes));
                var text = JsonConvert.SerializeObject(sseTile);
                await serverSentEventsService.SendEventAsync(text);

                WriteLineGreen($"Received Message {++count}");

                totalMessages--;
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
            };

            Action<Completed> completeAction = _ =>
            {
                image.Save(destination);
                WriteLineGreen("Tile render completed");
            };

            // TODO lab 1 (a)
            // implement the two Actor Receiver(s) that handle the message types:
            // - RenderedTile
            // - Completed
            // The Lambdas have been already implemented above with the 
            // "renderedTileAction" and "completeAction" 
            //
            // - Add some console printing in each "action" to print a message
            //   that display the current Thread id.
			ReceiveAsync<RenderedTile>(renderedTileAction);
            Receive<Completed>(completeAction);
        }
    }
}