using Sandbox;

namespace SIXDOF
{
	partial class SIXDOFPlayer : Player
	{
		[Net] public Rotation PhysRotation { get; private set; }

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

			Camera = new SIXDOFInsideCamera();

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

			if ( Input.Pressed( InputButton.View ) )
			{
				using ( Prediction.Off() )
				{
					if ( Camera is not SIXDOFInsideCamera )
					{
						Camera = new SIXDOFInsideCamera();
					}
					else
					{
						Camera = new SIXDOFOutsideCamera();
						//Log.Info( "Fuck off, no 3rd person camera for you" );
					}
				}
			}

			if (IsServer)
			{
				PhysRotation = PhysicsBody.Rotation;
				DebugOverlay.ScreenText( $"{PhysRotation}\n{PhysicsBody.AngularVelocity}" );
			}

			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				var ragdoll = new ModelEntity();
				//ragdoll.SetModel( "models/pew.vmdl" );
				ragdoll.SetModel( "models/citizen/citizen.vmdl" );
				ragdoll.GlowActive = true;
				ragdoll.GlowColor = Color.Red;
				ragdoll.GlowState = GlowStates.GlowStateOn;
				ragdoll.Position = EyePos + PhysicsBody.Rotation.Forward * 40;
				ragdoll.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				ragdoll.PhysicsGroup.Velocity = PhysicsBody.Rotation.Forward * 1000;
			}

			DebugOverlay.Line( PhysicsBody.Position, PhysicsBody.Position + PhysicsBody.Velocity );
		}

		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
	}
}
