using SourceKit.Generators.Builder.Annotations;

namespace ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record UserQuery(long[] UserIds);
