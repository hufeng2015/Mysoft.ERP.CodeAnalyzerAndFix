using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.ERP特殊规范.RM0015
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Rm0015DiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RM0015";

        private const string Category = "ERP特殊规范";

        private static readonly string Title = "61.CodeAnalyzer.SPEC:RM0015";

        private static readonly string MessageFormat = "AppService的方法必须用[ActionDescription]标记";

        private static readonly string Description = "ERP特殊规范>SPEC:RM0015;AppService的方法必须用[ActionDescription]标记";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description,
            CommonHelper.HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            var attributeDatas = methodSymbol.GetAttributes();

            //if (methodSymbol.ContainingType.BaseType != null &&
            //    methodSymbol.ContainingType.BaseType.Name == "AppService" &&
            //    attributeDatas.Any(x => x.AttributeClass.Name == "ActionDescriptionAttribute") == false)
            //{
            if (methodSymbol.DeclaredAccessibility == Accessibility.Public &&
                methodSymbol.ContainingType.Name.EndsWith("AppService") &&
                attributeDatas.Any(x => x.AttributeClass.Name == "ActionDescriptionAttribute") == false &&
                attributeDatas.Any(x => x.AttributeClass.Name == "ForbidHttpAttribute") == false)
            {
                //methodSymbol.ContainingType.BaseType!=null && methodSymbol.ContainingType.BaseType

                var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}