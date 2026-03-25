using SourceKit.Generators.Builder.Annotations;

namespace ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record PostQuery(
    long? CursorPostId,
    [RequiredValue] int PageSize,
    long[] UserIds);
