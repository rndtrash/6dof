using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIXDOF
{
    public class VehicleController : PawnController
	{
		public override void FrameSimulate()
		{
			base.FrameSimulate();

			Simulate();
		}

		public override void Simulate()
		{
			var player = Pawn as SIXDOFPlayer;
			if ( !player.IsValid() ) return;

			var vehicle = player.Vehicle as Vehicle;
			if ( !vehicle.IsValid() ) return;

			vehicle.Simulate( Client );

			if ( player.Vehicle == null )
			{
				Position = vehicle.Position + vehicle.Rotation.Up * (100 * vehicle.Scale);
				Velocity += vehicle.Rotation.Right * (200 * vehicle.Scale);
				return;
			}

			EyeRot = Input.Rotation;
			EyePosLocal = Vector3.Up * (64 - 10) * vehicle.Scale;
			Velocity = vehicle.Velocity;

			SetTag( "noclip" );
			SetTag( "sitting" );
		}
	}
}
