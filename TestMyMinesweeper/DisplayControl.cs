using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyMinesweeper
{
    public class DisplayControl
    {
        protected void Failure(string methodName, string elementName)
        {
            FailureGetElement("class " + this.GetType().Name + ", method " + methodName, elementName);
        }

        private void FailureGetElement(string where, string elementName)
        {
            Assert.Fail(where + " Error: \"" + elementName + "\" get failed");
        }
    }
}
