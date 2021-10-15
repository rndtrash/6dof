
using Sandbox;

namespace SIXDOF
{
	public partial class SIXDOFGame : Game
	{
		public SIXDOFGame()
		{
			if ( IsServer )
			{
				_ = new SIXDOFHudEntity();

				PhysicsWorld.Gravity = 0;
			}

			if ( IsClient )
			{
				//
			}
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new SIXDOFPlayer( client );
			client.Pawn = player;

			player.Respawn();

			var vehicle = new Vehicle();
			vehicle.Position += Vector3.Up * 200;
			vehicle.MountPilot( player );
		}
	}

}
