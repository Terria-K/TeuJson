using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TeuJson.Generator;

public class QuoteWriter 
{
    private Dictionary<string, string> quotes = new Dictionary<string, string>();

    public void AddQuoteMacro(string quoteID, Func<string> quoteAct) 
    {
        quotes.Add(quoteID, quoteAct());
    }

    public string Quote(string quote) 
    {
        var full = new StringBuilder();
        var idBuilder = new StringBuilder();
        using TextReader reader = new StringReader(quote);

        while (Read(reader, out char read)) 
        {
            // Starts the token
            if (read == '<') 
            {
                if (Peek(reader, out read) && read == '#') 
                {
                    Read(reader, out read);
                    while (Peek(reader, out read) && read != '>') 
                    {
                        if (char.IsWhiteSpace(read)) 
                        {
                            Skip(reader);
                            continue;                           
                        }
                        idBuilder.Append(read);
                        Skip(reader);
                    }
                    Read(reader, out read);
                    full.Append(quotes[idBuilder.ToString()]);
                    idBuilder.Clear();
                    continue;
                }
                full.Append(read);
                continue;
            }
            full.Append(read);
        }
        return full.ToString();
    }

    private static bool Read(TextReader reader, out char read) 
    {
        int r = reader.Read();
        read = (char)r;
        return r != -1;
    }

    private static bool Peek(TextReader reader, out char peek) 
    {
        int p = reader.Peek();
        peek = (char)p;
        return p != -1;
    }

    private static bool Skip(TextReader reader) 
    {
        return Read(reader, out _);
    }
}