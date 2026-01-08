//? Utils from other places
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.CSharp.Syntax;
global using System.Collections.Immutable;

//? Static utils from other places
global using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
global using static Microsoft.CodeAnalysis.CSharp.SyntaxFacts;

//? Utils from SecretAPI
global using SecretAPI.CodeGeneration.CodeBuilders;
global using SecretAPI.CodeGeneration.Utils;
global using SecretAPI.CodeGeneration.Diagnostics;

//? Static utils from SecretAPI
global using static SecretAPI.CodeGeneration.Utils.GenericTypeUtils;
global using static SecretAPI.CodeGeneration.Utils.GeneratedIdentifyUtils;
global using static SecretAPI.CodeGeneration.Utils.MethodUtils;
global using static SecretAPI.CodeGeneration.Utils.TypeUtils;