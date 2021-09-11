using Sandbox;

namespace SIXDOF
{
	public partial class SIXDOFInsideCamera : Camera
	{
		Vector3 lastPos;
		[Net] Rotation physRot { get; set; }

		public override void Activated()
		{
			var pawn = Local.Pawn as SIXDOFPlayer;
			if ( pawn == null ) return;

			Pos = pawn.PhysicsBody.Position;
			Rot = pawn.PhysRotation;

			lastPos = Pos;
		}

		public override void Update()
		{
			var pawn = Local.Pawn as SIXDOFPlayer;
			if ( pawn == null ) return;

			physRot = pawn.PhysRotation;

			var eyePos = pawn.PhysicsBody.Position;
			if ( eyePos.Distance( lastPos ) < 300 ) // TODO: Tweak this, or add a way to invalidate lastpos when teleporting
			{
				Pos = Vector3.Lerp( eyePos.WithZ( lastPos.z ), eyePos, 20.0f * Time.Delta );
			}
			else
			{
				Pos = eyePos;
			}

			Rot = pawn.PhysRotation;

			FieldOfView = 80;

			Viewer = pawn;
			lastPos = Pos;
		}
	}
}
