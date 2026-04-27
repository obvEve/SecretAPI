//? Utils from other places
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.CSharp.Syntax;
global using System.Collections.Immutable;

//? Static utils from other places
global using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
global using static Microsoft.CodeAnalysis.CSharp.SyntaxFacts;

//? Utils from SecretAPI
global using SecretAPI.SourceGenerators.Builders;
global using SecretAPI.SourceGenerators.Utils;

//? Static utils from SecretAPI
global using static SecretAPI.SourceGenerators.Utils.GenericTypeUtils;
global using static SecretAPI.SourceGenerators.Utils.GeneratedIdentifyUtils;
global using static SecretAPI.SourceGenerators.Utils.MethodUtils;
global using static SecretAPI.SourceGenerators.Utils.TypeUtils;