using TechTalk.SpecFlow;

namespace Tests.Steps
{
    [Binding]
    internal class ExampleStepDefinitions
    {
        [Given(@"Example '([^']*)'")]
        public void GivenExample(string example)
        {
            // Given example
        }

        [When(@"Example")]
        public void WhenExample()
        {
            // Given example
        }

        [Then(@"Example '([^']*)'")]
        public void ThenExample(string example)
        {
            // Given example
        }
    }
}
