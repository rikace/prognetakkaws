using System;
using Akka.Actor;
using Akka.Persistence;
using Lib.AspNetCore.ServerSentEvents;
using AkkaFractal.Core;
using static AkkaFractal.Core.ColorConsole;


namespace AkkaFractal.Web.Akka
{
    public delegate IActorRef SseTileActorProvider();

    public class SseTileActor : ReceivePersistentActor
    {
        private IActorRef renderActor;
        private RenderImage _state;
        
        public override string PersistenceId { get; } = "sse-actor";

        public SseTileActor(IServerSentEventsService serverSentEventsService, IActorRef tileRenderActor)
        {
            var split = 20;
            
            Recover<RenderImage>(request =>
            {
                WriteLineGreen($"Replaying RenderImage from journal");
                _state = request;
                
                if (Context.Child("RenderActor").Equals(ActorRefs.Nobody))
                {
                    WriteLineYellow($"Creating child actor RenderActor");

                    renderActor =
                        Context.ActorOf(
                            Props.Create(() =>
                                new RenderActor(serverSentEventsService, request.Width, request.Height, split)),
                            "RenderActor");
                }
            });
            
            
            Recover<SnapshotOffer>(offer =>
            {
                WriteLineCyan($"Received SnapshotOffer from snapshot store, updating state");

                _state = (RenderImage)offer.Snapshot;

                WriteLineCyan($"State set to Height {_state.Height} and to Width {_state.Width} from snapshot");
            });
            
            Command<RenderImage>(request =>
            {
                _state = request;
                
                var ys = request.Height / split;
                var xs = request.Width / split;

                WriteLineYellow($"Starting image processing size Width {request.Width} - Height {request.Height}");
                
                // TODO lab 1 (a)
                // Complete the "RenderActor" actor located inside the "Akka" folder.
                // This Actor should receive two message types:
                // - "RenderedTile" that uses the local lambda "renderedTileAction"
                // - "Complete" that triggers the local lambda "complete"
                // 
                // replace the following line of code "Nobody.Instance" with the
                // instantiation of the renderActor IActorRef as a Child of this current Actor "SseTileActor".
                //
                // NOTE: This actor should be implemented only once if and only if 
                //       it is not already instantiated
                //         (you could use the local "Context" to check if an actor with the same name already exist) 
                // NOTE: To create a Child Actor you should use the current "Context"

                // renderActor = Nobody.Instance;

                if (Context.Child("RenderActor").Equals(ActorRefs.Nobody))
                {
                    WriteLineYellow($"Creating child actor RenderActor");

                    renderActor =
                        Context.ActorOf(
                            Props.Create(() =>
                                new RenderActor(serverSentEventsService, request.Width, request.Height, split)),
                            "RenderActor");
                }

                for (var y = 0; y < split; y++)
                {
                    var yy = ys * y;
                    for (var x = 0; x < split; x++)
                    {
                        var xx = xs * x;
                        
                        // TODO lab 1 (b)
                        // pass the previously instantiated "renderActor" IActorRef as the "Sender"
                        // of the following "tileRenderActor" Message-Payload.
                        // in this way, when the "tileRenderActor" completes the computation,
                        // the response sent with the  "Sender.Tell" will be sent
                        // to the "renderActor" actor rather then the current "SseTileActor"
                        tileRenderActor.Tell(new RenderTile(yy, xx, xs, ys, request.Height, request.Width)
                            , renderActor);
                    }
                    
                    // TODO lab (b) - Supervision  
                    // implement the "SupervisorStrategy" in the "tileRenderActor" actor
                    // then uncomment these lines of code to test
                      // if (y / split == 0)
                      // {
                      //      WriteLineCyan($"simulating Actor crash");
                      //      tileRenderActor.Tell(new SimulateError());
                      // }
                }

                // TODO lab 1 (b) - same as previous "TODO lab 1 (b)"
                // 
                // pass the previously instantiated "renderActor" IActorRef as the "Sender"
                // of the following "tileRenderActor" Message-Payload.
                // In this way, when the "tileRenderActor" completes the computation,
                // the response sent with the "Sender.Tell" will be sent
                // to the "renderActor" actor rather then the current "SseTileActor"

                tileRenderActor.Tell(new Completed(), renderActor);
                WriteLineYellow($"Image processing completed");
            });
        }

        // TODO Lab2 : Supervision
        // implement the "SupervisorStrategy" to Restart the actor in case of 
        // "ArgumentException" 
        protected override SupervisorStrategy SupervisorStrategy()
        {  
            return base.SupervisorStrategy();
        }
    }
}