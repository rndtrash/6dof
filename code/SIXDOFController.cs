using Sandbox;

namespace SIXDOF
{
	public class SIXDOFController : BasePlayerController
	{
		public bool HasInput { get; private set; } = false;

		public SIXDOFController()
		{
		}

		public override void Simulate()
		{
			var p = Pawn as SIXDOFPlayer;
			/*var angular = (p.PhysRotation.Forward * (Input.Down( InputButton.Menu ) ? -1 : Input.Down( InputButton.Use ) ? 1 : 0) + p.PhysRotation.Left * Input.MouseDelta.x + p.PhysRotation.Up * Input.MouseDelta.y) * 1000 * (Input.Down( InputButton.Run ) ? 5 : 1);

			p.PhysicsBody.ApplyAngularImpulse( angular );

			var vel = (p.PhysRotation.Forward * Input.Forward) + (p.PhysRotation.Left * Input.Left) + (p.PhysRotation.Up * (Input.Down( InputButton.Jump ) ? 1 : Input.Down( InputButton.Duck ) ? -1 : 0));
			HasInput = !vel.IsNearZeroLength;

			vel = vel.Normal * 2000;

			if ( Input.Down( InputButton.Run ) )
				vel *= 5.0f;

			p.PhysicsBody.ApplyForce(vel * 100);*/
		}
	}
}
