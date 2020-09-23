using Xunit;
using AuthenticationNetCore.Test.Data;
using AuthenticationNetCore.Api.Utilities.RegexTools;

namespace AuthenticationNetCore
{
    public class RegexUtilitiesUnitTest
    {
        [Theory]
        [RegexUtilitiesTestDataSource]
        public void IsValidEmailTest(string email, bool isValid)
        {
            Assert.Equal(RegexUtilities.IsValidEmail(email), isValid);
        }
    }
}
