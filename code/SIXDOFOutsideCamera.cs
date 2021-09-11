using Sandbox;

namespace SIXDOF
{
	public class SIXDOFOutsideCamera : Camera
	{
		private Angles orbitAngles;
		private float orbitDistance = 150;
		private Angles PrevRotation;

		public override void Update()
		{
			var pawn = Local.Pawn as SIXDOFPlayer;

			if ( pawn == null )
				return;

			Pos = pawn.Position;

			Rot = orbitAngles.ToRotation();

			float distance = 130.0f * pawn.Scale;
			/*var targetPos = Input.Rotation.Forward * -distance;

			var tr = Trace.Ray( Pos, targetPos )
				.WorldOnly()
				.Radius( 8 )
				.Run();

			Pos = tr.EndPos;*/

			Pos += orbitAngles.ToRotation().Backward * orbitDistance;

			FieldOfView = 70;

			Viewer = null;
		}

		/*public override void BuildInput( InputBuilder input )
		{
			base.BuildInput( input );

			orbitAngles.yaw += input.AnalogLook.yaw;
			orbitAngles.pitch += input.AnalogLook.pitch;
			orbitAngles = orbitAngles.Normal;
			//orbitAngles.pitch = orbitAngles.pitch.Clamp( -89, 89 );

			var controller = (Local.Pawn as SIXDOFPlayer).Controller as SIXDOFController;
			if ( controller.HasInput )
			{
				PrevRotation = input.ViewAngles;
			}
			else
			{
				input.ViewAngles = PrevRotation = orbitAngles;
			}

			//input.Clear();
			//input.ViewAngles = Angles.Zero;

			input.StopProcessing = true;
		}*/
	}

}
