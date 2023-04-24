using JetBrains.Annotations;

namespace ConsoleApi.Serialization.Abstractions;

/// <inheritdoc />
/// <summary>
/// Tout contrat doit être un record, donc une classe concrète (le serializer supporte mal les interfaces)
/// et donc, incompatible avec <see cref="JsonDeserializationContract"/> qui est aussi un record.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors)]
public abstract record JsonSerializationContract;

//TODO STATIC ANALYZER : Les propriétés d'un contrat ne peuvent pas être plus que init;
//TODO STATIC ANALYZER : Les propriétés d'un contrat sont un type de base ou un autre contrat