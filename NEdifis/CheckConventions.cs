using NEdifis.Conventions;

namespace NEdifis
{
    class CheckConventions : ConventionBase
    {
        protected override void Configure()
        {
            Conventions.Add(new ExcludeFromCodeCoverageClassHasBecauseAttribute());
            Conventions.Add(new AllClassesNeedATest());
            Conventions.Add(new ClassAndTestReferenceEachOther());
            Conventions.Add(new TestClassesShouldEndWithShould());
        }
    }
}