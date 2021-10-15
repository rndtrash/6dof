using Sandbox;
using System;

namespace SIXDOF
{
	public partial class Vehicle : Prop
	{
		[Net] public SIXDOFPlayer pilot { get; private set; }

		public Vector3 PilotMountingPoint { get; private set; }

		private struct InputState
		{
			public float throttle;
			public float yaw;
			public float breaking;
			public float pitch;
			public float roll;

			public void Reset()
			{
				throttle = 0;
				yaw = 0;
				breaking = 0;
				pitch = 0;
				roll = 0;
			}
		}

		private InputState currentInput;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/nighthawk/nighthawk.vmdl" );
			SetInteractsExclude( CollisionLayer.Player );
			EnableSelfCollisions = false;

			var att = GetAttachment( "pilot" );
			if ( !att.HasValue )
				throw new Exception( "Wrong model, has no pilot mounting point" );

			PilotMountingPoint = att.Value.Position;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if ( pilot is SIXDOFPlayer player )
			{
				RemovePilot( player );
			}
		}

		public void ResetInput()
		{
			currentInput.Reset();
		}

		[Event.Tick.Server]
		protected void Tick()
		{
			if ( pilot is SIXDOFPlayer player )
			{
				if ( player.LifeState != LifeState.Alive || player.Vehicle != this )
				{
					RemovePilot( player );
				}
			}
		}

		public override void Simulate( Client owner )
		{
			if ( owner == null ) return;
			if ( !IsServer ) return;

			using ( Prediction.Off() )
			{
				currentInput.Reset();

				if ( false ) // TODO: if the player decided to give up
				{
					if ( owner.Pawn is SIXDOFPlayer player )
					{
						player.Catapult();
						RemovePilot( player );

						return;
					}
				}

				currentInput.throttle = (Input.Down( InputButton.Forward ) ? 1 : 0) + (Input.Down( InputButton.Back ) ? -1 : 0);
				currentInput.yaw = (Input.Down( InputButton.Left ) ? 1 : 0) + (Input.Down( InputButton.Right ) ? -1 : 0);
				currentInput.breaking = (Input.Down( InputButton.Jump ) ? 1 : 0);
				currentInput.pitch = (Input.Down( InputButton.Run ) ? 1 : 0) + (Input.Down( InputButton.Duck ) ? -1 : 0);
				currentInput.roll = (Input.Down( InputButton.Menu ) ? 1 : 0) + (Input.Down( InputButton.Use ) ? -1 : 0);
			}
		}

		[Event.Physics.PreStep]
		public void OnPrePhysicsStep()
		{
			if ( !IsServer )
				return;

			var selfBody = PhysicsBody;
			if ( !selfBody.IsValid() )
				return;

			var body = selfBody.SelfOrParent;
			if ( !body.IsValid() )
				return;

			var dt = Time.Delta;

			body.DragEnabled = false;
			body.GravityEnabled = false;
		}

		public void MountPilot( SIXDOFPlayer player )
		{
			if ( player is SIXDOFPlayer && player.Vehicle == null )
			{
				player.Vehicle = this;
				player.VehicleController = new VehicleController();
				player.VehicleAnimator = new VehicleAnimator();
				//player.VehicleCamera = new CarCamera();
				player.Parent = this;
				player.LocalPosition = PilotMountingPoint;
				player.LocalRotation = Rotation.Identity;
				player.LocalScale = 1;
				player.PhysicsBody.Enabled = false;

				pilot = player;
			}
		}

		private void RemovePilot( SIXDOFPlayer player )
		{
			pilot = null;

			ResetInput();

			if ( !player.IsValid() )
				return;

			player.Vehicle = null;
			player.VehicleController = null;
			player.VehicleAnimator = null;
			player.VehicleCamera = null;
			player.Parent = null;

			if ( player.PhysicsBody.IsValid() )
			{
				player.PhysicsBody.Enabled = true;
				player.PhysicsBody.Position = player.Position;
			}
		}

		public override void StartTouch( Entity other )
		{
			base.StartTouch( other );

			if ( !IsServer )
				return;

			var body = PhysicsBody;
			if ( !body.IsValid() )
				return;

			body = body.SelfOrParent;
			if ( !body.IsValid() )
				return;

			if ( other is SIXDOFPlayer player && player.Vehicle == null )
			{
				var speed = body.Velocity.Length;
				var forceOrigin = Position + Rotation.Down * Rand.Float( 20, 30 );
				var velocity = (player.Position - forceOrigin).Normal * speed;
				var angularVelocity = body.AngularVelocity;

				OnPhysicsCollision( new CollisionEventData
				{
					Entity = player,
					Pos = player.Position + Vector3.Up * 50,
					Velocity = velocity,
					PreVelocity = velocity,
					PostVelocity = velocity,
					PreAngularVelocity = angularVelocity,
					Speed = speed,
				} );
			}
		}
	}
}
