using System;
using System.Text;

// Test the AddSecurityRequirementToOperations logic
var jsonContent = @"{""paths"":{""/{""get"":{""responses"":{""200"":{}}},""post"":{""responses"""":{""200"":{}}}}}}}";

Console.WriteLine("Original JSON length: " + jsonContent.Length);

// Find paths
int pathsIndex = jsonContent.IndexOf("\"paths\":{");
if (pathsIndex < 0) { Console.WriteLine("paths not found"); }
else { 
    Console.WriteLine("paths found at: " + pathsIndex);
    
    // Find end
    int startIndex = pathsIndex + 9;
    int openBraces = 1;
    int endIndex = startIndex;
    
    for (int i = startIndex; i < jsonContent.Length && openBraces > 0; i++)
    {
        if (jsonContent[i] == '{') openBraces++;
        else if (jsonContent[i] == '}') openBraces--;
        
        if (openBraces == 0)
            endIndex = i;
    }
    
    Console.WriteLine("paths section end: " + endIndex);
    string pathsSection = jsonContent.Substring(startIndex, endIndex - startIndex);
    Console.WriteLine("pathsSection: " + pathsSection);
    
    // Find operations
    string[] methods = { "\"get\":" };
    foreach (string method in methods)
    {
        int methodPos = 0;
        while ((methodPos = pathsSection.IndexOf(method, methodPos)) >= 0)
        {
            Console.WriteLine($"Found {method} at {methodPos}");
            
            int responsesStart = pathsSection.IndexOf("\"responses\":{", methodPos);
            Console.WriteLine($"  responses found at: {responsesStart}");
            
            if (responsesStart > 0)
            {
                // Count braces
                int braceCount = 1;
                int closePos = responsesStart + 13;
                
                while (closePos < pathsSection.Length && braceCount > 0)
                {
                    if (pathsSection[closePos] == '{') braceCount++;
                    else if (pathsSection[closePos] == '}') braceCount--;
                    closePos++;
                }
                
                Console.WriteLine($"  responses closes at: {closePos}");
                
                // Would inject here
                string injection = ",\"security\":[{\"Bearer\":[]}]";
                var modified = pathsSection.Insert(closePos - 1, injection);
                Console.WriteLine($"  would inject: {injection}");
                Console.WriteLine($"  new length: {modified.Length}");
            }
            
            methodPos += method.Length;
        }
    }
}
