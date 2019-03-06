using System;
using System.Collections.Generic;
using System.Linq;
using mc.CodeAlalysis;
using mc.CodeAlalysis.Binding;
using mc.CodeAlalysis.Syntax;

namespace mc
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;

            //Initialize the default variables
            var variables = new Dictionary<VariableSymbol, object>();

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                if (line.Equals("#showtree"))
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parser trees Turn on" : "Showing parser trees Turn off");
                    continue;
                }

                if (line.Equals("#cls"))
                {
                    Console.Clear();
                    continue;
                }
                
                var expressionTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(expressionTree);
                var evaluationResult = compilation.Evaluate(variables);
                var diagnostics = evaluationResult.Diagnostics;

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Print(expressionTree.Root);
                    Console.ResetColor();
                }

                if (diagnostics.Any())
                {
                    
                    foreach (var diagnostic in diagnostics)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefix = line.Substring(0, diagnostic.Span.Strt);
                        var error = line.Substring(diagnostic.Span.Strt, diagnostic.Span.Len);
                        var suffix = line.Substring(diagnostic.Span.End);
                        Console.Write("    ");

                        Console.Write(prefix);
                        Console.ForegroundColor = ConsoleColor.Red;
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
            }
        }

        public static void Print(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();
            indent += isLast ? "    " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                Print(child, indent, child == lastChild);
        }
    }
}
