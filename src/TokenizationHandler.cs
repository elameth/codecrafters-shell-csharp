
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
        bool backSlashed = false, backSlashedInDoubleQuote = false; //this is way too specific of a bool probably

        foreach (var character in input)
        {
            
            //escape
            if (backSlashed)
            {
                backSlashed = false;
                currentToken.Append(character);
                continue;
            } 
            
            switch (character)
            {
                case '\'' when !inDoubleQuote && !backSlashed:
                    inSingleQuote = !inSingleQuote; 
                    continue;
                case '"' when !inSingleQuote  && !backSlashed:
                    inDoubleQuote = !inDoubleQuote; 
                    continue;
                case '\\' when !inDoubleQuote && !inSingleQuote && !backSlashed: //because backslash is already an escape character in windows
                case '\\' when inDoubleQuote:
                    backSlashed = !backSlashed;
                    continue;
            }

            if (char.IsWhiteSpace(character) && !inDoubleQuote && !inSingleQuote && !backSlashed)
            {
                if (currentToken.Length <= 0) continue;
                tokens.Add(currentToken.ToString());
                currentToken.Clear();
                continue;
            }
            //skip
            if (!backSlashed)
                currentToken.Append(character);
        }
        if (currentToken.Length > 0)
            tokens.Add(currentToken.ToString());
        return tokens;
    }
}