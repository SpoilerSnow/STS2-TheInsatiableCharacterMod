using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class InsatiableNoEscape : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		HoverTipFactory.FromCard<InsatiableSwallow>()];
	public InsatiableNoEscape()
		: base(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Salivate", 0f);
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
		await PowerCmd.Apply<InsatiableNoEscapePower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}