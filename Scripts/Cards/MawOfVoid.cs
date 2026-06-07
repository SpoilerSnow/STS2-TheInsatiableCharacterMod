using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class MawOfVoid : InsatiableCardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move)];
	public MawOfVoid()
		: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd
        .Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this)
        .Targeting(cardPlay.Target)
		.WithHitFx("vfx/vfx_bite")
		.Execute(choiceContext);
	}
	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card == this)
		{
			await PowerCmd.Apply<MawOfVoidPower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
		}
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(6);
	}
}