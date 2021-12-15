using RateSetter.Sources.Extensions;
using Xunit;

namespace RateSetter.Tests
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("hello world")]
        [InlineData("hEllo wOrld")]
        [InlineData("HELLO WORLD")]
        public void ToTitleCase_SuccessCases(string input)
        {
            const string expected = "Hello World";
            var actual = input.ToTitleCase();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("This is a sentence with multiple    spaces", "This is a sentence with multiple spaces")]
        [InlineData("This is     a sentence      with multiple    spaces in multiple places",
            "This is a sentence with multiple spaces in multiple places")]
        [InlineData(
            "    This is     a sentence      with multiple    spaces in multiple places and has space before and after sentence    ",
            "This is a sentence with multiple spaces in multiple places and has space before and after sentence")]
        public void TrimAndRemoveDuplicateSpaces_SuccessCases(string input, string expectResult)
        {
            var result = input.TrimSpecialCharacters().TrimDuplicateSpaces();
            Assert.True(string.Equals(result, expectResult));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("This is !@#$%^&*()?><-,_+=`~[];':\"/\\a sentence", "This is a sentence")]
        [InlineData("!@#$%^&*()?><-,_+=`~[];':\"/\\", " ")]
        public void TrimSpecialCharacters_SuccessCases(string input, string expectResult)
        {
            var result = input.TrimSpecialCharacters();
            Assert.True(string.Equals(result, expectResult));
        }
    }
}