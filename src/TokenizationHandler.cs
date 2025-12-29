
public class TokenizationHandler
{

    //unless stuff gets really complicated, this will handle every possible input "rule" 
    //POSSIBLE BUG: only a single quote is in the entire string
    public static List<string>? Tokenize(string? input)
    {
        //should not be possible to get empty string, anyhow
        if (input == null)
            return null;
        
        var tokens = new List<string>();
        var currentToken = new System.Text.StringBuilder(); //the robot suggested this
        bool inSingleQuote = false, inDoubleQuote = false;

        foreach (var character in input)
        {
            switch (character)
            {
                //I had no idea how to convey this, my thanks to the robot
                case '\'' when !inDoubleQuote:
                    inSingleQuote = !inSingleQuote; 
                    continue;
                case '"' when !inSingleQuote:
                    inDoubleQuote = !inDoubleQuote; 
                    continue;
            }

            if (char.IsWhiteSpace(character) && !inDoubleQuote && !inSingleQuote)
            {
                if (currentToken.Length <= 0) continue;
                tokens.Add(currentToken.ToString());
                currentToken.Clear();
                continue;
            }
            currentToken.Append(character);
        }
        if (currentToken.Length > 0)
            tokens.Add(currentToken.ToString());
        return tokens;
    }
}