using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class InsatiableNoEscape : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		HoverTipFactory.FromCard<InsatiableSwallow>()];
	public InsatiableNoEscape()
		: base(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<InsatiableNoEscapePower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
        List<CardPileAddResult> statusCards = new List<CardPileAddResult>();
			for (int i = 0; i < 6; i++)
			{
				CardModel card = base.CombatState.CreateCard<InsatiableSwallow>(base.Owner);
				PileType newPileType = (i < 3) ? PileType.Draw : PileType.Discard;
				List<CardPileAddResult> list = statusCards;
				list.Add(await CardPileCmd.AddGeneratedCardToCombat(card, newPileType, null, CardPilePosition.Random));
			}
            CardCmd.PreviewCardPileAdd(statusCards);
			await Cmd.Wait(1f);
	}
	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
