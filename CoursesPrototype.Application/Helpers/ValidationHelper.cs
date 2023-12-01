namespace CoursesPrototype.Application.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates that strings are not empty
        /// </summary>
        /// <param name="args">String values</param>
        /// <returns>True if all strings are not empty, false if any string is empty</returns>
        public static bool ValidateToEmptyStrings(params string?[] args)
        {
            foreach(var arg in args)
            {
                if(string.IsNullOrWhiteSpace(arg))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
