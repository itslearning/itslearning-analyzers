using Microsoft.CodeAnalysis.Diagnostics;

namespace Itslearning.Analyzers.UnitTesting
{
    public abstract class ItslearningUnitTestingAnalyzerBase : DiagnosticAnalyzer
    {
        protected NUnitContext NUnitContext;

        /// <summary>
        /// Note!!! Don't call it in descendant classes. Use <see cref="Analyze"/> instead.
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(compilationContext =>
            {
                NUnitContext = new NUnitContext(compilationContext.Compilation);
                if (!NUnitContext.IsPresent)
                    return;

                Analyze(compilationContext);
            });
        }

        /// <summary>
        /// Do actual analysis.
        /// </summary>
        protected abstract void Analyze(CompilationStartAnalysisContext context);
    }
}