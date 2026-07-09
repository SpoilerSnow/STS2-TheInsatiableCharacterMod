using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts;
[RegisterAffliction]
public class WeatheringAffliction : ModAfflictionTemplate
{
	public override bool HasExtraCardText => true;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Affliction is WeatheringAffliction)
		{
            int weatheringCount = cardPlay.Card.Affliction.Amount;
            decimal quickSandCount = weatheringCount;
			await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), cardPlay.Card.Owner.Creature, quickSandCount, cardPlay.Card.Owner.Creature, null);
        }
	}
}