using NEdifis.Conventions;

namespace NEdifis
{
    internal class CheckConventions : ConventionBase
    {
        protected override void Configure()
        {
            Conventions.Add(new ExcludeFromCodeCoverageClassHasBecauseAttribute());
            Conventions.Add(new AllClassesNeedATest());
            Conventions.Add(new ClassAndTestReferenceEachOther());
            Conventions.Add(new TestClassesShouldEndWithShould());
            Conventions.Add(new TestClassesShouldBePrivate());
        }
    }
}