using Sandbox.UI;

namespace SIXDOF
{
	public partial class SIXDOFHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public SIXDOFHudEntity()
		{
			if ( IsClient )
			{
				RootPanel.SetTemplate( "/hud.html" );
				RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
				RootPanel.AddChild<VoiceList>();
				RootPanel.AddChild<NameTags>();
				RootPanel.AddChild<KillFeed>();
			}
		}
	}

}
