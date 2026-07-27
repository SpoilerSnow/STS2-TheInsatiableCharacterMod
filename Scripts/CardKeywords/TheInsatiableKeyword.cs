using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace TheInsatiable.Scripts.CardKeywords;

[RegisterOwnedCardKeyword(nameof(Swallow), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(SelfSwallow), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Digest), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Dynamic), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Gulp), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Insect), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
public class TheInsatiableKeyword
{
    public static readonly CardKeyword Swallow = ModContentRegistry.GetQualifiedKeywordId("TheInsatiable", nameof(Swallow)).GetModCardKeyword();
    public static readonly CardKeyword SelfSwallow = ModContentRegistry.GetQualifiedKeywordId("TheInsatiable", nameof(SelfSwallow)).GetModCardKeyword();
    public static readonly CardKeyword Digest = ModContentRegistry.GetQualifiedKeywordId("TheInsatiable", nameof(Digest)).GetModCardKeyword();
    public static readonly CardKeyword Dynamic = ModContentRegistry.GetQualifiedKeywordId("TheInsatiable", nameof(Dynamic)).GetModCardKeyword();
    public static readonly CardKeyword Gulp = ModContentRegistry.GetQualifiedKeywordId("TheInsatiable", nameof(Gulp)).GetModCardKeyword();
    public static readonly CardKeyword Insect = ModContentRegistry.GetQualifiedKeywordId("TheInsatiable", nameof(Insect)).GetModCardKeyword();
}