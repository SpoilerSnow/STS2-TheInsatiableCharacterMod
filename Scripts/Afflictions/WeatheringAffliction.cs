using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Afflictions;

[RegisterAffliction]
public class WeatheringAffliction : ModAfflictionTemplate
{
	public override bool HasExtraCardText => true;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
	public override async Task OnPlay(PlayerChoiceContext choiceContext, Creature? target)
	{
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), Card.Owner.Creature, Amount, Card.Owner.Creature, null);
	}
}