using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;


namespace TheInsatiable.Scripts;

public abstract class InsatiableCardModel : ModCardTemplate, ITheInsatiableModel
{
	public virtual bool HasCustomPortrait => ResourceLoader.Exists($"res://TheInsatiable/images/cards/{GetType().Name}.png");
	public override string PortraitPath => HasCustomPortrait ? ($"res://TheInsatiable/images/cards/{GetType().Name}.png") : ($"res://TheInsatiable/images/cards/1.png");

	public InsatiableCardModel(int energyCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true) 
		: base(energyCost, type, rarity, target, showInCardLibrary)
	{
	}
	public interface IChoosable
	{
		Task<CardModel?> OnChosen(PlayerChoiceContext choiceContext);
	}
	public static readonly IReadOnlyList<IChoosable> choesnpile1 = new InsatiableCardModel.IChoosable[]
    {
        ModelDb.Card<InsatiableDrawPile>(),
        ModelDb.Card<InsatiableHandPile>(),
        ModelDb.Card<InsatiableDiscardPile>()
    };
	public static readonly IReadOnlyList<IChoosable> choesnpile2 = new InsatiableCardModel.IChoosable[]
    {
        ModelDb.Card<InsatiableDrawPile>(),
        ModelDb.Card<InsatiableDiscardPile>()
    };
	public virtual Task BeforeCardSwallow(CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual async Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        if (card == this)
        {
            await Gulp(); 
        }
        return;
    }
    public virtual Task BeforeCreatureSwallow(Creature creature, bool force)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force)
    {
        return Task.CompletedTask;
    }
    public virtual Task Gulp()
    {
        return Task.CompletedTask;
    }
    public override async Task AfterAutoPrePlayPhaseEnteredEarly(PlayerChoiceContext choiceContext, Player player)
	{
		CardPile? pile = base.Pile;
		if (pile != null && pile == Entry.SwallowPile.GetPile(player) && player == base.Owner)
		{
			await Digest(); 
		}
	}
    public virtual Task Digest()
    {
        return Task.CompletedTask;
    }
}
