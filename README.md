# Introduction
This repository is intended to aid in migrating API projects using [JsonApiDotNetCore](https://github.com/json-api-dotnet/JsonApiDotNetCore) from earlier versions.

# How to use
The [release notes](https://github.com/json-api-dotnet/JsonApiDotNetCore/releases) describe what has changed between versions.
But to see the effects, this repository contains an example ASP.NET API project.
By comparing the changes between branches in this repo, you can see how this example project was changed to work with the next major version.

### Upgrade from v3 to v4

The diff for upgrade of example project [from v3.1.0 to v4.0.0](https://github.com/json-api-dotnet/MigrationGuide/compare/release/3.1.0...release/4.0.0) shows how to convert with minimal change in behavior. It includes tests that assert on the response json.

Since v3, a lot of the internal workings have been optimized. Depending on how much custom code you wrote, the next table of renames may help to find back what you need.

| JsonApiDotNetCore v3  | JsonApiDotNetCore v4 |
| --- | --- |
| `BaseJsonApiController.GetRelationshipAsync` | `.GetSecondaryAsync` |
| `BaseJsonApiController.GetRelationshipsAsync` | `.GetRelationshipAsync` |
| `EntityResourceService` | `JsonApiResourceService` |
| `IEntityRepository` | `IResourceRepository` |
| `DefaultEntityRepository` | `EntityFrameworkCoreRepository` |
| `ResourceDefinition` | `JsonApiResourceDefinition` |
| `ContextEntity` | `ResourceContext` |
| `IContextGraph` | `IResourceGraph` |
| `RequestMiddleware` | `JsonApiMiddleware` |
| `IQueryParser` | `IQueryStringReader` |
| `RelationshipAttribute.(Dependent)Type` | `RelationshipAttribute.RightType` |
| `Constants.ContentType` | `HeaderConstants.MediaType` |


#### IJsonApiContext replacement
To improve separation of concerns, `IJsonApiContext` has been refactored and no longer exists.
The table below lists which interfaces to inject, in order to obtain the same information.

| `IJsonApiContext` Member  | Replacement |
| --- | --- |
| `Options` | `IJsonApiOptions` |
| `ResourceGraph` |`IResourceGraph` |
| `QuerySet`| `IEnumerable<IQueryConstraintProvider>`|
| `PageManager` | `IPaginationContext` |
| `AttributesToUpdate`, `RelationshipsToUpdate` | `ITargetedFields` |
| `HasOneRelationshipPointers`, `HasManyRelationshipPointers` | `RelationshipAttribute.GetValue()` |
| `BasePath` | `IJsonApiRequest.BasePath` |
| `RequestEntity` | `IJsonApiRequest.(SecondaryResource ?? PrimaryResource)` |
| `DocumentMeta` | `IResponseMeta` |
| `MetaBuilder` | `IResourceDefinition.GetMeta()` |
| `IsRelationshipPath` | `IJsonApiRequest.Kind` |
| `GenericProcessorFactory` | `IGenericServiceFactory` |
