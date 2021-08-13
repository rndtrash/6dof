using Sandbox;

namespace SIXDOF
{
    class TPoseAnimator : PawnAnimator
    {
        public override void Simulate()
        {
            Rotation = Rotation.LookAt( Input.Rotation.Forward.WithZ( 0 ), Vector3.Up );

            SetLookAt( "aim_eyes", Pawn.EyePos + Input.Rotation.Forward * 200 );

            if ( Pawn.ActiveChild is BaseCarriable carry )
            {
                carry.SimulateAnimator( this );
            }
        }
    }
}
