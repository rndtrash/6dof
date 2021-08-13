using Sandbox;

namespace SIXDOF
{
	partial class SIXDOFPlayer : Player
	{
		public override void Spawn()
		{
			base.Spawn();

			MoveType = MoveType.Physics;
			CollisionGroup = CollisionGroup.Interactive;
			PhysicsEnabled = true;
			UsePhysicsCollision = true;
		}

		public override void Respawn()
		{
			Host.AssertServer();

			SetModel( "models/citizen/citizen.vmdl" );

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );

			Controller = new SIXDOFController();
			
			Animator = new TPoseAnimator();

			Camera = new SIXDOFOutsideCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			LifeState = LifeState.Alive;
			Health = 100;
			Velocity = Vector3.Zero;
			WaterLevel.Clear();

			Game.Current?.MoveToSpawnpoint( this );
			ResetInterpolation();

			PhysicsBody.DragEnabled = false;
			PhysicsBody.LinearDrag = 0;
			PhysicsBody.AngularDrag = 0;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			SimulateActiveChild( cl, ActiveChild );

			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				var ragdoll = new ModelEntity();
				ragdoll.SetModel( "models/citizen/citizen.vmdl" );
				ragdoll.Position = EyePos + EyeRot.Forward * 40;
				ragdoll.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				ragdoll.PhysicsGroup.Velocity = EyeRot.Forward * 1000;
			}

			DebugOverlay.Line( Position, Position + Velocity );
		}

		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
	}
}
