using Sandbox;

namespace SIXDOF
{
	public class SIXDOFController : BasePlayerController
	{
		public bool HasInput { get; private set; }  = false;

		public SIXDOFController()
		{
		}

		public override void FrameSimulate()
		{
			base.FrameSimulate();

			EyeRot = Input.Rotation;
		}

		public override void Simulate()
		{
			var vel = (Input.Rotation.Forward * Input.Forward) + (Input.Rotation.Left * Input.Left) + (Input.Rotation.Up * /*Input.Up*/ (Input.Down( InputButton.Jump ) ? 1 : Input.Down( InputButton.Duck ) ? -1 : 0));
			HasInput = !vel.IsNearZeroLength;

			/*if ( Input.Down( InputButton.Jump ) )
			{
				vel += Vector3.Up * 1;
			}*/

			vel = vel.Normal * 2000;

			if ( Input.Down( InputButton.Run ) )
				vel *= 5.0f;

			Velocity += vel * Time.Delta;

			//Velocity = Velocity.Approach( 0, Velocity.Length * Time.Delta * 5.0f );



			EyeRot = Input.Rotation;
			WishVelocity = Velocity;
			GroundEntity = null;
			BaseVelocity = Vector3.Zero;
		}
	}
}
