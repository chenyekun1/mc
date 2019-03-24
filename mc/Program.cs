using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mc.CodeAlalysis;
using mc.CodeAlalysis.Binding;
using mc.CodeAlalysis.Syntax;
using mc.CodeAlalysis.Text;

namespace mc
{
    internal static class Program
    {
        private static void Main()
        {
            //Initialize the default variables
            var showTree    = false;
            var variables   = new Dictionary<VariableSymbol, object>();
            var textBuilder = new StringBuilder();

            while (true)
            {
                if (textBuilder.Length == 0)
                    Console.Write("> ");
                else
                    Console.Write("| ");

                var input   = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);
                textBuilder.Append(input);
                
                if (textBuilder.Length == 0)
                {
                    showTree = BuildinCommand(showTree, input);
                    continue;
                }

                var text = textBuilder.ToString();
                
                var expressionTree = SyntaxTree.Parse(text);

                if (!isBlank && expressionTree.Diagnostics.Any())
                    continue;

                var compilation = new Compilation(expressionTree);
                var evaluationResult = compilation.Evaluate(variables);
                var diagnostics = evaluationResult.Diagnostics;

                if (showTree)
                    DisplaySyntaxTree(expressionTree);

                if (diagnostics.Any())
                {
                    foreach (var diagnostic in diagnostics)
                    {
                        var lineIndex  = expressionTree.Text.GetLineIndex(diagnostic.Span.Strt);
                        var line       = expressionTree.Text.Lines[lineIndex];
                        var lineNumber = lineIndex + 1;
                        var character  = diagnostic.Span.Strt - line.Start + 1;

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Strt);
                        var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

                        var prefix = expressionTree.Text.ToString(prefixSpan);
                        var error = expressionTree.Text.ToString(diagnostic.Span);
                        var suffix = expressionTree.Text.ToString(suffixSpan);
                        Console.Write("    ");

                        Console.Write(prefix);
                        Console.ForegroundColor = ConsoleColor.DarkRed;

                        Console.Write(error);
                        Console.ResetColor();
                        Console.Write(suffix);
                        Console.WriteLine();
                    }
                }
                else
                {
                    var res = evaluationResult.Value;
                    Console.WriteLine(res);
                }

                textBuilder.Clear();
            }
        }

        private static void DisplaySyntaxTree(SyntaxTree expressionTree)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            expressionTree.Root.WriteTo(Console.Out);
            Console.ResetColor();
        }

        private static bool BuildinCommand(bool showTree, string line)
        {
            if (line.Equals("#showtree"))
            {
                showTree = !showTree;
                Console.WriteLine(showTree ? "Showing parser trees Turn on" : "Showing parser trees Turn off");
            }

            if (line.Equals("#cls"))
            {
                Console.Clear();
            }

            return showTree;
        }
    }
}
