using System;
using System.Linq;
using mc.CodeAlalysis;
using mc.CodeAlalysis.Syntax;

namespace mc
{
    internal class Program
    {
        private static void Main()
        {
            bool showTree = false;

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

                var color = Console.ForegroundColor;

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Print(expressionTree.Root);
                    Console.ForegroundColor = color;
                }

                if (expressionTree.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (string diagnostic in expressionTree.Diagnostics)
                        Console.WriteLine(diagnostic);
                    Console.ForegroundColor = color;
                }
                else
                {
                    var e = new Evaluator(expressionTree.Root);
                    var res = e.Evaluate();
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
