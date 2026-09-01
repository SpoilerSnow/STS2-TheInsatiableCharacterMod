using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;
using TheInsatiable.Scripts.Cards;


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
            await OnGulp(); 
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
    public virtual Task OnGulp()
    {
        return Task.CompletedTask;
    }
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
        if (base.Owner.PlayerCombatState.TurnNumber <= 1)
        {
            return;
        }
		PileType? currentPile = base.Pile?.Type;
		if (currentPile == Entry.SwallowPile && player == base.Owner)
		{
            await OnDigest();
		}
	}
    public virtual Task OnDigest()
    {
        return Task.CompletedTask;
    }
    public void FlashOnPlayer()
	{
		FlashOn();
	}
	public void FlashOn(Creature? target = null)
	{
		CardModel card = this.CardScope.CreateCard(ModelDb.GetById<CardModel>(((AbstractModel)this).Id), ((CardModel)this).Owner);
		TheInsatiableVfxCmd.CardFlashVfx(card, target);
	}
}
