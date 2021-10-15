using Sandbox;

namespace SIXDOF
{
	public partial class SIXDOFPlayer : Player
	{
		[Net] public PawnController VehicleController { get; set; }
		[Net] public PawnAnimator VehicleAnimator { get; set; }
		[Net, Predicted] public ICamera VehicleCamera { get; set; }
		[Net, Predicted] public Entity Vehicle { get; set; }
		[Net, Predicted] public ICamera MainCamera { get; set; }

		public ICamera LastCamera { get; set; }


		/// <summary>
		/// The clothing container is what dresses the citizen
		/// </summary>
		public Clothing.Container Clothing = new();

		/// <summary>
		/// Default init
		/// </summary>
		public SIXDOFPlayer()
		{
		}

		/// <summary>
		/// Initialize using this client
		/// </summary>
		public SIXDOFPlayer( Client cl ) : this()
		{
			// Load clothing from client data
			Clothing.LoadFromClient( cl );
		}

		public override void Spawn()
		{
			MainCamera = new FirstPersonCamera();
			LastCamera = MainCamera;

			base.Spawn();
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();

			MainCamera = LastCamera;
			Camera = MainCamera;

			if ( DevController is NoclipController )
			{
				DevController = null;
			}

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Clothing.DressEntity( this );

			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			Particles.Create( "particles/impact.flesh.bloodpuff-big.vpcf", Position );
			Particles.Create( "particles/impact.flesh-big.vpcf", Position );
			PlaySound( "kersplat" );

			VehicleController = null;
			VehicleAnimator = null;
			VehicleCamera = null;
			Vehicle = null;

			LastCamera = MainCamera;
			MainCamera = new SpectateRagdollCamera();
			Camera = MainCamera;
			Controller = null;

			EnableAllCollisions = false;
			EnableDrawing = false;
		}

		public override void TakeDamage( DamageInfo info )
		{
			Health = 0;

			base.TakeDamage( info );
		}

		public override PawnController GetActiveController()
		{
			if ( VehicleController != null ) return VehicleController;
			if ( DevController != null ) return DevController;

			return base.GetActiveController();
		}

		public override PawnAnimator GetActiveAnimator()
		{
			if ( VehicleAnimator != null ) return VehicleAnimator;

			return base.GetActiveAnimator();
		}

		public ICamera GetActiveCamera()
		{
			if ( VehicleCamera != null ) return VehicleCamera;

			return MainCamera;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( Input.ActiveChild != null )
			{
				ActiveChild = Input.ActiveChild;
			}

			if ( LifeState != LifeState.Alive )
				return;

			if ( VehicleController != null && DevController is NoclipController )
			{
				DevController = null;
			}

			var controller = GetActiveController();
			if ( controller != null )
				EnableSolidCollisions = !controller.HasTag( "noclip" );

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );

			if ( Input.Pressed( InputButton.View ) )
			{
				if ( MainCamera is not FirstPersonCamera )
				{
					MainCamera = new FirstPersonCamera();
				}
				else
				{
					MainCamera = new ThirdPersonCamera();
				}
			}

			Camera = GetActiveCamera();
		}

		public void Catapult()
		{
			//
		}

		/*[ClientRpc]
		public void SetOpacity(float opacity)
		{
			SceneObject.SetValue( "transparency", opacity );
		}*/
	}
}
